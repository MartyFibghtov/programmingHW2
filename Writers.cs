namespace learn;

public class Writers
{
    public static void WriteOk(String str)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(str);
        Console.ResetColor();
    }

    public static void WriteError(String str)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(str);
        Console.ResetColor();
    }
}