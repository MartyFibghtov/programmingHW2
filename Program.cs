// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Security.AccessControl;


namespace learn
{

    class HallNameConflictException : Exception
    {
    }

    class FilmNameConflictException : Exception
    {
    }

    class HallDoesNotExistException : Exception
    {
    }

    class FilmDoesNotExistException : Exception
    {
    }

    class CinemaInfo
    {
        List<Hall> _halls;
        List<Film> _films;
        List<FilmSession> _filmSessions;

        public CinemaInfo()
        {
            _halls = new List<Hall>();
            _films = new List<Film>();
            _filmSessions = new List<FilmSession>();
        }

        public bool IsHallNameFree(String hallName)
        {
            if (_halls.Any(hall => hall.GetName().ToLower().Equals(hallName.ToLower())))
            {
                return false;
            }

            return true;
        }

        public bool IsFilmNameFree(String filmName)
        {
            if (_films.Any(hall => hall.GetName().ToLower().Equals(filmName.ToLower())))
            {
                return false;
            }

            return true;
        }

        public bool IsHallFreeOnDateTime(DateTime start, int duration, int hallId)
        {
            CheckHallId(hallId);
            var end = start.AddMinutes(duration);
            var hallSessions = new List<FilmSession>();
            hallSessions.AddRange(_filmSessions.Where(session => session.GetHallId() == hallId));
            return !(hallSessions.Any(fs =>
                (fs.GetStartTime().CompareTo(start) >= 0 && fs.GetStartTime().CompareTo(end) <= 0) ||
                (fs.GetStartTime().CompareTo(end) < 0 && fs.GetEndTime().CompareTo(start) >= 0)));
        }

        public void AddHall(Hall newHall)
        {
            if (!IsHallNameFree(newHall.GetName().ToLower()))
            {
                throw new HallNameConflictException();
            }

            _halls.Add(newHall);
        }

        public void DeleteFilm(int filmId)
        {
            var itemToRemove = _films.Single(r => r.GetId() == filmId);
            _films.Remove(itemToRemove);
        }

        public void DeleteHall(int hallId)
        {
            var itemToRemove = _halls.Single(r => r.GetId() == hallId);
            _halls.Remove(itemToRemove);
        }
        public void DeleteSession(int Id)
        {
            var itemToRemove = _filmSessions.Single(r => r.GetId() == Id);
            _filmSessions.Remove(itemToRemove);
        }

        public void AddFilm(Film newFilm)
        {
            if (!IsFilmNameFree(newFilm.GetName().ToLower()))
            {
                throw new FilmNameConflictException();
            }

            _films.Add(newFilm);
        }

        public List<Film> GetFilms()
        {
            return _films;
        }

        public List<Hall> GetHalls()
        {
            return _halls;
        }

        public Hall GetHallById(int hallId)
        {
            foreach (var hall in _halls.Where(hall => hall.GetId() == hallId))
            {
                return hall;
            }

            throw new HallDoesNotExistException();
        }
        
        public Film GetFilmById(int hallId)
        {
            foreach (var film in _films.Where(film => film.GetId() == hallId))
            {
                return film;
            }

            throw new FilmDoesNotExistException();
        }

        public FilmSession? GetFilmSessionById(int filmSessionId)
        {
            return _filmSessions.FirstOrDefault(filmSession => filmSession.GetId() == filmSessionId);
        }

        public bool HallExistById(int id)
        {
            return _halls.Any(hall => hall.GetId() == id);
        }

        public void CheckHallId(int id)
        {
            if (!HallExistById(id))
                throw new HallDoesNotExistException();
        }

        public void AddFilmSession(FilmSession filmSession)
        {
            _filmSessions.Add(filmSession);
        }

        public List<FilmSession> GetAvailableFilmSessions(int filmId)
        {
            DateTime currTime = DateTime.Now;

            return _filmSessions
                .Where(filmSession =>
                    filmSession.GetFilmId() == filmId && filmSession.GetStartTime().CompareTo(currTime) > 0)
                .OrderBy(filmSession => filmSession.GetStartTime()).ToList();
        }
        
        public List<int> GetFilmSessionsIdsByFilmId(int filmId)
        {

            return _filmSessions
                .Where(filmSession =>
                    filmSession.GetFilmId() == filmId)
                .OrderBy(filmSession => filmSession.GetStartTime()).ToList()
                .Select(x => x.GetId()).ToList();
        }
        
        public List<int> GetFilmSessionsIdsByHallId(int hallId)
        {

            return _filmSessions
                .Where(filmSession =>
                    filmSession.GetHallId() == hallId)
                .OrderBy(filmSession => filmSession.GetStartTime()).ToList()
                .Select(x => x.GetId()).ToList();
        }

