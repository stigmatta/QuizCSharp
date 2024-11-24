using MenuNamespace;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UserNamespace;

namespace UserDBNamespace
{
    [Serializable]
    [DataContract]
    public class UserResult
    {
        public User User { get; set; }
        public string Key { get; set; }
        public List<int> Value { get; set; }
    }
    public class UserDB
    {
        private string filePath = "users.json";

        [DataMember]
        private List<User> users;

        public UserDB()
        {
            LoadUsersFromFile();
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }
        public void PrintUsers()
        {
            foreach (var user in users)
            {
                Console.WriteLine($"Login:{user.Login},Password:{user.Password},Birthday:{user.Birthday}");
            }
        }

        public void RefreshDB()
        {
            using (FileStream usersFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<User>));
                json.WriteObject(usersFile, users);
            }
            Console.WriteLine("База данных пользователей обновлена.");
        }

        public void RefreshUser(User current)
        {

            int removeInd = ReturnUserIndex(current);

            if (removeInd != -1)
            {
                users.RemoveAt(removeInd);
                users.Add(current);
            }
            RefreshDB();
        }
        public void ChangePassword(ref User current)
        {
            Console.Clear();
            Console.WriteLine("Введите ваш старый пароль:");
            string oldPassword = Console.ReadLine();

            if (oldPassword != current.Password)
            {
                Console.WriteLine("Неверный пароль.");
                Menu.TryAgain();
            }
            else
            {
                Console.WriteLine("Введите новый пароль:");
                string newPassword;

                do
                {
                    newPassword = Console.ReadLine();
                    if (UserFieldChecker.CheckString(newPassword))
                        break;

                    Console.WriteLine("Пароль должен соответствовать правилам. Попробуйте снова.");
                } while (true);

                current.Password = newPassword;

                int removeInd = ReturnUserIndex(current);

                if (removeInd != -1)
                {
                    users.RemoveAt(removeInd);
                    users.Add(current);
                }

                RefreshDB();

                Console.WriteLine("Пароль успешно изменён.");
            }
        }
        public void ChangeLogin(ref User current)
        {
            Console.Clear();
            Console.WriteLine("Введите ваш новый логин:");
            string newLogin;

            do
            {
                newLogin = Console.ReadLine();
                if (UserFieldChecker.CheckString(newLogin))
                    break;

                Console.WriteLine("Логин должен соответствовать правилам. Попробуйте снова.");
            } while (true);


            int removeInd = ReturnUserIndex(current);
            current.Login = newLogin;


            if (removeInd != -1)
            {
                users.RemoveAt(removeInd);
                users.Add(current);
            }

            RefreshDB();

            Console.WriteLine("Логин успешно изменён.");
        }

        public void ChangeBirthday(ref User current)
        {
            Console.Clear();
            Console.WriteLine("Введите дату рождения в формате (YY-MM-DD):");
            string date;

            do
            {
                date = Console.ReadLine();


                if (!string.IsNullOrEmpty(date) && DateOnly.TryParse(date, out DateOnly tmp))
                {
                    if (tmp < new DateOnly(1920, 1, 1) || tmp > new DateOnly(2020, 1, 1))
                        break;
                }
                break;

            } while (true);


            int removeInd = ReturnUserIndex(current);
            current.Birthday = date;


            if (removeInd != -1)
            {
                users.RemoveAt(removeInd);
                users.Add(current);
            }

            RefreshDB();

        }


        public bool CheckForUserInDB(string login)
        {
            FileStream usersFile = new FileStream(filePath, FileMode.OpenOrCreate);

            if (users.Count == 0)
            {
                usersFile.Close();
                return false;
            }
            foreach (var user in users)
            {
                if (user.Login == login)
                {
                    usersFile.Close();
                    return true;
                }
            }
            usersFile.Close();
            return false;
        }

        public bool CheckPasswordWithLogin(string login, string password, ref User current)
        {
            FileStream usersFile = new FileStream(filePath, FileMode.OpenOrCreate);

            if (users.Count == 0)
            {
                usersFile.Close();
                return false;
            }
            foreach (var user in users)
            {
                if (user.Login == login && user.Password == password)
                {
                    Console.WriteLine("Вы вошли в аккаунт. Нажмите ЕNTER, чтобы продолжить");
                    current = new User(user.Login, user.Password, user.Birthday);
                    current.Results = user.Results;
                    usersFile.Close();
                    return true;
                }
                else if (user.Login == login && user.Password != password)
                {
                    Console.WriteLine("Пароль неверен.");

                    usersFile.Close();
                    return false;
                }
            }

            Console.WriteLine("Такого профиля нет");

            usersFile.Close();
            return false;
        }

        public void LoadUsersFromFile()
        {
            FileStream usersFile = new FileStream(filePath, FileMode.OpenOrCreate);

            users = new List<User>();

            if (usersFile.Length > 0)
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<User>));

                users = (List<User>)json.ReadObject(usersFile);
            }

            usersFile.Close();
        }

        public int ReturnUserIndex(User cur)
        {

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Login == cur.Login)
                {
                    return i;
                }
            }
            return -1;

        }

        public void TopInCategory(string category)
        {
            Console.Clear();
            int limit = 5;
            List<UserResult> results = new List<UserResult>();

            for (int i = 0; i < users.Count; i++)
            {
                foreach (var item in users[i].Results)
                {
                    if (item.Key == category)
                    {
                        results.Add(new UserResult
                        {
                            User = users[i],
                            Key = item.Key,
                            Value = item.Value
                        });
                    }
                }
            }


            int index = 1;
            var topResults = results.OrderByDescending(result => result.Value.Max());
            foreach (var result in topResults)
            {
                Console.WriteLine($"{index++}. User: {result.User.Login}, Points: {result.Value.Max()}");
            }
        }

    }
}
