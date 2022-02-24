namespace learn;

public class IncorrectAgeRatingException : Exception{}

public class Film
{
    private int id;
    public static int[] CorrectAgeRatings = {0, 6, 12, 16, 18};

    String _filmName;
    int _ageRating;
    private int _filmDuration; // В минутах

    public Film(string filmName, int ageRating, int filmDuration)
    {
        id = (new Random()).Next();
        _filmName = filmName;
        _ageRating = ageRating;
        _filmDuration = filmDuration;
    }

    public int GetId()
    {
        return id;
    }

    public String GetName()
    {
        return _filmName;
    }

    public void SetName(string name)
    {
        _filmName = name;
    }

    public void SetAgeRating(int ageRating)
    {
        _ageRating = ageRating;
    }
    
    public int GetDuration()
    {
        return _filmDuration;
    }

    public void SetDuration(int duration)
    {
        _filmDuration = duration;
    }

    public int GetAgeRating()
    {
        return _ageRating;
    }
    public void Print()
    {
        Console.WriteLine($"Id: {id}\nНазвание: {_filmName}\nВозрастной рейтинг: {_ageRating}+ \nДлительность {_filmDuration / 60}:{_filmDuration % 60}");
    }
}

