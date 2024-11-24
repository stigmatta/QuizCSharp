using System.Text.Json;

namespace QuizNamespace
{
    public class Question
    {
        public string QuestionText { get; set; }
        public Dictionary<string, bool> Answers { get; set; } = new Dictionary<string, bool>();
    }

    public abstract class Quiz
    {
        protected string FilePath { get; set; } = string.Empty;
        public string QuizType { get; set; } = string.Empty;
        public List<Question> Questions { get; set; } = new List<Question>();

        public void LoadQuestions()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("Файл с вопросами не найден.");
                return;
            }

            try
            {
                string jsonContent = File.ReadAllText(FilePath);
                Questions = JsonSerializer.Deserialize<List<Question>>(jsonContent);

                if (Questions == null)
                {
                    Console.WriteLine("Не удалось загрузить вопросы.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при десериализации: {ex.Message}");
            }
        }


        public void CreateShuffledQuiz()
        {
            List<Question> Questions = new List<Question>();

            string jsonContent = File.ReadAllText("biology.json");
            List<Question> biologyQuestions = JsonSerializer.Deserialize<List<Question>>(jsonContent);

            jsonContent = File.ReadAllText("history.json");
            List<Question> historyQuestions = JsonSerializer.Deserialize<List<Question>>(jsonContent);

            jsonContent = File.ReadAllText("physics.json");
            List<Question> physicsQuestions = JsonSerializer.Deserialize<List<Question>>(jsonContent);

            Questions.AddRange(biologyQuestions);
            Questions.AddRange(historyQuestions);
            Questions.AddRange(physicsQuestions);


            Random rng = new Random();
            List<Question> shuffledQuestions = Questions.OrderBy(q => rng.Next()).ToList();

            var endResult = shuffledQuestions.Take(20);

            string shuffleJson = JsonSerializer.Serialize(endResult);
            File.WriteAllText("shuffle.json", shuffleJson);

        }


    public int StartQuiz()
        {
            LoadQuestions();
            Console.Clear();
            int correct_answers = 0;


            for (int i = 0; i < Questions.Count;i++)
            {
                Console.Clear();
                int your_answer;
                Console.WriteLine(Questions[i].QuestionText);
                int answerIndex = 1;

                foreach (var answer in Questions[i].Answers)
                    Console.WriteLine($"{answerIndex++}. {answer.Key}");


                do
                {
                    your_answer = int.Parse(Console.ReadLine());
                } while (your_answer < 1 || your_answer > 4);

               
                if (Questions[i].Answers.ElementAt(your_answer - 1).Value)
                    correct_answers++;
            }

            do
            {
                Console.WriteLine($"Вы набрали {correct_answers} правильных ответов из 20. Нажмите ENTER,чтобы вернуться в главное меню");
            } while (Console.ReadKey(true).Key != ConsoleKey.Enter);

            return correct_answers;
        }
    }

    public class Biology : Quiz
    {
        public Biology()
        {
            FilePath = "biology.json";
            QuizType = "Biology";
        }
    }

    public class History : Quiz
    {
        public History()
        {
            QuizType = "History";
            FilePath = "history.json";
        }
    }

    public class Physics: Quiz
    {
        public Physics()
        {
            QuizType = "Physics";
            FilePath = "physics.json";
        }
    }

    public class Shuffle : Quiz
    {
        public Shuffle()
        {
            QuizType = "Shuffle";
            FilePath = "shuffle.json";
        }
    }


}
