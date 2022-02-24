namespace learn
{
    public class SeatIsBookedException : Exception
    {
        
    }

    public class SeatDoesNotExistException : Exception
    {
        
    }
    
    public class Seat
    {
        private int _price;
        private int _soldPrice;
        private bool _isBooked = false;
        
        public void Book()
        {
            if (_isBooked)
                throw new SeatIsBookedException();
            _isBooked = true;
            SaveSoldPrice();
        }
        public void SetPrice(int newPrice)
        {
            _price = newPrice;
        }

        public int GetPrice()
        {
            return _price;
        }

        public bool IsBooked()
        {
            return _isBooked;
        }

        private void SaveSoldPrice()
        {
            _soldPrice = _price;
        }
    }
    
    public class Hall
    {
        int Id;
        String name = "Default hall name";
        (int rows, int columns) hallSize = (rows: 0, columns: 0);
        int[,] defaultPrices;
        
        public Hall(String hallName, int rows, int columns, int[,] prices)
        {
            Id = (new Random()).Next();
            name = hallName;
            hallSize.rows = rows;
            hallSize.columns = columns;
            defaultPrices = prices;
        }

        public (int rows, int cols) GetHallSize()
        {
            return hallSize;
        }

        public HallPricesBooking NewHallPricesBooking()
        {
            return new HallPricesBooking(GenNewSeatMap(defaultPrices));
        }

        public HallPricesBooking NewHallPricesBooking(int[,] prices)
        {
            return new HallPricesBooking(GenNewSeatMap(prices));
        }
        
        public int GetId()
        {
            return Id;
        }

        public String GetName()
        {
            return name;
        }

        public void SetName(string nname)
        {
            name = nname;
        }
        Seat[,] GenNewSeatMap(int[,] prices)
        {
            Seat[,] seatsMap;

            seatsMap = new Seat[hallSize.rows, hallSize.columns];
            for (var row = 0; row < hallSize.rows; row++)
            {
                for (var column = 0; column < hallSize.columns; column++)
                {
                    seatsMap[row, column] = new Seat();
                    seatsMap[row, column].SetPrice(prices[row, column]);
                }
            }
            
            return seatsMap;
        }

        public void PrintSeatsPriceMap()
        {
            int longestPrice = FindLongestPrice();
            
            Console.Write("  ");
            for (var column = 0; column < defaultPrices.GetLength(1); column++)
                Console.Write((column + 1).ToString().PadLeft(longestPrice) + " ");
            Console.WriteLine();
            for (var row = 0; row < defaultPrices.GetLength(0); row++)
            {
                Console.Write((row + 1) + " ");
                for (var column = 0; column < defaultPrices.GetLength(1); column++)
                {
                    Console.Write(defaultPrices[row, column].ToString().PadLeft(longestPrice));

                    if (column != defaultPrices.GetLength(1))
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        private int FindLongestPrice()
        {
            int longest = 0;
            int length;
        
            foreach (var price in defaultPrices)
            {
                length = price.ToString().Length;
                if (length > longest)
                    longest = length;
            }
            return longest;
        }
        
        public void Print()
        {
            Console.WriteLine($"id: {Id}");
            Console.WriteLine($"Название: {name}");
            Console.WriteLine("Дефолтные цены");
            
            PrintSeatsPriceMap();
            Console.WriteLine();
        }
        
    }

    public class HallPricesBooking
    {
        Seat[,] _seatsMap;
        
        public HallPricesBooking(Seat[,] seatsMap)
        {
            _seatsMap = seatsMap;
        }

        public void SetSeatPrice(int price, int row, int column)
        {
            if (! SeatExists(row, column))
                throw new Exception("Seat does not exist");
            _seatsMap[row, column].SetPrice(price);
        }
        
        public int GetSeatPrice(int row, int column)
        {
            if (! SeatExists(row, column))
                throw new SeatDoesNotExistException();
            return _seatsMap[row, column].GetPrice();
        }
        
        public void BookSeat(int row, int column)
        {
            if (!SeatExists(row, column))
                throw new SeatDoesNotExistException();
            _seatsMap[row, column].Book();
        }
        public bool SeatExists(int row, int column)
        {
            if (row >= _seatsMap.GetLength(0) || column >= _seatsMap.GetLength(1))
                return false;
            if (row < 0 || column < 0)
                return false;
            return true;
        }
        
        public bool seatIsAvailable(int row, int column)
        {
            if (SeatExists(row, column) && ! _seatsMap[row, column].IsBooked())
                return true;
            return false;
        }

        public void print_seats_map()
        {
            Console.Write("  ");
            for (var column = 0; column < _seatsMap.GetLength(1); column++)
                Console.Write((column + 1) + " ");
            Console.WriteLine();
            for (var row = 0; row < _seatsMap.GetLength(0); row++)
            {
                Console.Write((row + 1) + " ");
                for (var column = 0; column < _seatsMap.GetLength(1); column++)
                {
                    if (_seatsMap[row, column].IsBooked())
                        Console.Write('x');
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('o');
                        Console.ResetColor();
                    }

                    if (column != _seatsMap.GetLength(1))
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        private int FindLongestPrice()
        {
            int longest = 0;
            int length;
            
            foreach (var seat in _seatsMap)
            {
                length = seat.GetPrice().ToString().Length;
                if (length > longest)
                    longest = length;
            }
            
            return longest;
        }
        public void PrintSeatsPriceMap()
        {
            Console.WriteLine("\n\n====================");
            Console.WriteLine("Зеленое - Место свободно, \nкрасное - место выкуплено");

            int longestPrice = FindLongestPrice();
            
            Console.Write("  ");
            for (var column = 0; column < _seatsMap.GetLength(1); column++)
                Console.Write((column + 1).ToString().PadLeft(longestPrice) + " ");
            Console.WriteLine();
            for (var row = 0; row < _seatsMap.GetLength(0); row++)
            {
                Console.Write((row + 1) + " ");
                for (var column = 0; column < _seatsMap.GetLength(1); column++)
                {
                    if (! _seatsMap[row, column].IsBooked())
                        Console.ForegroundColor = ConsoleColor.Green;
                    else 
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(_seatsMap[row, column].GetPrice().ToString().PadLeft(longestPrice));
                    
                    if (column != _seatsMap.GetLength(1))
                        Console.Write(' ');
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        public (int, int) GetBookStat()
        {
            int booked = 0;
            int notBooked = 0;

            foreach (var seat in _seatsMap)
            {
                if (seat.IsBooked())
                    booked++;
                else
                    notBooked++;
            }
            
            return (notBooked, booked);
        }

        public bool AllAreBooked()
        {
            var stat = GetBookStat();
            return (stat.Item1 == 0);

        }

    }
}

