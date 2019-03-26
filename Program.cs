using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lab1_1
{
    class Program
    {
        static char[] alphabet_ = new char[33] {'а', 'б', 'в', 'г', 'ґ', 'д', 'е', 'є', 'ж', 'з', 'и',
                                            'і', 'ї', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с',
                                            'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string path = Path();
            string text = Reader(path);
            Chance(text);

            string[] extension = new string[] { ".txt", ".gz", ".tar", ".zip", ".rar", ".xz" };
            for (int i = 0; i < extension.Length; i++)
            {
                FileInfo file = new FileInfo(path.Split('.')[0] + extension[i]);
                double size = file.Length;
                Console.WriteLine("Використання " + extension[i] + " створює документ" +
                    " розміром {0:f0} байт", (size));
            }
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
        static string Reader(string path)
        {
            StreamReader reader = new StreamReader(path, Encoding.GetEncoding(1251));
            string text = "";
            while (reader.Peek() != -1)
            {
                text += reader.ReadLine();
            }
            text = text.ToLower();

            return text;
        }
        static void Chance(string text)
        {
            int lenght = text.Count(Char.IsLetter);
            Dictionary<char, double> letter = new Dictionary<char, double>();
            for (int i = 0; i < alphabet_.Length; i++)
            {
                letter[alphabet_[i]] = 0;
            }
            foreach (var symbol in text)
            {
                if (letter.ContainsKey(symbol))
                    letter[symbol]++;
            }

            Console.WriteLine("Імовірність появи символу:");
            double value;
            double entropy = 0;
            for (int i = 0; i < letter.Count; i++)
            {
                if (letter.ElementAt(i).Value != 0)
                {
                    letter.TryGetValue(alphabet_[i], out value);
                    Console.WriteLine(" - " + alphabet_[i] + " = {0:f2} %", 100 * value / lenght);
                    entropy += (value / lenght) * Math.Log(1.0 / (value / lenght), 2);
                }
            }
            Console.WriteLine("\nЕнтропія = {0:f3}", entropy);
            Console.WriteLine("Кількість інформації в тексті = {0:f0} байт\n", entropy * lenght / 8);

        }
    }
}
