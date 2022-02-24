// namespace learn;
//
//
// public abstract class Menu
// {
//     public static void Run()
//     {
//         
//     }
//     
// }
//
//
//
// public class MainMenu : Menu
// {
//     public static int BuyTicket = 0;
//     public static int GoAdmin = 1;
//     public static int Account = 2;
//     public static int Exit = 3;
//
//     private static String[] options = new[] {"Купить билет", "Войти в админку", "Войти в личный кабинет", "Выход"};
//     private static String baseMessage = "Что вы хотите сделать?";
//
//     public static void Run()
//     {
//         int UserAction = UserInput.GetChoice(options, baseMessage);
//         while (UserAction != Exit)
//         {
//             if (UserAction == BuyTicket)
//                 BuyMenu.Run();
//             else if (UserAction == GoAdmin)
//                 AdminMenu.Run();
//             else if (UserAction == Account)
//                 AccountMenu.Run();
//             UserAction = UserInput.GetChoice(options, baseMessage);
//         }
//
//         Console.WriteLine("Выход из главного меню");
//     }
// }
//
// public class BuyMenu : Menu
// {
//     public static int BuyTicket = 0;
//     public static int Exit = 1;
//     private static string[] options = {"Купить билет", "В главное меню"};
//     private static string baseMessage = "Что вы хотите сделать?";
//     
//     
//     static (int, int) chooseSeat()
//     {
//         Writers.WriteOk("Введите номер ряда и места через пробел");
//         var seatCoord = UserInput.GetTwoPosIntsSepBySpace();
//         return (seatCoord.int1 - 1, seatCoord.int2 - 1);
//     }
//         
//         static void MakeBooking(Hall hall, Accounting accounting, User user)
//         {
//             var seatCoord = choose_seat();
//             if (!hall.SeatExists(seatCoord.Item1, seatCoord.Item2))
//             {
//                 Console.WriteLine("Такого места нет!\n");
//                 return;
//             }
//         
//             if (!hall.seatIsAvailable(seatCoord.Item1, seatCoord.Item2))
//             {
//                 Console.WriteLine("Это место уже занято\n");
//                 return;
//             }
//         
//             int price = hall.GetSeatPrice(seatCoord.Item1, seatCoord.Item2);
//         
//             if (user.GetBalance() < price)
//             {
//                 Console.WriteLine("Не хватает денежек :(\n");
//                 return;
//             }
//         
//             hall.book_seat(seatCoord.Item1, seatCoord.Item2);
//             user.ChargeOff(price);
//             accounting.AddCheck(price);
//             Console.WriteLine("Оплата прошла успешно!");
//         }
//         
//         static void BuyTickets(Hall hall, Accounting accounting, User user)
//         {
//             int userAction = buyMenu();
//         
//             while (userAction != (int) BuyMenuOptions.BackToMainMenu)
//             {
//                 if (userAction == (int) BuyMenuOptions.BookSeat)
//                 {
//                     MakeBooking(hall, accounting, user);
//                 }
//         
//                 userAction = buyMenu();
//             }
//         }
//     public static void Run()
//     {
//         int userAction = UserInput.GetChoice(options, baseMessage);
//         while (userAction != Exit)
//         {
//             
//             
//             userAction = UserInput.GetChoice(options, baseMessage);
//         }
//     }
// }
//
// public class AdminMenu : Menu
// {
//     
// }
//
// public class AccountMenu : Menu
// {}
//
