
using UserNamespace;

namespace MenuNamespace
{
    public class Menu
    {
        public static int TryOrBack()
        {
            int tmpChoice;
            Console.WriteLine("1 - попробовать еще раз");
            Console.WriteLine("2 - вернуться в меню");
            do
            {
                tmpChoice = int.Parse(Console.ReadLine());
                if (tmpChoice != 1 && tmpChoice != 2)
                    Console.WriteLine("Выберите корректную опцию");
            } while (tmpChoice != 1 && tmpChoice != 2);

            return tmpChoice;
        }

        public static ConsoleKey TryAgain()
        {
            Console.WriteLine("Нажмите ENTER для продолжения...");
            var key = Console.ReadKey(true).Key;
            return key;
        }

        public static void CheckLogin(ref string login)
        {
            do
            {
                Console.WriteLine("Введите логин:");
                login = Console.ReadLine();
                if (UserFieldChecker.CheckString(login) == false)
                {
                    Console.WriteLine("В логине есть недопустимые символы или он слишком короткий.");
                    continue;
                }
                break;
            } while (true);
        }

        public static void CheckPassword(ref string password)
        {
            do
            {
                Console.WriteLine("Введите пароль:");
                password = Console.ReadLine();
                if (UserFieldChecker.CheckString(password) == false)
                {
                    Console.WriteLine("В пароле есть недопустимые символы или он слишком короткий.");
                    continue;
                }

                break;
            } while (true);
        }

        public static int LoginOrRegistration()
        {
            Console.Clear();
            Console.WriteLine("1 - войти в аккаунт");
            Console.WriteLine("2 - создать аккаунт");
            int choice;

            do
            {
                choice = int.Parse(Console.ReadLine());
                if (choice != 1 && choice != 2)
                    Console.WriteLine("Выберите корректную опцию");
            } while (choice != 1 && choice != 2);

            return choice;
        }

        public static int MainMenuChoice()
        {
            Console.Clear();
            int menuChoice;
            Console.WriteLine("Добро пожаловать!");
            Console.WriteLine("1 - начать новую викторину");
            Console.WriteLine("2 - посмотреть результаты своих прошлых викторин");
            Console.WriteLine("3 - посмотреть топ-20 по конкретной викторине");
            Console.WriteLine("4 - настройки");
            Console.WriteLine("5 - выход");

            do
            {
                menuChoice = int.Parse(Console.ReadLine());

            } while (menuChoice < 1 || menuChoice > 5);
            return menuChoice;
        }

        public static int SettingsMenuChoice()
        {
            Console.Clear();
            int settingsChoice;
            Console.WriteLine("1 - сменить пароль");
            Console.WriteLine("2 - сменить логин");
            Console.WriteLine("3 - сменить дату рождения");
            Console.WriteLine("4 - назад");

            do
            {
                settingsChoice = int.Parse(Console.ReadLine());
            }while(settingsChoice<1 || settingsChoice > 4);

            return settingsChoice;
        }

        public static int QuizMenuChoice()
        {
            Console.Clear();
            int quizChoice;
            Console.WriteLine("1 - иcтория");
            Console.WriteLine("2 - физика");
            Console.WriteLine("3 - биология");
            Console.WriteLine("4 - смешанная викторина");
            Console.WriteLine("5 - вернуться назад");
            do
            {
                quizChoice = int.Parse(Console.ReadLine());
            } while (quizChoice < 1 || quizChoice > 4);

            return quizChoice;

        }

    }
}