        public List<int> GetFilmsByAgeRating(int ageRating)
        {
            return _films
                .Where(film => film.GetAgeRating() == ageRating)
                .Select(films => films.GetId()).ToList();
        }
        
        public List<int> GetFilmSessionsIdsByAgeRating(int ageRating)
        {
            var relevantFilms = GetFilmsByAgeRating(ageRating);
            return _filmSessions
                .Where(filmSession =>
                    relevantFilms.Contains(filmSession.GetFilmId()))
                .Select(x => x.GetId()).ToList();
        }

        public string GetFilmSessionNameById(int filmSessionId)
        {
            var filmSession = GetFilmSessionById(filmSessionId);
            var hall = GetHallById(filmSession.GetHallId());
            return $"Время:{filmSession.GetStartTime()}-{filmSession.GetEndTime()}\n\tЗал: {hall.GetName()}";
        }

        public void PrintAllInfo()
        {
            Writers.WriteOk("Films:\n");
            foreach (var film in _films)
            {
                film.Print();
                Console.WriteLine();
            }

            Console.WriteLine();
            Writers.WriteOk("Halls:\n");
            foreach (var hall in _halls)
            {
                hall.Print();
                Console.WriteLine();
            }

            Console.WriteLine();
            Writers.WriteOk("FilmSessions:\n");
            foreach (var filmSession in _filmSessions)
            {
                filmSession.Print();
                Console.WriteLine();
            }
        }
        
        public (int, int) GetHallsLoad()
        {
            DateTime currTime = DateTime.Now;
            (int, int) res = (0, 0);
            
            var filmSessions = _filmSessions
                .Where(filmSession => filmSession.GetStartTime().CompareTo(currTime) > 0);
            foreach (var filmSession in filmSessions)
            {
                var (item1, item2) = filmSession.GetHallPricesBooking().GetBookStat();
                res.Item1 += item1;
                res.Item2 += item2;
            }

            return res;
        }
    }
    
    class GoBackException : Exception {}

    class Programm
    {
        static bool getReadFromFile()
        {
            Console.WriteLine("Хотите прочитать данные из файла?");
            return UserInput.GetBool();
        }


        // Halls Initialization
        public static String GetAvailableHallName(CinemaInfo cinemaInfo)
        {
            String hallName = Console.ReadLine().Trim();
            while (!cinemaInfo.IsHallNameFree(hallName))
            {
                Writers.WriteError("Это название уже занято!");
                hallName = Console.ReadLine().Trim();
            }

            return hallName;
        }

        public static Hall InitHall(CinemaInfo cinemaInfo)
        {
            Hall hall;

            Console.WriteLine();

            Writers.WriteOk("Введите название зала:");
            String hallName = GetAvailableHallName(cinemaInfo);
            Console.WriteLine();

            Writers.WriteOk("Введите размер зала в формате x y");
            var hallSize = UserInput.GetTwoPosIntsSepBySpace();
            Console.WriteLine();

            Writers.WriteOk("Введите дефолтные цены для зала");
            var defaultPrices = UserInput.Get2DIntArray(hallSize.int1, hallSize.int2);
            Console.WriteLine();

            hall = new Hall(hallName, hallSize.int1, hallSize.int2, defaultPrices);
            return hall;
        }

