using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lab1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string path = Path();
            char[] text = Reader(path);
            char[] text64 = Encoding64(File.ReadAllBytes(path));
            Console.WriteLine("\nТекстовий файл:");
            Chance(text);
            Console.WriteLine("\nТекстовий файл в base64:");
            Chance(text64);
            foreach (char symbol in text64)
                Console.Write(symbol);

            char[] commpressed = Reader(path.Split('.')[0] + ".gz");
            char[] commpressed64 = Encoding64(File.ReadAllBytes(path.Split('.')[0] + ".gz"));
            Console.WriteLine("\n\nСтиснутий файл:");
            Chance(commpressed);
            Console.WriteLine("\nСтиснутий файл в base64:");
            Chance(commpressed64);
            foreach (char symbol in text64)
                Console.Write(symbol);

        }
        static string Path()
        {
            Console.WriteLine("Введіть номер [1-3]:");
            string path;
            try
            {
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        path = @"M:\Labs\CompSys\Lab1\Texts\EVO.txt";
                        break;
                    case 2:
                        path = @"M:\Labs\CompSys\Lab1\Texts\TBB.txt";
                        break;
                    case 3:
                        path = @"M:\Labs\CompSys\Lab1\Texts\WIH.txt";
                        break;
                    default:
                        Console.WriteLine("Введено не правильне значення. За замовчування використовується 1");
                        path = @"M:\Labs\CompSys\Lab1\Texts\EVO.txt";
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Введено не правильне значення. За замовчування використовується 1");
                path = @"M:\Labs\CompSys\Lab1\Texts\EVO.txt";
            }
            return path;
        }
        static void Chance(char[] text)
        {
            int lenght = text.Count(char.IsLetter);
            Dictionary<char, double> letter = new Dictionary<char, double>();

            foreach (var symbol in text) // all symbols in text
            {
                if (letter.ContainsKey(symbol))
                    letter[symbol]++;
                else letter[symbol] = 1;
            }

            double chance;
            double entropy = 0;
            for (int i = 0; i < letter.Count; i++) // entrophy
            {
                chance = letter.ElementAt(i).Value / lenght;
                entropy += chance * Math.Log(1.0 / chance, 2);
            }

            Console.WriteLine("Ентропія = {0:f3}", entropy);
            Console.WriteLine("Кількість інформації в тексті = {0:f0} байт", entropy * lenght / 8);
        }
        static char[] Reader(string path)
        {
            StreamReader reader = new StreamReader(path, System.Text.Encoding.GetEncoding(1251));
            string text = "";
            while (reader.Peek() != -1)
            {
                text += reader.ReadLine();
            }
            text = text.ToLower();

            char[] arr = new char[text.Length];
            for (int i = 0; i < text.Length; i++) // text string to char[]
                arr[i] = text[i];

            return arr;
        }
        private static char CharTable(byte b)
        {
            char[] indexTable = new char[64] {
        'A','B','C','D','E','F','G','H','I','J','K','L','M',
        'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
        'a','b','c','d','e','f','g','h','i','j','k','l','m',
        'n','o','p','q','r','s','t','u','v','w','x','y','z',
        '0','1','2','3','4','5','6','7','8','9','+','/'};

            if ((b >= 0) && (b <= 63))
            {
                return indexTable[b];
            }
            else
            {
                return ' ';
            }
        }
        public static char[] Encoding64(byte[] data)
        {

            int length, length2;
            int blockCount;
            int paddingCount;
            length = data.Length;

            if ((length % 3) == 0)
            {
                paddingCount = 0;
                blockCount = length / 3;
            }
            else
            {
                paddingCount = 3 - (length % 3);
                blockCount = (length + paddingCount) / 3;
            }

            length2 = length + paddingCount;

            byte[] source2 = new byte[length2];

            for (int x = 0; x < length2; x++)
            {
                if (x < length)
                {
                    source2[x] = data[x];
                }
                else
                {
                    source2[x] = 0;
                }
            }

            byte b1, b2, b3; 
            byte temp, temp1, temp2, temp3, temp4;
            byte[] buffer = new byte[blockCount * 4];
            char[] result = new char[blockCount * 4];

            for (int x = 0; x < blockCount; x++)
            {
                b1 = source2[x * 3];
                b2 = source2[x * 3 + 1];
                b3 = source2[x * 3 + 2];

                temp1 = (byte)((b1 & 252) >> 2);
                foreach (byte aa in source2)
                    Console.WriteLine(aa);

                temp = (byte)((b1 & 3) << 4);
                temp2 = (byte)((b2 & 240) >> 4);
                temp2 += temp;

                temp = (byte)((b2 & 15) << 2);
                temp3 = (byte)((b3 & 192) >> 6);
                temp3 += temp;

                temp4 = (byte)(b3 & 63);

                buffer[x * 4] = temp1;
                buffer[x * 4 + 1] = temp2;
                buffer[x * 4 + 2] = temp3;
                buffer[x * 4 + 3] = temp4;

            }

            for (int x = 0; x < blockCount * 4; x++)
            {
                result[x] = CharTable(buffer[x]);
            }

            switch (paddingCount)
            {
                case 0:
                    break;
                case 1:
                    result[blockCount * 4 - 1] = '=';
                    break;
                case 2:
                    result[blockCount * 4 - 1] = '=';
                    result[blockCount * 4 - 2] = '=';
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
