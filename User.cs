namespace learn;

public class User
{
    int id;
    string _fullName;
    private int _balance;
    List<int> _ticketsIds = new List<int>();

    public User(int balance, string fullName)
    {
        id = (new Random()).Next();
        _fullName = fullName;
        _balance = balance;
    }

    public void AddTicketId(int TicketId)
    {
        
    }

    public int GetId()
    {
        return id;
    }

    public int GetBalance()
    {
        return _balance;
    }

    public bool HasEnoughMoney(int sum)
    {
        return _balance >= sum;
    }

    public void insertBalance(int balance)
    {
        _balance += balance;
    }

    public void ChargeOff(int sum)
    {
        if (_balance < sum)
            throw new Exception("Not enough money");
        _balance -= sum;
    }

    public string GetFullName()
    {
        return _fullName;
    }
}