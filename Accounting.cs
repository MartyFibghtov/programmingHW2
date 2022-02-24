namespace learn;

public class Ticket
{
    private int id;
    int _sum;
    int _userId;
    int _filmSessionId;
    (int, int) _seat;
    DateTime _datetime;

    public Ticket(int sum, int userId, int filmSessionId, (int, int) seat)
    {
        id = (new Random()).Next();
        _sum = sum; 
        _userId = userId; 
        _filmSessionId = filmSessionId;
        _seat = seat;
        _datetime = DateTime.Now;
    }

    public int GetId()
    {
        return id;
    }

    public int GetSum()
    {
        return _sum;
    }

    public DateTime GetDatetime()
    {
        return _datetime;
    }
    
    public int GetUserId()
    {
        return _userId;
    }

    public int GetFilmSessionId()
    {
        return _filmSessionId;
    }

    public (int, int) GetSeatCoord()
    {
        return _seat;
    }

}
public class Accounting
{
    List<Ticket> _tickets = new List<Ticket>();

    public void AddTicket(Ticket ticket)
    {
        _tickets.Add(ticket);
    }

    public List<Ticket> GetTickets()
    {
        return _tickets;
    }

    public static int FindIncome(List<Ticket> tickets)
    {
        return tickets.Sum(ticket => ticket.GetSum());
    }

    public List<Ticket> GetUserTickets(int userId)
    {
        return _tickets.Where(ticket => ticket.GetUserId() == userId).ToList();
    }
    
    
}