        public static void InitHalls(CinemaInfo cinemaInfo)
        {
            Console.WriteLine("Сколько залов?");
            int hallsCount = UserInput.GetNaturalInt();

            for (int i = 0; i < hallsCount; i++)
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine($"Заполнение информации о зале {i}");
                        Console.WriteLine();
                        Hall hall = InitHall(cinemaInfo);
                        cinemaInfo.AddHall(hall);
                        break;
                    }
                    catch (HallNameConflictException e)
                    {
                        Writers.WriteError("Это название зала уже занято!");
                        Console.WriteLine("Давай по новой...");
                        Console.WriteLine();
                    }
                }
            }

            Writers.WriteOk("Сбор залов завершен! Переходим к сбору фильмов");
        }


        // Films Initialization

        public static String GetAvailableFilmName(CinemaInfo cinemaInfo)
        {
            String filmName = Console.ReadLine().Trim();
            while (!cinemaInfo.IsFilmNameFree(filmName))
            {
                Writers.WriteError("Это название фильма уже занято!");
                filmName = Console.ReadLine().Trim();
            }

            return filmName;
        }

        public static (int int1, int int2) GetFilmDuration()
        {
            var filmLengthHhMm = UserInput.GetTwoIntsSepBySpace();
            while (filmLengthHhMm.int1 < 0 || filmLengthHhMm.int2 < 0 ||
                   (filmLengthHhMm.int1 + filmLengthHhMm.int2 == 0))
            {
                Writers.WriteError("Ну фильм длины ноль так себе идея. Отрицательному времени соболезную...");
                Writers.WriteOk("Давай по новой");
                filmLengthHhMm = UserInput.GetTwoIntsSepBySpace();
            }

            return filmLengthHhMm;
        }

        public static Film InitFilm(CinemaInfo cinemaInfo)
        {
            Writers.WriteOk("Введите название фильма:");
            var filmName = GetAvailableFilmName(cinemaInfo);
            Console.WriteLine();

            Writers.WriteOk("Введите возрастной рейтинг");
            var ageRating = UserInput.GetIntFromArray(Film.CorrectAgeRatings);
            Console.WriteLine();

            Writers.WriteOk("Введите продолжительность фильма в формате чч мм");
            var filmLengthHhMm = GetFilmDuration();
            Console.WriteLine();
            var filmLengthM = (filmLengthHhMm.int1 * 60 + filmLengthHhMm.int2);

            Film film = new Film(filmName, ageRating, filmLengthM);

            return film;
        }

        public static void InitFilms(CinemaInfo cinemaInfo)
        {
            Console.WriteLine("Сколько фильмов?");
            var filmsCount = UserInput.GetNaturalInt();

            for (int i = 0; i < filmsCount; i++)
            {
                while (true)
                {
                    try
                    {
                        Film film = InitFilm(cinemaInfo);
                        cinemaInfo.AddFilm(film);
                        Writers.WriteOk("Фильм сохранен!");
                        film.Print();
                        Console.WriteLine();
                        break;
                    }
                    catch (HallNameConflictException e)
                    {
                        Writers.WriteError("Это название фильма уже занято!");
                        Console.WriteLine("Давай по новой...");
                        Console.WriteLine();
                    }
                }
            }

            Writers.WriteOk("Сбор фильмов завершен! Переходим к сбору сеансов");
        }


        // Film Sessions Initialization

        public static DateTime GetDateTimeForSession(CinemaInfo cinemaInfo, Film film, int hallId)
        {
            int filmDuration = film.GetDuration();

            Console.WriteLine("Введите время начала сеанса");
            DateTime datetime = UserInput.GetDateTime();
            Console.WriteLine();

            while (!cinemaInfo.IsHallFreeOnDateTime(datetime, filmDuration, hallId))
            {
                Writers.WriteError("Это время уже занято!");
                datetime = UserInput.GetDateTime();
                Console.WriteLine();
            }

            return datetime;
        }

        public static FilmSession InitFilmSession(CinemaInfo cinemaInfo, Film film)
        {
            var hallId = GetHallIdChoice(cinemaInfo);
            var hall = cinemaInfo.GetHallById(hallId);
            var datetime = GetDateTimeForSession(cinemaInfo, film, hallId);

            Writers.WriteOk("Ввести новые цены?");
            Console.WriteLine("Иначе будут установлены дефолтные цены");
            var enterNewPrices = UserInput.GetBool();
            Console.WriteLine();

            HallPricesBooking hallPricesBooking;
            if (enterNewPrices)
            {
                var hallSize = hall.GetHallSize();
                var prices = UserInput.Get2DIntArray(hallSize.rows, hallSize.cols);
                hallPricesBooking = hall.NewHallPricesBooking(prices);
            }
            else
                hallPricesBooking = hall.NewHallPricesBooking();

            FilmSession filmSession = new FilmSession(film, hall, datetime, hallPricesBooking);

            return filmSession;
        }

        public static void InitFilmSessions(CinemaInfo cinemaInfo)
        {
            foreach (var film in cinemaInfo.GetFilms())
            {
                Console.WriteLine();
                Console.WriteLine($"Хотите внести информацию о сеансах для фильма {film.GetName()}?");
                var addSession = UserInput.GetBool();
                while (addSession)
                {
                    Writers.WriteOk("Введите информацию о сеансе");
                    Console.WriteLine();
                    var filmSession = InitFilmSession(cinemaInfo, film);
                    cinemaInfo.AddFilmSession(filmSession);
                    Writers.WriteOk("Сеанс сохранен");
                    filmSession.Print();
                    Console.WriteLine("Хотите внести информацию о еще одном сеансе?");
                    addSession = UserInput.GetBool();
                }
            }

            Console.WriteLine();
            Writers.WriteOk("Сбор информации о сенсах завершен!");
            Writers.WriteOk("Переходим к Авторизации!");
        }

        static void InitFromUserInput(CinemaInfo cinemaInfo)
        {
            InitHalls(cinemaInfo);
            InitFilms(cinemaInfo);
            InitFilmSessions(cinemaInfo);
        }

        // User Initialization 

        static User InitUser()
        {
            int balance;
            User user;

            Writers.WriteOk("Введите ваш баланс. Только честно плиз");
            balance = UserInput.GetInt();
            Console.WriteLine();

            Writers.WriteOk("Введите ФИО");
            string fullname = Console.ReadLine();
            user = new User(balance, fullname);

            return user;
        }


        // Различные выбиралки 

        static (int row, int col) GetSeatCoordChoice(HallPricesBooking hallPricesBooking)
        {
            Writers.WriteOk("Введите номер ряда и места через пробел");

            var seatCoord = UserInput.GetTwoPosIntsSepBySpace();
            while (!hallPricesBooking.SeatExists(seatCoord.int1 - 1, seatCoord.int2 - 1))
            {
                Writers.WriteError("Такого места не существует");
                seatCoord = UserInput.GetTwoPosIntsSepBySpace();
            }

            return (seatCoord.int1 - 1, seatCoord.int2 - 1);
        }
        
        static (int row, int col) GetFreeSeatCoordChoice(HallPricesBooking hallPricesBooking)
        {
            if (hallPricesBooking.AllAreBooked())
            {
                Writers.WriteError("Все места заняты");
                throw new GoBackException();
            }
            Writers.WriteOk("Введите номер ряда и места через пробел");

            var seatCoord = UserInput.GetTwoPosIntsSepBySpace();
            if (!hallPricesBooking.SeatExists(seatCoord.int1 - 1, seatCoord.int2 - 1) || 
                   !hallPricesBooking.seatIsAvailable(seatCoord.int1 - 1, seatCoord.int2 - 1))
            {
                // TODO Разный вывод в зависимости от проблемы
                Writers.WriteError("Такого места не существует или оно занято");
                Console.WriteLine();
                throw new GoBackException();
            }

            return (seatCoord.int1 - 1, seatCoord.int2 - 1);
        }

        public static int GetFilmIdChoice(CinemaInfo cinemaInfo)
        {
            var films = cinemaInfo.GetFilms();
            if (films.Count == 0)
            {
                Writers.WriteError("Нет ни одного фильма");
                return 0;
            }
            var filmsNames = films.Select(x => x.GetName()).ToArray();
            var filmChoice = UserInput.GetChoice(filmsNames, "Выберите фильм");
            var chosenFilm = films[filmChoice];
            Writers.WriteOk($"Выбран фильм {chosenFilm.GetName()}");

            return chosenFilm.GetId();
        }

        public static int GetHallIdChoice(CinemaInfo cinemaInfo)
        {
            var halls = cinemaInfo.GetHalls();
            var hallsNames = halls.Select(x => x.GetName()).ToArray();
            var hallChoice = UserInput.GetChoice(hallsNames, "Выберите зал");
            var chosenHall = halls[hallChoice];
            Writers.WriteOk($"Выбран зал {chosenHall.GetName()}");

            return chosenHall.GetId();
        }

        public static int GetSessionIdChoice(CinemaInfo cinemaInfo, int filmId)
        {
            var availableSessions = cinemaInfo.GetAvailableFilmSessions(filmId);
            if (availableSessions.Count == 0)
            {
                Writers.WriteError($"Нет доступных сеансов");
                return 0;
            }
                
            var sessionsNames = availableSessions.Select(x => cinemaInfo.GetFilmSessionNameById(x.GetId())).ToArray();
            Console.WriteLine(sessionsNames.ToString());
            var sessionChoice = UserInput.GetChoice(sessionsNames, "Выберите сеанс");
            var chosenSession = availableSessions[sessionChoice];
            Writers.WriteOk($"Выбран сеанс {sessionsNames[sessionChoice]}");

            return chosenSession.GetId();
        }


        // Покупка билета 

        public static void BuyTicket(CinemaInfo cinemaInfo, User user, Accounting accounting)
        {
            try
            {
                var filmId = GetFilmIdChoice(cinemaInfo);
                if (filmId == 0) return;

                var filmSessionId = GetSessionIdChoice(cinemaInfo, filmId);
                if (filmSessionId == 0) return;

                var filmSession = cinemaInfo.GetFilmSessionById(filmSessionId);
                var hallPricesBooking = filmSession.GetHallPricesBooking();
                if (hallPricesBooking.AllAreBooked())
                {
                    Writers.WriteError("Все билеты на этот сеанс распроданы");
                    Console.WriteLine();
                    return;
                }
                hallPricesBooking.PrintSeatsPriceMap();

                var seatCoord = GetFreeSeatCoordChoice(hallPricesBooking);
                var seatPrice = hallPricesBooking.GetSeatPrice(seatCoord.Item1, seatCoord.Item2);

                Writers.WriteOk($"Билет стоит {seatPrice}, хотите его купить?");
                if (!UserInput.GetBool())
                {
                    Writers.WriteOk("Как скажете :)");
                    return;
                }

                if (!user.HasEnoughMoney(seatPrice))
                {
                    Writers.WriteError("Недостаточно средств :(");
                    return;
                }

                Ticket ticket = new Ticket(seatPrice, user.GetId(), filmSessionId, seatCoord);

                hallPricesBooking.BookSeat(seatCoord.Item1, seatCoord.Item2);
                user.ChargeOff(seatPrice);
                user.AddTicketId(ticket.GetId());
                accounting.AddTicket(ticket);

                Writers.WriteOk("Билет успешно приобретен!");
                Console.WriteLine();
            }
            catch (GoBackException e)
            {
                Writers.WriteOk("Возврат в главное меню");
                
            }
        }
        
        public static void BuyTicketsMenu(CinemaInfo cinemaInfo, User user, Accounting accounting)
        {
            int Buy = 0;
            int Exit = 1;
            string[] options = {"Купить билет", "В главное меню"};
            string baseMessage = "Что вы хотите сделать?";
            
            int userAction = UserInput.GetChoice(options, baseMessage);
            while (userAction != Exit)
            {
                BuyTicket(cinemaInfo, user, accounting);
                userAction = UserInput.GetChoice(options, baseMessage);
            }

        }

        
        // Личный кабинет

        public static void ShowTickets(User user, Accounting accounting, CinemaInfo cinemaInfo)
        {
            var tickets = accounting.GetUserTickets(user.GetId());
            tickets = tickets.OrderBy(x => cinemaInfo.GetFilmSessionById(x.GetId()).GetStartTime()).ToList();
            
            foreach (var ticket in tickets)
            {
                var filmSession = cinemaInfo.GetFilmSessionById(ticket.GetId());
                Console.WriteLine(cinemaInfo.GetFilmById(filmSession.GetFilmId()).GetName());
                Console.WriteLine(cinemaInfo.GetFilmSessionNameById(filmSession.GetId()));
                Console.WriteLine($"Ряд: {ticket.GetSeatCoord().Item1} Место{ticket.GetSeatCoord().Item2}");
                Console.WriteLine();
            }
        }

        public static void AddMoneyMenu(User user)
        {
            Console.WriteLine($"Ваш текущий баланс: {user.GetBalance()}");
            Console.WriteLine();
            
            Writers.WriteOk("Введите сумму пополнения");
            var sum = UserInput.GetInt();
            user.insertBalance(sum);
            
            Console.WriteLine($"Ваш новый баланс: {user.GetBalance()}");
        }

        public static void ChangeUserMenu(User user)
        {
            Console.WriteLine("Пока не робит");
        }
        
        public static void AccountMenu(User user, Accounting accounting, CinemaInfo cinemaInfo)
        {
            int SeeTickets = 0;
            int AddMoney = 1;
            int LogOut = 2;
            int Exit = 3;
            
            var options = new[] {"Мои билеты", "Пополнить баланс", "Сменить юзера", "Выход"};
            var baseMessage = "Что вы хотите сделать?";
            
            int UserAction = UserInput.GetChoice(options, baseMessage);
            while (UserAction != Exit)
            {
                if (UserAction == SeeTickets)
                    ShowTickets(user, accounting, cinemaInfo);
                else if (UserAction == AddMoney)
                    AddMoneyMenu(user);
                else if (UserAction == LogOut)
                    ChangeUserMenu(user);
                UserAction = UserInput.GetChoice(options, baseMessage);
            }
            Console.WriteLine("Выход в главное меню");
        }


        // Админка 

        static bool checkAdminPermission()
        {
            String password = "aboba";
            bool hasAdminPermission = false;
        
            Console.WriteLine("Введите пароль:");
            String userPassword = Console.ReadLine();
        
            if (userPassword.Equals(password))
            {
                Writers.WriteOk("Добро пожаловать, Одмен!\n");
                hasAdminPermission = true;
            }
            else
            {
                Writers.WriteError("Пароль неверный, мамкин хакер!\n");
            }
        
            return hasAdminPermission;
        }
        
        
        // Аналитика 


        public static void ShowHallsLoad(CinemaInfo cinemaInfo)
        {
            var load = cinemaInfo.GetHallsLoad();
            Writers.WriteOk($"Загруженность залов ({load.Item2}/{load.Item1 + load.Item2})");
            Console.WriteLine();
        }

        public static List<Ticket> FilterByDate(List<Ticket> tickets, DateTime startTime, DateTime endTime)
        {
            return tickets.Where(ticket => ticket.GetDatetime().CompareTo(startTime) >= 0 && ticket.GetDatetime().CompareTo(endTime) <= 0).ToList();
        }

        public static List<Ticket> FilterByDayTime(List<Ticket> tickets, TimeSpan startTime, TimeSpan endTime)
        {
            return tickets.Where(ticket => TimeSpan.Compare(ticket.GetDatetime().TimeOfDay, startTime)  >= 0 && 
                                           TimeSpan.Compare(ticket.GetDatetime().TimeOfDay, endTime)  <= 0).ToList();
        }

        
        public static List<Ticket> FilterBySession(List<Ticket> tickets, int sessionId)
        {
            return tickets.Where(ticket => ticket.GetFilmSessionId() == sessionId).ToList();
        }
        
        public static List<Ticket> FilterByFilmId(List<Ticket> tickets, int filmId, CinemaInfo cinemaInfo)
        {
            var relevantFilmSessions = cinemaInfo.GetFilmSessionsIdsByFilmId(filmId);
            return tickets.Where(ticket => relevantFilmSessions.Contains(ticket.GetFilmSessionId())).ToList();
        }
        
        public static List<Ticket> FilterByHallId(List<Ticket> tickets, int hallId, CinemaInfo cinemaInfo)
        {
            var relevantFilmSessions = cinemaInfo.GetFilmSessionsIdsByHallId(hallId);
            return tickets.Where(ticket => relevantFilmSessions.Contains(ticket.GetFilmSessionId())).ToList();
        }
        
        public static List<Ticket> FilterByAgeRating(List<Ticket> tickets, int ageRating, CinemaInfo cinemaInfo)
        {
            
            var relevantFilmSessions = cinemaInfo.GetFilmSessionsIdsByAgeRating(ageRating);
            return tickets.Where(ticket => relevantFilmSessions.Contains(ticket.GetFilmSessionId())).ToList();
        }
        
        public static void TotalIncomeMenu(Accounting accounting, CinemaInfo cinemaInfo)
        {
            var options = new[] {"Конкретный фильм", "Конкретный зал", "Промежуток времени", "Время дня", "Конкретный возрастной рейтинг", "Выход"};
            var baseMessage = "Что вы хотите сделать?";

            int byFilm = 0;
            int byHall = 1;
            int byDateTime = 2;
            int byTime = 3;
            int byAgeRating = 4;
            int Exit = 5;

            List<Ticket> tickets = accounting.GetTickets();
            Console.WriteLine($"Суммарный доход без фильтров: {Accounting.FindIncome(tickets)}");
            int userAction = UserInput.GetChoice(options, baseMessage);
            while (userAction != Exit)
            {
                if (userAction == byFilm)
                {
                    int filmId = GetFilmIdChoice((cinemaInfo));
                    if (filmId == 0)
                    {
                        Console.WriteLine("Фильтр отменен");
                    }
                    tickets = FilterByFilmId(tickets, filmId, cinemaInfo);
                }
                    
                else if (userAction == byHall)
                {
                    int hallId = GetHallIdChoice(cinemaInfo);
                    tickets = FilterByHallId(tickets, hallId, cinemaInfo);
                }
                
                else if (userAction == byDateTime)
                {
                    Console.WriteLine("Введите дату и время начала периода");
                    var startTime = UserInput.GetDateTime();
                    Console.WriteLine("Введите дату и время конца периода");
                    var endTime = UserInput.GetDateTime();
                    tickets = FilterByDate(tickets, startTime, endTime);
                }
                
                else if (userAction == byTime)
                {
                    Console.WriteLine("Введите время начала периода");
                    var startTime = UserInput.GetTimeSpan();
                    Console.WriteLine("Введите время конца периода");
                    var endTime = UserInput.GetTimeSpan();
                    tickets = FilterByDayTime(tickets, startTime, endTime);
                }
                else if (userAction == byAgeRating)
                {
                    Writers.WriteOk("Введите возрастной рейтинг");
                    var ageRating = UserInput.GetIntFromArray(Film.CorrectAgeRatings);
                    Console.WriteLine();

                    tickets = FilterByAgeRating(tickets, ageRating, cinemaInfo);
                }

                Console.WriteLine($"С учетом всех фильтров доход: {Accounting.FindIncome(tickets)}");
                
                userAction = UserInput.GetChoice(options, baseMessage);
            }

        }
        
        public static void AnalyticsMenu(CinemaInfo cinemaInfo, Accounting accounting)
        {
            int hallsLoad = 0;
            int totalIncome = 1;
            int Exit = 2;
            
            var options = new[] {"Загруженость залов", "Рассчет доходов", "Выход"};
            var baseMessage = "Что вы хотите сделать?";
            
            int userAction = UserInput.GetChoice(options, baseMessage);
            while (userAction != Exit)
            {
                if (userAction == hallsLoad)
                    ShowHallsLoad(cinemaInfo);
                else if (userAction == totalIncome)
                    TotalIncomeMenu(accounting, cinemaInfo);
                userAction = UserInput.GetChoice(options, baseMessage);
            }
            Console.WriteLine("Выход в главное меню");
            
        }
        
        // Внесение изменений 

        public static void EditHalls(CinemaInfo cinemaInfo)
        {
            int deleteHall = 0;
            int changeName = 1;
            int createHall = 2;
            int Exit = 3;

            var options = new[] {"Удалить зал", "Измененить название зала", "Создать зал", "Выход"};
            var baseMessage = "Что вы хотите сделать?";

            int userAction = UserInput.GetChoice(options, baseMessage);
            while (userAction != Exit)
            {
                if (userAction == deleteHall)
                {
                    var hallId = GetHallIdChoice(cinemaInfo);
                    if (cinemaInfo.GetFilmSessionsIdsByHallId(hallId).Count() > 0)
                    {
                        Writers.WriteError("С этим залом связан сеанс, отказано");
                        return;
                    }
                    cinemaInfo.DeleteHall(hallId);
                }   
                else if (userAction == changeName)
                {
                    var hallId = GetHallIdChoice(cinemaInfo);
                    var hall = cinemaInfo.GetHallById(hallId);
                    Console.WriteLine("Введите новое имя зала");
                    var name = Console.ReadLine();
                    hall.SetName(name);
                    Writers.WriteOk("готово");
                }
                else if (userAction == createHall)
                {
                    var hall = InitHall(cinemaInfo);
                    cinemaInfo.AddHall(hall);
                    Writers.WriteOk("готово");
                }
                userAction = UserInput.GetChoice(options, baseMessage);
            }
        }
        
        public static void EditFilms(CinemaInfo cinemaInfo)
        {
            int deleteFilm = 0;
            int changeName = 1;
            int changeDuration = 2;
            int changeAgeRating = 3;
            int createFilm = 4;
            int Exit = 5;

            var options = new[] {"Удалить фильм", "Измененить название фильма", "Изменить длину фильма", "Изменить возр. рейтинг фильма", "Создать фильм", "Выход"};
            var baseMessage = "Что вы хотите сделать?";

            int userAction = UserInput.GetChoice(options, baseMessage);
            while (userAction != Exit)
            {
                if (userAction == deleteFilm)
                {
                    var filmId = GetFilmIdChoice(cinemaInfo);
                    if (cinemaInfo.GetFilmSessionsIdsByFilmId(filmId).Count() > 0)
                    {
                        Writers.WriteError("С этим фильмом связан сеанс, отказано");
                        return;
                    }
                    cinemaInfo.DeleteFilm(filmId);
                }   
                else if (userAction == changeName)
                {
                    var filmId = GetFilmIdChoice(cinemaInfo);
                    var film = cinemaInfo.GetFilmById(filmId);
                    Console.WriteLine("Введите новое имя фильма");
                    var name = Console.ReadLine();
                    film.SetName(name);
                    Writers.WriteOk("готово");
                }
                else if (userAction == changeDuration)
                {
                    var filmId = GetFilmIdChoice(cinemaInfo);
                    var film = cinemaInfo.GetFilmById(filmId);
                    if (cinemaInfo.GetFilmSessionsIdsByFilmId(filmId).Count() > 0)
                    {
                        Writers.WriteError("С этим фильмом связан сеанс, отказано");
                        return;
                    }
                    var filmLengthHhMm = GetFilmDuration();
                    var filmLengthM = (filmLengthHhMm.int1 * 60 + filmLengthHhMm.int2);
                    film.SetDuration(filmLengthM);
                    Writers.WriteOk("готово");
                }
                else if (userAction == changeAgeRating)
                {
                    var filmId = GetFilmIdChoice(cinemaInfo);
                    var ageRating = UserInput.GetIntFromArray(Film.CorrectAgeRatings);
                    var film = cinemaInfo.GetFilmById(filmId);
                    film.SetAgeRating(ageRating);
                    Writers.WriteOk("готово");
                }
                else if (userAction == createFilm)
                {
                    var film = InitFilm(cinemaInfo);
                    cinemaInfo.AddFilm(film);
                    Writers.WriteOk("готово");
                }
                userAction = UserInput.GetChoice(options, baseMessage);
            }
        }

        public static void EditSession(CinemaInfo cinemaInfo)
        {
            int deleteSession = 0;
            int changePrice = 1;
            int createSession = 2;
            int Exit = 3;

            var options = new[] {"Удалить сеанс", "Измененить цену места", "Создать сеанс", "Выход"};
            var baseMessage = "Что вы хотите сделать?";

            int userAction = UserInput.GetChoice(options, baseMessage);
            while (userAction != Exit)
            {
                if (userAction == deleteSession)
                {
                    Console.WriteLine("Для какого фильма сессия?");
                    var filmId = GetFilmIdChoice(cinemaInfo);
                    var filmSessionId = GetSessionIdChoice(cinemaInfo, filmId);
                    var session = cinemaInfo.GetFilmSessionById(filmSessionId);
                    if (session.GetHallPricesBooking().GetBookStat().Item2 != 0)
                    {
                        Writers.WriteError("Уже есть бронь, нельзя редакетировать");
                        return;
                    }
                    cinemaInfo.DeleteSession(filmSessionId);
                    Writers.WriteOk("Готово");
                }   
                else if (userAction == changePrice)
                {
                    Console.WriteLine("Для какого фильма сессия?");
                    var filmId = GetFilmIdChoice(cinemaInfo);
                    var filmSessionId = GetSessionIdChoice(cinemaInfo, filmId);
                    var session = cinemaInfo.GetFilmSessionById(filmSessionId);
                    var h = session.GetHallPricesBooking();
                    h.PrintSeatsPriceMap();
                    var coord = GetSeatCoordChoice(h);
                    Console.WriteLine("Введите новую цену");
                    var price = UserInput.GetNaturalInt();
                    h.SetSeatPrice(price, coord.row, coord.col);
                    Writers.WriteOk("Готово");
                }
                else if (userAction == createSession)
                {
                    Console.WriteLine("Для какого фильма создаем?");
                    var filmId = GetFilmIdChoice(cinemaInfo);
                    var film = cinemaInfo.GetFilmById(filmId);
                    var sess = InitFilmSession(cinemaInfo, film);
                    cinemaInfo.AddFilmSession(sess);
                    Writers.WriteOk("Готово");
                }
                
                userAction = UserInput.GetChoice(options, baseMessage);
            }
        }
        public static void EditingMenu(CinemaInfo cinemaInfo)
        {
            int hallEdit = 0;
            int filmEdit = 1;
            int sessionEdit = 2;
            int Exit = 3;
            
            var options = new[] {"Изменение залов", "Изменение фильмов", "Изменение сеансов", "Выход"};
            var baseMessage = "Что вы хотите сделать?";
            
            int userAction = UserInput.GetChoice(options, baseMessage);
            while (userAction != Exit)
            {
                if (userAction == hallEdit)
                    EditHalls(cinemaInfo);
                else if (userAction == filmEdit)
                    EditFilms(cinemaInfo);
                else if (userAction == sessionEdit)
                    EditSession(cinemaInfo);
                
                userAction = UserInput.GetChoice(options, baseMessage);
            }
            Console.WriteLine("Выход в главное меню");

        }
        
        public static void AdminMenu(Accounting accounting, CinemaInfo cinemaInfo)
        {

            int Analitics = 0;
            int Changes = 1;
            int Exit = 2;
            
            var options = new[] {"Аналитика", "Редактирование", "Выход"};
            var baseMessage = "Что вы хотите сделать?";
            

            if (!checkAdminPermission())
                return;
            
            int userAction = UserInput.GetChoice(options, baseMessage);
            while (userAction != Exit)
            {
                if (userAction == Analitics)
                    AnalyticsMenu(cinemaInfo, accounting);
                else if (userAction == Changes)
                    EditingMenu(cinemaInfo);
                userAction = UserInput.GetChoice(options, baseMessage);
            }
            Console.WriteLine("Выход в главное меню");
            
            
        }
        
        public static void MainMenu(CinemaInfo cinemaInfo, User user, Accounting accounting)
        {
            int BuyTicket = 0;
            int GoAdmin = 1;
            int Account = 2;
            int Exit = 3;

            var options = new[] {"Купить билет", "Войти в админку", "Войти в личный кабинет", "Выход"};
            var baseMessage = "Что вы хотите сделать?";
            
            int UserAction = UserInput.GetChoice(options, baseMessage);
            while (UserAction != Exit)
            {
                if (UserAction == BuyTicket)
                    BuyTicketsMenu(cinemaInfo, user, accounting);
                else if (UserAction == GoAdmin)
                    AdminMenu(accounting, cinemaInfo);
                else if (UserAction == Account)
                    AccountMenu(user, accounting, cinemaInfo);
                UserAction = UserInput.GetChoice(options, baseMessage);
            }

            Console.WriteLine("Выход из главного меню");
            
        }

        static void Main(string[] args)
        {
            // Сначала вносим информацию о всех залах
            // Вносим инфу о фильмах + сеансах на эти фильмы
            // Ну дальше понятно


            CinemaInfo cinemaInfo = new CinemaInfo();
            Accounting accounting = new Accounting();
            User user;

            // bool readFromFile = getReadFromFile();
            //
            // if (readFromFile)
            // {
            //     // InitFromFile(cinemaInfo); 
            // }
            // else
            // {
            InitFromUserInput(cinemaInfo);
            // }

            cinemaInfo.PrintAllInfo();

            user = InitUser();
            
            MainMenu(cinemaInfo, user, accounting);
            
            Console.WriteLine("До связи!");
        }
    }
}

    


