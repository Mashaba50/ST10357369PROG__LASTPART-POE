using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CybersecurityChatbot;

namespace POE_for_Prog_last_part.Quiz
{
    public class QuizManager
    {
        private readonly List<QuizQuestion> _questions = new List<QuizQuestion>();
        private int _currentIndex = -1;
        private int _score = 0;
        public bool IsQuizActive => _currentIndex >= 0 && _currentIndex < _questions.Count;

        public QuizManager()
        {
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            _questions.AddRange(new[]
            {
            new QuizQuestion(
                "What should you do if you receive an email asking for your password?",
                new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                2,
                "Correct! Reporting phishing emails helps prevent scams."
            ),
            new QuizQuestion(
                "Which of these is a strong password?",
                new List<string> { "password123", "JohnDoe1980", "P@ssw0rd!2023", "12345678" },
                2,
                "Strong passwords include uppercase, lowercase, numbers, and symbols."
            ),
            new QuizQuestion(
                "What does HTTPS in a website URL indicate?",
                new List<string> { "The site has high traffic", "The site is encrypted and secure", "The site is government-owned", "The site is popular" },
                1,
                "HTTPS ensures encrypted communication between your browser and the website."
            )
        });
        }
        public void StartQuiz()
        {
            _currentIndex = -1;
            _score = 0;
        }

        public QuizQuestion NextQuestion()
        {
            if (++_currentIndex >= _questions.Count) return null;
            return _questions[_currentIndex];
        }

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

        public QuizResults GetResults()
        {
            return new QuizResults(_score, _questions.Count);
        }
    }

} 