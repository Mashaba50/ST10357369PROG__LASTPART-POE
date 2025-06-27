using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_for_Prog_last_part.Quiz
{
    /// <summary>
    /// Represents a single quiz question with its options and correct answer
    /// </summary>
    public class QuizQuestion
    {
        /// <summary>
        /// Gets the question text
        /// </summary>
        public string Question { get; }

        /// <summary>
        /// Gets the list of possible answer options
        /// </summary>
        public List<string> Options { get; }

        /// <summary>
        /// Gets the index of the correct option (0-based)
        /// </summary>
        public int CorrectOptionIndex { get; }

        /// <summary>
        /// Gets the explanation shown after answering the question
        /// </summary>
        public string Explanation { get; }

        /// <summary>
        /// Constructs a new quiz question
        /// </summary>
        /// <param name="question">The question text</param>
        /// <param name="options">List of answer options</param>
        /// <param name="correctOptionIndex">Index of correct answer (0-based)</param>
        /// <param name="explanation">Explanation of the answer</param>
        public QuizQuestion(string question, List<string> options, int correctOptionIndex, string explanation)
        {
            Question = question;
            Options = options;
            CorrectOptionIndex = correctOptionIndex;
            Explanation = explanation;
        }
    }

    /// <summary>
    /// Represents the result of answering a single quiz question
    /// </summary>
    public class QuizResult
    {
        /// <summary>
        /// Indicates whether the user's answer was correct
        /// </summary>
        public bool IsCorrect { get; }

        /// <summary>
        /// Explanation of the correct answer
        /// </summary>
        public string Explanation { get; }

        /// <summary>
        /// Constructs a quiz result
        /// </summary>
        /// <param name="isCorrect">True if answer was correct</param>
        /// <param name="explanation">Educational explanation</param>
        public QuizResult(bool isCorrect, string explanation)
        {
            IsCorrect = isCorrect;
            Explanation = explanation;
        }
    }

    /// <summary>
    /// Represents the final results of a completed quiz
    /// </summary>
    public class QuizResults
    {
        /// <summary>
        /// Gets the number of correctly answered questions
        /// </summary>
        public int Score { get; }

        /// <summary>
        /// Gets the total number of questions in the quiz
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// Constructs the final quiz results
        /// </summary>
        /// <param name="score">Number of correct answers</param>
        /// <param name="total">Total questions in quiz</param>
        public QuizResults(int score, int total)
        {
            Score = score;
            Total = total;
        }
    }
}