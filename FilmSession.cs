namespace learn;

public class FilmSession
{
    private int id;
    private readonly int _filmId;
    private readonly int _hallId;
    private HallPricesBooking _hallPricesBooking;
    private readonly DateTime _dateTimeStart;
    private readonly DateTime _dateTimeEnd;

    public FilmSession(Film film, Hall hall, DateTime dateTime, HallPricesBooking hallPricesBooking)
    {
        id = (new Random()).Next();
        _filmId = film.GetId();
        _hallId = hall.GetId();
        _hallPricesBooking = hallPricesBooking;
        _dateTimeStart = dateTime;
        _dateTimeEnd = dateTime.AddMinutes(film.GetDuration());
    }

    public int GetId()
    {
        return id;
    }

    public HallPricesBooking GetHallPricesBooking()
    {
        return _hallPricesBooking;
    }
    public int GetFilmId()
    {
        return _filmId;
    }
    
    public int GetHallId()
    {
        return _hallId;
    }
    
    public DateTime GetStartTime()
    {
        return _dateTimeStart;
    }
    
    public DateTime GetEndTime()
    {
        return _dateTimeEnd;
    }
    
    public void Print()
    {
        Console.WriteLine($"filmId: {_filmId}");
        Console.WriteLine($"hallId: {_hallId}");
        Console.WriteLine($"startTime: {_dateTimeStart}");
        Console.WriteLine($"endTime: {_dateTimeEnd}");
        _hallPricesBooking.PrintSeatsPriceMap();
    }

}