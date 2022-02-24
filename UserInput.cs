using System.Data;

namespace learn;

public class UserInput
{
    public static int GetIntInRange(int min, int max)
    {
        int res;

        while (!int.TryParse(Console.ReadLine(), out res) || res < min || res > max)
        {
            Writers.WriteError($"Введите целое число в диапазоне {min} - {max}");
            Console.WriteLine();
        }

        return res;
    }
    
    public static int GetNaturalInt()
    {
        int res;

        while (!int.TryParse(Console.ReadLine(), out res) || res <= 0)
        {
            Writers.WriteError($"Введите целое число больше нуля!");
            Console.WriteLine();
        }

        return res;
    }

    public static int GetIntFromArray(int[] values)
    {
        int res;

        while (!int.TryParse(Console.ReadLine(), out res) || Array.IndexOf(values, res) == -1)
        {
            Writers.WriteError($"Введите целое число из вот этих:");
            Console.WriteLine(string.Join(", ", values.Select(x => x.ToString()).ToArray()));
            Console.WriteLine();
        }

        return res;
    }
    
    public static int GetInt()
    {
        int res;

        while (!int.TryParse(Console.ReadLine(), out res))
        {
            Writers.WriteError($"Введите целое число:");
            Console.WriteLine();
        }

        return res;
    }

    public static (int int1, int int2) GetTwoIntsSepBySpace()
    {
        int int1;
        int int2;
        while (true)
        {
            try
            {
                string[] size_info = Console.ReadLine().Split(' ');
                int1 = int.Parse(size_info[0]);
                int2 = int.Parse(size_info[1]);
                break;
            }
            catch
            {
                Writers.WriteError("Какой-то мутный ввод, введи два числа через пробел");
                Console.WriteLine();
            }
        }

        return (int1, int2);
    }
    public static (int int1, int int2) GetTwoPosIntsSepBySpace()
    {
        int int1;
        int int2;
        while (true)
        {
            try
            {
                string[] size_info = Console.ReadLine().Split(' ');
                int1 = int.Parse(size_info[0]);
                int2 = int.Parse(size_info[1]);
                if (int1 <= 0 || int2 <= 0)
                    throw new Exception();
                break;
            }
            catch
            {
                Writers.WriteError("Какой-то мутный ввод, введи два положительных числа через пробел");
                Console.WriteLine();
            }
        }

        return (int1, int2);
    }
    
    public static int GetChoice(string[] options, String baseMessage)
    {
        Console.WriteLine(baseMessage);
        Console.WriteLine();
        for (int i = 0; i < options.Length; i++)
        {
            Writers.WriteOk($"{i + 1} {options[i]}");
        }
        Console.WriteLine();
        return UserInput.GetIntInRange(1, options.Length) - 1;
    }

    public static int[,] Get2DIntArray(int rows, int cols)
    {
        int[,] res = new int[rows, cols];
        while (true)
        {
            try
            {
                for (var row = 0; row < rows; row++)
                {
                    string[] newRow = Console.ReadLine().Split(' ');
                    for (var col = 0; col < cols; col++)
                    {
                        res[row, col] = int.Parse(newRow[col]);
                        if (res[row, col] <= 0)
                            throw new Exception();
                    }
                }
                break;
            }
            catch
            {
                Writers.WriteError($"Дожили, админ не может нормально данные ввести...\nВведите массив положительных ЧИСЕЛОК размера {rows}x{cols}!");
                Console.WriteLine();
            }
        }

        return res;
    }
    
    public static bool GetBool()
    {
        Console.Write("[y/n] ");
        String input = Console.ReadLine().ToLower();

        while (!(input.Equals("y") || input.Equals("n")))
        {
            Writers.WriteError($"Введите y или n");
            Console.WriteLine();
            Console.Write("[y/n] ");
            input = Console.ReadLine().ToLower();
        }

        return input.Equals("y");
    }

    public static DateTime GetDateTime()
    {
        string format = "dd.MM.yyyy HH:mm";
        DateTime userDateTime;
        while (true)
        {
            try
            {
                Console.WriteLine("Время в формате дд.мм.гггг чч:мм:");
                userDateTime = DateTime.ParseExact(Console.ReadLine(), format, null);
                break;
            }
            catch (Exception e)
            {
                Writers.WriteError("Введите дату и время в формате дд/мм/гггг чч:мм");
            }
        }
        return userDateTime;
    }

    public static TimeSpan GetTimeSpan()
    {
        string format = "HH:mm";
        TimeSpan userDateTime;
        while (true)
        {
            try
            {
                Console.WriteLine("Время в формате чч:мм:");
                userDateTime = TimeSpan.ParseExact(Console.ReadLine(), format,  null);
                break;
            }
            catch (Exception e)
            {
                Writers.WriteError("Введите время в формате чч:мм");
            }
        }
        return userDateTime;
    }
    
}