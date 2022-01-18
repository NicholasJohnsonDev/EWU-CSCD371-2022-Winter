// Nicholas Johnson
using System.Collections;

namespace PrincessBrideTrivia;
public class Program
{
    public static void Main()
    {
        Welcome();
        try
        {
            PlayGame();
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occured while playing the game:\n" + e.ToString());
        }

    }

    private static void GameResults(int numberCorrect, int numberOfQuestions)
    {
        Console.WriteLine("You got " + GetPercentCorrect(numberCorrect, numberOfQuestions) + " correct! Restart program to try again."); //TODO: different response if Length is null
    }

    private static void PlayGame()
    {
        string filePath = GetFilePath();
        Question[] questions = LoadQuestions(filePath);
        questions = RandomizeQuestions(questions);

        int questionNumber = 1;
        int numberCorrect = 0;

        for (int i = 0; i < questions.Length; i++) //TODO: add null-coalescing operator ?? incase file empty set to 0
        {
            bool result = AskQuestion(questions[i], questionNumber);
            if (result)
            {
                numberCorrect++;
            }
        }

        GameResults(numberCorrect, questions.Length);
    }

    public static string GetPercentCorrect(int numberCorrectAnswers, int numberOfQuestions)
    {
        int percent = (int)Math.Round((double)numberCorrectAnswers / (double)numberOfQuestions * 100);
        return percent + "%";
    }

    private static void Welcome()
    {
        Console.WriteLine("Welcome to Princess Bride Trivia!\n" +
                "Please type in the number for the your answer and hit enter to submit.");
    }

    public static bool AskQuestion(Question question, int questionNumber)
    {
        DisplayQuestion(question, questionNumber);

        string userGuess = GetGuessFromUser();
        return DisplayResult(userGuess, question);
    }

    private static string GetGuessFromUser()
    {
        //TODO: handle possible responses other than 1, 2, or 3
        ArrayList acceptedAnsewers = new() { "1", "2", "3" };
        bool validAnswer = false;
        string userGuess = "";
        while (!validAnswer)
        {
            userGuess = Console.ReadLine().Trim();
            if (acceptedAnsewers.Contains(userGuess))
            {
                validAnswer = true;
            }
            else
            {
                validAnswer = false;
                Console.WriteLine("Invalid input. Please enter: 1, 2, or 3.");
            }
        }

        return userGuess;
    }

    public static bool DisplayResult(string userGuess, Question question)
    {
        if (userGuess == question.CorrectAnswerIndex)
        {
            Console.WriteLine("Correct!\n");
            return true;
        }

        Console.WriteLine("Incorrect!\n");
        return false;
    }

    public static void DisplayQuestion(Question question, int questionNumber)
    {
        Console.WriteLine($"Question {questionNumber}: " + question.Text);
        for (int i = 0; i < question.Answers.Length; i++) //TODO: handle for null question
        {
            Console.WriteLine((i + 1) + ": " + question.Answers[i]);
        }
    }

    public static string GetFilePath()
    {
        return "Trivia.txt";
    }

    public static Question[] LoadQuestions(string filePath)
    {
        //TODO: add a try catch in case no file 
        //TODO: handle different files, any number of questions
        string[] lines = File.ReadAllLines(filePath);

        Question[] questions = new Question[lines.Length / 5];
        for (int i = 0; i < questions.Length; i++)
        {
            int lineIndex = i * 5;
            string questionText = lines[lineIndex];

            string answer1 = lines[lineIndex + 1];
            string answer2 = lines[lineIndex + 2];
            string answer3 = lines[lineIndex + 3];

            string correctAnswerIndex = lines[lineIndex + 4];

            Question question = new Question();
            question.Text = questionText;
            question.Answers = new string[3];
            question.Answers[0] = answer1;
            question.Answers[1] = answer2;
            question.Answers[2] = answer3;
            question.CorrectAnswerIndex = correctAnswerIndex;
            questions[i] = question;
        }

        return questions;
    }

    public static Question[] RandomizeQuestions(Question[] questions)
    {
        // Returns randomized version of input without changing the input
        Random random = new();

        List<Question> questionsList = questions.ToList();
        Question[] randomizedQuestions = questions;

        do
        {
            var randomized = questionsList.OrderBy(item => random.Next());
            randomizedQuestions = randomized.ToArray();
        } while (questions == questionsList.ToArray());

        return randomizedQuestions;
    }
}
