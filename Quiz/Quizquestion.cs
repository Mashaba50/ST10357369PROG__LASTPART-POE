using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_for_Prog_last_part.Quiz
{
    public class QuizQuestion
    {
        public string Question { get; }
        public List<string> Options { get; }
        public int CorrectOptionIndex { get; }
        public string Explanation { get; }

        public QuizQuestion(string question, List<string> options, int correctOptionIndex, string explanation)
        {
            Question = question;
            Options = options;
            CorrectOptionIndex = correctOptionIndex;
            Explanation = explanation;
        }
    }

    public class QuizResult
    {
        public bool IsCorrect { get; }
        public string Explanation { get; }

        public QuizResult(bool isCorrect, string explanation)
        {
            IsCorrect = isCorrect;
            Explanation = explanation;
        }
    }

    public class QuizResults
    {
        public int Score { get; }
        public int Total { get; }

        public QuizResults(int score, int total)
        {
            Score = score;
            Total = total;
        }
    }
}