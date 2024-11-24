using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace UserNamespace
{
    public class UserFieldChecker
    {
        public static string Pattern { get; private set; }
        static UserFieldChecker()
        {
            Pattern = @"^[a-zA-Z0-9_]{8,}$";
        }
        public static bool CheckString(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return Regex.IsMatch(value, Pattern);
        }
    }



    [Serializable]
    [DataContract]
    public class User
    {
        string login,password;
        [DataMember(Name = "Login")]
        public string Login {
            get => login;
            set
            {
                if(UserFieldChecker.CheckString(value))
                    login = value;
                else
                    Console.WriteLine("Логин в неверном формате");
            }
        }

        [DataMember(Name = "Password")]
        public string Password
        {
            get => password;
            set
            {
                if (UserFieldChecker.CheckString(value))
                    password = value;
                else
                    Console.WriteLine("Пароль в неверном формате");
            }
        }

        [DataMember(Name = "BirthdayDate")]
        public string Birthday { get; set; }

        [DataMember(Name = "Results")]
        public Dictionary<string,List<int>> Results { get; set; }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
            Birthday = null;
            Results = new Dictionary<string,List<int>>();
        }

        public void AddResult(string quizType,int correct_answers)
        {
            if(Results.ContainsKey(quizType))
            {
                Results[quizType].Add(correct_answers);
            }
            else
                Results.Add(quizType, new List<int>() { correct_answers});
        }
        public User(string login, string password,string birthday):this(login,password)
        {
            Birthday = birthday;
        }

        public void ShowPrevResults()
        {
            Console.WriteLine("Все ваши результаты предыдущих викторин");
            Console.Clear();
            foreach(var item in Results)
            {
                Console.WriteLine($"Тип викторины - {item.Key}");
                for(int i = 0; i < item.Value.Count; i++)
                {
                    Console.WriteLine($"{i+1}.Количество правильных ответов (из 20) - {item.Value[i]}");

                }
                Console.WriteLine();

            }
        }


    }

}
