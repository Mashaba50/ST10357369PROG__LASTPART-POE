using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CybersecurityChatbot;

namespace POE_for_Prog_last_part.Quiz
{
    /// <summary>
    /// Manages the cybersecurity quiz lifecycle including:
    /// - Question storage and sequencing
    /// - Answer validation
    /// - Score tracking
    /// - Result calculation
    /// </summary>
    public class QuizManager
    {
        // Stores all quiz questions
        private readonly List<QuizQuestion> _questions = new List<QuizQuestion>();

        // Tracks current question index (-1 = quiz not started)
        private int _currentIndex = -1;

        // Accumulates correct answer count
        private int _score = 0;

        /// <summary>
        /// Indicates if quiz is in progress
        /// </summary>
        public bool IsQuizActive => _currentIndex >= 0 && _currentIndex < _questions.Count;

        /// <summary>
        /// Initializes quiz questions on creation
        /// </summary>
        public QuizManager()
        {
            InitializeQuestions();
        }

        /// <summary>
        /// Populates the question bank with cybersecurity questions
        /// </summary>
        private void InitializeQuestions()
        {
            _questions.AddRange(new[]
            {
                new QuizQuestion(
                    "What should you do if you receive an email asking for your password?",
                    new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                    2,  // Correct option index (Report...)
                    "Correct! Reporting phishing emails helps prevent scams."
                ),
                new QuizQuestion(
                    "Which of these is a strong password?",
                    new List<string> { "password123", "JohnDoe1980", "P@ssw0rd!2023", "12345678" },
                    2,  // Correct option index (P@ssw0rd!2023)
                    "Strong passwords include uppercase, lowercase, numbers, and symbols."
                ),
                new QuizQuestion(
                    "What does HTTPS in a website URL indicate?",
                    new List<string> { "The site has high traffic", "The site is encrypted and secure", "The site is government-owned", "The site is popular" },
                    1,  // Correct option index (encrypted and secure)
                    "HTTPS ensures encrypted communication between your browser and the website."
                )
            });
        }

        /// <summary>
        /// Resets quiz state to start new session
        /// </summary>
        public void StartQuiz()
        {
            _currentIndex = -1;
            _score = 0;
        }

        /// <summary>
        /// Retrieves next question in sequence
        /// </summary>
        /// <returns>Next QuizQuestion or null if quiz ended</returns>
        public QuizQuestion NextQuestion()
        {
            if (++_currentIndex >= _questions.Count) return null;
            return _questions[_currentIndex];
        }

        /// <summary>
        /// Validates user's answer for current question
        /// </summary>
        /// <param name="selectedOption">User's selected option index (0-based)</param>
        /// <returns>Result with correctness and explanation</returns>
        public QuizResult SubmitAnswer(int selectedOption)
        {
            var question = _questions[_currentIndex];
            bool isCorrect = selectedOption == question.CorrectOptionIndex;

            if (isCorrect) _score++;

            return new QuizResult(
                isCorrect: isCorrect,
                explanation: question.Explanation
            );
        }

        /// <summary>
        /// Generates final quiz results
        /// </summary>
        /// <returns>QuizResults object with score metrics</returns>
        public QuizResults GetResults()
        {
            return new QuizResults(_score, _questions.Count);
        }
    }
}