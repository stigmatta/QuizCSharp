using UserNamespace;
using UserDBNamespace;
using MenuNamespace;
using QuizNamespace;

UserDB userDB = new UserDB();


LoginOrRegistration:


string login=null, password = null;
DateOnly dateOnly;
User currentUser = null;
ConsoleKey key;

int choice = Menu.LoginOrRegistration();


Console.Clear();

Menu.CheckLogin(ref login);
Menu.CheckPassword(ref password);


switch (choice)
{
    case 1:
        try
        {
        LoginOnly:

            if (!userDB.CheckPasswordWithLogin(login, password,ref currentUser))
            {
                Console.Clear();
                goto LoginOrRegistration;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        break;
    case 2:
        if (userDB.CheckForUserInDB(login))
        {
            Console.WriteLine("Такой логин уже существует");
            do
            {
                 key = Menu.TryAgain();
            } while (key != ConsoleKey.Enter);

            goto LoginOrRegistration;
        }

    BirthdayDate:

        Console.WriteLine("Впишите свою дату рождения в формате YYYY-MM-DD (необязательно, ENTER чтобы пропустить)");
        string date = Console.ReadLine();

        if (string.IsNullOrEmpty(date))
        {
            User tmp = new User(login, password);
            userDB.AddUser(tmp);
            currentUser = tmp;
            userDB.RefreshDB();
        }

        else if (!string.IsNullOrEmpty(date) && DateOnly.TryParse(date, out DateOnly tmp))
        {
            if (tmp < new DateOnly(1920, 1, 1) || tmp > new DateOnly(2020, 1, 1))
                goto BirthdayDate;
            User tmpUser = new User(login, password,date);
            userDB.AddUser(tmpUser);
            currentUser = tmpUser;
            userDB.RefreshDB();
        }

        else
            goto BirthdayDate;

        break;

}


do
{
} while (Console.ReadKey(true).Key != ConsoleKey.Enter);


MainMenu:
int menuChoice = Menu.MainMenuChoice();

switch (menuChoice)
{
    case 1:
        int quizChoice = Menu.QuizMenuChoice();
        Quiz quiz = null;
        int correct_answers = 0;
        switch (quizChoice)
        {
            case 1:
                quiz = new History();
                break;
            case 2:
                quiz = new Physics();
                break;
            case 3:
                quiz = new Biology();
                break;
            case 4:
                quiz = new Shuffle();
                break;
            case 5:
                goto MainMenu;
                
        }
        if(quiz.QuizType == "Shuffle")

            quiz.CreateShuffledQuiz();

        if(quiz != null)
            correct_answers = quiz.StartQuiz();
        currentUser.AddResult(quiz.QuizType, correct_answers);
        userDB.RefreshUser(currentUser);
        goto MainMenu;

    case 2:
        currentUser.ShowPrevResults();
        do
        {
            key = Menu.TryAgain();
        } while (key != ConsoleKey.Enter);
        goto MainMenu;
    case 3:
        int quizTopChoice = Menu.QuizMenuChoice();
        string category = null;
        switch (quizTopChoice)
        {
            case 1:
                category = "History";
                break;
            case 2:
                category = "Physics";
                break;
            case 3:
                category = "Biology";
                break;
            case 4:
                category = "Shuffle";
                break;
            case 5:
                goto MainMenu;
        }
        if (category != null)
            userDB.TopInCategory(category);
        do
        {
            key = Menu.TryAgain();
        } while (key != ConsoleKey.Enter);
        goto MainMenu;
    case 4:
        int settingsChoice = Menu.SettingsMenuChoice();
        switch (settingsChoice)
        {
            case 1:
                userDB.ChangePassword(ref currentUser);
                break;
            case 2:
                userDB.ChangeLogin(ref currentUser);
                break;
            case 3:
                userDB.ChangeBirthday(ref currentUser);
                break;
            case 4: goto MainMenu;
        }
        break;
    case 5: goto LoginOrRegistration;
}
goto MainMenu;










