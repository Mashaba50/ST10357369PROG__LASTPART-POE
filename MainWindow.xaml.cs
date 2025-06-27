using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace CybersecurityChatbot
{
    public partial class MainWindow : Window
    {
        // Application state
        private string userName = "";
        private List<string> userInterests = new List<string>();
        private Random random = new Random();
        private Dictionary<string, string> memory = new Dictionary<string, string>();
        private List<string> activityLog = new List<string>();
        private List<TaskItem> tasks = new List<TaskItem>();
        private List<QuizQuestion> quizQuestions;
        private int currentQuestionIndex = -1;
        private int quizScore = 0;
        private bool isFirstInteraction = true;
        private TaskItem pendingTask = null;
        private bool awaitingReminderConfirmation = false;
        private bool inQuizMode = false;

        // Response dictionaries
        private Dictionary<string, List<string>> randomResponseLists = new Dictionary<string, List<string>>()
        {
            ["greeting"] = new List<string> {
                "Hello there!", "Hi!", "Greetings!", "Welcome!"
            },
            ["farewell"] = new List<string> {
                "Goodbye!", "Farewell!", "See you later!", "Stay safe!"
            },
            ["positive_acknowledgement"] = new List<string> {
                "Great!", "Excellent!", "Good to hear!", "Understood."
            },
            ["negative_acknowledgement"] = new List<string> {
                "Oh, I'm sorry to hear that.", "That's unfortunate.", "I understand."
            },
            ["general_inquiry"] = new List<string> {
                "That's an interesting question.", "Let me think about that.", "Could you tell me more?"
            }
        };

        private Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>()
        {
            ["password"] = new List<string> {
                "Use strong, unique passwords for each account.",
                "Avoid using personal info like birthdays in your password.",
                "Consider using a password manager to keep your accounts safe."
            },
            ["scam"] = new List<string> {
                "Be cautious of emails asking for personal information.",
                "Don't click on suspicious links — scammers mimic trusted brands.",
                "Always verify the source before sharing sensitive data."
            },
            ["privacy"] = new List<string> {
                "Review and update your privacy settings regularly.",
                "Use secure browsers and avoid public Wi-Fi.",
                "Limit the personal data you share on social media."
            }
        };

        private List<string> positiveSentiments = new List<string> { "curious", "interested", "excited", "good", "great", "thank you", "thanks" };
        private List<string> negativeSentiments = new List<string> { "worried", "confused", "frustrated", "scared", "bad", "terrible", "help", "problem" };

        public MainWindow()
        {
            InitializeComponent();
            InitializeQuizQuestions();
            LogActivity("Chatbot initialized");
            ShowWelcomeMessage();
        }

        private void ShowWelcomeMessage()
        {
            AddChatMessage("👋 Hi there! What's your name?", Colors.LightSkyBlue);
        }

        private void InitializeQuizQuestions()
        {
            quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion(
                    "What should you do if you receive an email asking for your password?",
                    new List<string> {
                        "A) Reply with your password",
                        "B) Delete the email",
                        "C) Report the email as phishing",
                        "D) Ignore it"
                    },
                    "C) Report the email as phishing",
                    "Correct! Reporting phishing emails helps prevent scams."
                ),
                new QuizQuestion(
                    "Which of these is a strong password?",
                    new List<string> {
                        "A) password123",
                        "B) JohnDoe1980",
                        "C) P@ssw0rd!2023",
                        "D) 12345678"
                    },
                    "C) P@ssw0rd!2023",
                    "Strong passwords include uppercase, lowercase, numbers, and symbols."
                ),
                new QuizQuestion(
                    "What does HTTPS in a website URL indicate?",
                    new List<string> {
                        "A) The site has high traffic",
                        "B) The site is encrypted and secure",
                        "C) The site is government-owned",
                        "D) The site is popular"
                    },
                    "B) The site is encrypted and secure",
                    "HTTPS ensures encrypted communication between your browser and the website."
                ),
                new QuizQuestion(
                    "What is two-factor authentication?",
                    new List<string> {
                        "A) Using two different passwords",
                        "B) Verifying identity with two different methods",
                        "C) Logging in from two devices",
                        "D) Having two security questions"
                    },
                    "B) Verifying identity with two different methods",
                    "2FA adds an extra layer of security beyond just a password."
                ),
                new QuizQuestion(
                    "Why should you avoid using public Wi-Fi for sensitive transactions?",
                    new List<string> {
                        "A) It's too slow",
                        "B) It might be monitored by attackers",
                        "C) It drains battery faster",
                        "D) It's more expensive"
                    },
                    "B) It might be monitored by attackers",
                    "Public Wi-Fi networks are often unsecured and vulnerable to snooping."
                )
            };
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessInput();
        }

        private void InputBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ProcessInput();
            }
        }

        private void ProcessInput()
        {
            if (string.IsNullOrWhiteSpace(InputBox.Text))
            {
                return;
            }

            string input = InputBox.Text.Trim();
            AddChatMessage($"You: {input}", Colors.LightBlue);
            InputBox.Clear();

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                AddChatMessage(GetRandomResponse("farewell"), Colors.LightGreen);
                LogActivity("Chat session ended");
                return;
            }

            if (isFirstInteraction)
            {
                userName = input;
                Remember("userName", userName);
                AddChatMessage($"Welcome, {userName}! I'm your Cybersecurity Assistant.", Colors.LightGreen);
                AddChatMessage("Ask me about passwords, phishing, privacy, or online scams.", Colors.LightSkyBlue);
                LogActivity($"User identified: {userName}");
                isFirstInteraction = false;
                return;
            }

            // Handle pending task reminder confirmation
            if (awaitingReminderConfirmation)
            {
                HandleReminderConfirmation(input);
                return;
            }

            // Handle quiz answers
            if (inQuizMode)
            {
                HandleQuizAnswer(input);
                return;
            }

            string sentimentResponse = DetectSentiment(input);
            string response = GetResponse(input);

            if (!string.IsNullOrEmpty(sentimentResponse))
            {
                AddChatMessage(sentimentResponse, Colors.LightCyan);
            }

            TypeWriteResponse(response, Colors.LightCyan);
        }

        private void HandleReminderConfirmation(string input)
        {
            if (input.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                AddChatMessage("When would you like to be reminded? (e.g., 'in 3 days', 'tomorrow')", Colors.LightCyan);
                awaitingReminderConfirmation = true; // Still waiting for time specification
            }
            else if (input.Equals("no", StringComparison.OrdinalIgnoreCase) ||
                     input.Equals("n", StringComparison.OrdinalIgnoreCase))
            {
                tasks.Add(pendingTask);
                LogActivity($"Task added: {pendingTask.Title} (no reminder)");
                AddChatMessage($"Task '{pendingTask.Title}' added without reminder.", Colors.LightGreen);
                pendingTask = null;
                awaitingReminderConfirmation = false;
            }
            else
            {
                // Try to parse time directly
                ParseAndSetReminder(input);
            }
        }

        private void ParseAndSetReminder(string timeInput)
        {
            DateTime? reminderDate = null;

            if (timeInput.Contains("tomorrow"))
            {
                reminderDate = DateTime.Now.AddDays(1);
            }
            else if (timeInput.Contains("next week"))
            {
                reminderDate = DateTime.Now.AddDays(7);
            }
            else
            {
                // Parse "in X days/hours"
                string[] parts = timeInput.Split(' ');
                if (parts.Length >= 2 && int.TryParse(parts[1], out int value))
                {
                    if (parts[2].StartsWith("day"))
                    {
                        reminderDate = DateTime.Now.AddDays(value);
                    }
                    else if (parts[2].StartsWith("hour"))
                    {
                        reminderDate = DateTime.Now.AddHours(value);
                    }
                }
            }

            if (reminderDate.HasValue)
            {
                pendingTask.Reminder = reminderDate.Value;
                tasks.Add(pendingTask);
                LogActivity($"Task added: {pendingTask.Title} (reminder: {reminderDate.Value:g})");
                AddChatMessage($"Got it! I'll remind you about '{pendingTask.Title}' on {reminderDate.Value:g}.", Colors.LightGreen);

                // Ask a quiz question after setting reminder
                AskRandomQuizQuestion();
            }
            else
            {
                AddChatMessage("I didn't understand that. Please specify a time like 'in 3 days' or 'tomorrow'.", Colors.OrangeRed);
                return;
            }

            pendingTask = null;
            awaitingReminderConfirmation = false;
        }

        private void AskRandomQuizQuestion()
        {
            if (quizQuestions.Count == 0) return;

            currentQuestionIndex = random.Next(quizQuestions.Count);
            var question = quizQuestions[currentQuestionIndex];

            AddChatMessage("\nHere's a quick cybersecurity quiz question:", Colors.Gold);
            AddChatMessage(question.QuestionText, Colors.White);

            foreach (var choice in question.Choices)
            {
                AddChatMessage(choice, Colors.LightGray);
            }

            inQuizMode = true;
        }

        private void HandleQuizAnswer(string input)
        {
            var currentQuestion = quizQuestions[currentQuestionIndex];
            bool isCorrect = input.Trim().Equals(currentQuestion.CorrectAnswer.Substring(0, 1), StringComparison.OrdinalIgnoreCase);

            if (isCorrect)
            {
                AddChatMessage($"✅ {currentQuestion.Explanation}", Colors.LightGreen);
            }
            else
            {
                AddChatMessage($"❌ Incorrect. The correct answer is: {currentQuestion.CorrectAnswer}", Colors.LightPink);
            }

            inQuizMode = false;
        }

        private string GetResponse(string input)
        {
            // Handle natural language task creation
            if (input.Contains("add task") || input.Contains("create task"))
            {
                return HandleTaskCreation(input);
            }

            // Handle reminder requests
            if (input.Contains("remind me") || input.Contains("set reminder"))
            {
                return HandleReminderRequest(input);
            }

            // Handle activity log requests
            if (input.Contains("what have you done") || input.Contains("show activity log") || input.Contains("recent actions"))
            {
                return ShowActivitySummary();
            }

            // Handle quiz requests
            if (input.Contains("start quiz") || input.Contains("test me"))
            {
                StartQuiz();
                return "Starting cybersecurity quiz...";
            }

            // Keyword handlers
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    RememberInterest(keyword);
                    return keywordResponses[keyword][random.Next(keywordResponses[keyword].Count)];
                }
            }

            // Memory recall
            if (input.Contains("remember") && userInterests.Count > 0)
            {
                string rememberedName = RetrieveFromMemory("userName");
                string namePart = string.IsNullOrEmpty(rememberedName) ? "" : $" {rememberedName},";
                return $"Yes{namePart} you previously mentioned you're interested in: {string.Join(", ", userInterests)}.";
            }

            if (input.Contains("what can you do"))
            {
                return "I can help with:\n- Password security advice\n- Phishing scam detection\n" +
                       "- Privacy protection tips\n- Cybersecurity quiz\n- Task reminders\n- Activity logs";
            }

            // Default response
            return $"{GetRandomResponse("general_inquiry")} I'm not sure I understood that. Could you ask about a specific cybersecurity topic?";
        }

        private string HandleTaskCreation(string input)
        {
            string taskTitle = ExtractTaskTitle(input);
            string taskDesc = $"Review account {taskTitle.ToLower()} settings to ensure your data is protected.";

            pendingTask = new TaskItem(taskTitle, taskDesc, DateTime.Now);
            awaitingReminderConfirmation = true;

            return $"Task added with the description \"{taskDesc}\". Would you like a reminder?";
        }

        private string ExtractTaskTitle(string input)
        {
            // Extract task title from command
            int startIndex = input.IndexOf('-');
            if (startIndex != -1)
            {
                return input.Substring(startIndex + 1).Trim();
            }

            startIndex = input.IndexOf("task");
            if (startIndex != -1)
            {
                return input.Substring(startIndex + 4).Trim();
            }

            return "Security Task";
        }

        private string HandleReminderRequest(string input)
        {
            string taskTitle = "Update password";
            if (input.Contains("privacy"))
            {
                taskTitle = "Review privacy settings";
            }
            else if (input.Contains("authentication") || input.Contains("2fa"))
            {
                taskTitle = "Enable two-factor authentication";
            }

            // Try to parse time
            DateTime? reminderDate = ParseReminderTime(input);
            if (reminderDate.HasValue)
            {
                var task = new TaskItem(taskTitle, "", reminderDate.Value);
                tasks.Add(task);
                LogActivity($"Reminder set: {taskTitle} on {reminderDate.Value:g}");
                return $"Reminder set for '{taskTitle}' on {reminderDate.Value:g}.";
            }

            // If we couldn't parse, create a pending task
            pendingTask = new TaskItem(taskTitle, "", DateTime.Now);
            awaitingReminderConfirmation = true;
            return $"When would you like to be reminded about '{taskTitle}'?";
        }

        private DateTime? ParseReminderTime(string input)
        {
            if (input.Contains("tomorrow"))
            {
                return DateTime.Now.AddDays(1);
            }
            if (input.Contains("next week"))
            {
                return DateTime.Now.AddDays(7);
            }

            // Parse "in X days"
            string[] parts = input.Split(' ');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Equals("in") && i + 2 < parts.Length)
                {
                    if (int.TryParse(parts[i + 1], out int value))
                    {
                        if (parts[i + 2].StartsWith("day"))
                        {
                            return DateTime.Now.AddDays(value);
                        }
                        if (parts[i + 2].StartsWith("week"))
                        {
                            return DateTime.Now.AddDays(value * 7);
                        }
                        if (parts[i + 2].StartsWith("hour"))
                        {
                            return DateTime.Now.AddHours(value);
                        }
                    }
                }
            }

            return null;
        }

        private string ShowActivitySummary()
        {
            if (activityLog.Count == 0)
            {
                return "No activities recorded yet.";
            }

            string summary = "Here's a summary of recent actions:\n";
            int count = Math.Min(5, activityLog.Count);
            int startIndex = activityLog.Count - count;

            for (int i = startIndex; i < activityLog.Count; i++)
            {
                summary += $"{i - startIndex + 1}. {activityLog[i]}\n";
            }

            return summary;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskTitleBox.Text))
            {
                AddChatMessage("Please enter a task title", Colors.OrangeRed);
                return;
            }

            var newTask = new TaskItem(
                TaskTitleBox.Text,
                TaskDescBox.Text,
                ReminderPicker.SelectedDate ?? DateTime.Now.AddDays(7)
            );

            tasks.Add(newTask);
            LogActivity($"Task added: {newTask.Title}");

            TaskTitleBox.Clear();
            TaskDescBox.Clear();
            RefreshTaskList();

            AddChatMessage($"🔔 Task added: {newTask.Title}", Colors.LightGreen);
        }

        private void RefreshTaskList()
        {
            TaskListBox.ItemsSource = null;
            TaskListBox.ItemsSource = tasks;
        }

        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskItem selectedTask)
            {
                TaskTitleBox.Text = selectedTask.Title;
                TaskDescBox.Text = selectedTask.Description;
                ReminderPicker.SelectedDate = selectedTask.Reminder;
            }
        }

        private void StartQuiz()
        {
            MainTabControl.SelectedItem = QuizTab;
            currentQuestionIndex = 0;
            quizScore = 0;
            ShowNextQuestion();
            LogActivity("Quiz started");
        }

        private void ShowNextQuestion()
        {
            if (currentQuestionIndex >= quizQuestions.Count)
            {
                EndQuiz();
                return;
            }

            var question = quizQuestions[currentQuestionIndex];
            QuizQuestion.Text = $"{currentQuestionIndex + 1}. {question.QuestionText}";

            QuizOptionsPanel.Children.Clear();
            foreach (var choice in question.Choices)
            {
                var radioButton = new RadioButton
                {
                    Content = choice,
                    Foreground = Brushes.White,
                    Margin = new Thickness(0, 5, 0, 5),
                    FontSize = 14
                };
                QuizOptionsPanel.Children.Add(radioButton);
            }

            QuizFeedback.Text = "";
            QuizScore.Text = $"Score: {quizScore}/{currentQuestionIndex}";
            QuizSubmitButton.IsEnabled = true;
        }

        private void QuizSubmit_Click(object sender, RoutedEventArgs e)
        {
            var selectedOption = QuizOptionsPanel.Children
                .OfType<RadioButton>()
                .FirstOrDefault(rb => rb.IsChecked == true);

            if (selectedOption == null)
            {
                QuizFeedback.Text = "Please select an answer!";
                QuizFeedback.Foreground = Brushes.Orange;
                return;
            }

            var currentQuestion = quizQuestions[currentQuestionIndex];
            bool isCorrect = (selectedOption.Content.ToString() == currentQuestion.CorrectAnswer);

            if (isCorrect)
            {
                quizScore++;
                QuizFeedback.Text = $"✅ Correct! {currentQuestion.Explanation}";
                QuizFeedback.Foreground = Brushes.LightGreen;
            }
            else
            {
                QuizFeedback.Text = $"❌ Incorrect. The correct answer is: {currentQuestion.CorrectAnswer}";
                QuizFeedback.Foreground = Brushes.LightPink;
            }

            currentQuestionIndex++;
            QuizSubmitButton.IsEnabled = false;

            // Auto-proceed to next question after delay
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2.5) };
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                ShowNextQuestion();
            };
            timer.Start();
        }

        private void EndQuiz()
        {
            string resultMessage;
            Color color;

            if (quizScore == quizQuestions.Count)
            {
                resultMessage = "🎉 Perfect score! You're a cybersecurity expert!";
                color = Colors.Gold;
            }
            else if (quizScore >= quizQuestions.Count * 0.7)
            {
                resultMessage = "👍 Great job! You have strong cybersecurity knowledge!";
                color = Colors.LightGreen;
            }
            else
            {
                resultMessage = "💡 Good effort! Review the questions to improve your knowledge.";
                color = Colors.LightSalmon;
            }

            AddChatMessage($"\n📊 Quiz Results: {quizScore}/{quizQuestions.Count}", Colors.LightCyan);
            AddChatMessage(resultMessage, color);
            LogActivity($"Quiz completed. Score: {quizScore}/{quizQuestions.Count}");
        }

        private void LogActivity(string activity)
        {
            string timestampedActivity = $"{DateTime.Now:HH:mm}: {activity}";
            activityLog.Add(timestampedActivity);
            ActivityLogList.ItemsSource = null;
            ActivityLogList.ItemsSource = activityLog;
        }

        private string DetectSentiment(string input)
        {
            if (positiveSentiments.Any(input.Contains))
            {
                return $"{GetRandomResponse("positive_acknowledgement")} 👍";
            }
            if (negativeSentiments.Any(input.Contains))
            {
                return $"{GetRandomResponse("negative_acknowledgement")} 🤔";
            }
            return "";
        }

        private void RememberInterest(string keyword)
        {
            if (!userInterests.Contains(keyword))
            {
                userInterests.Add(keyword);
                LogActivity($"Interest recorded: {keyword}");
            }
        }

        private string GetRandomResponse(string listName)
        {
            if (randomResponseLists.ContainsKey(listName) && randomResponseLists[listName].Any())
            {
                return randomResponseLists[listName][random.Next(randomResponseLists[listName].Count)];
            }
            return "";
        }

        private void Remember(string key, string value)
        {
            memory[key] = value;
        }

        private string RetrieveFromMemory(string key)
        {
            return memory.ContainsKey(key) ? memory[key] : null;
        }

        private void AddChatMessage(string text, Color color)
        {
            var textBlock = new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(color),
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Margin = new Thickness(0, 5, 0, 5),
                FontSize = 14
            };
            ChatItems.Items.Add(textBlock);
            ScrollChatToBottom();
        }

        private async void TypeWriteResponse(string text, Color color)
        {
            var textBlock = new TextBlock
            {
                Foreground = new SolidColorBrush(color),
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Margin = new Thickness(0, 5, 0, 15),
                FontSize = 14
            };
            ChatItems.Items.Add(textBlock);

            for (int i = 0; i < text.Length; i++)
            {
                textBlock.Text += text[i];
                ScrollChatToBottom();
                await Task.Delay(25);
            }
        }

        private void ScrollChatToBottom()
        {
            ChatScrollViewer.ScrollToEnd();
        }
    }

    // Supporting classes
    public class TaskItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder { get; set; }

        public TaskItem(string title, string description, DateTime reminder)
        {
            Title = title;
            Description = description;
            Reminder = reminder;
        }
    }

    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public List<string> Choices { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }

        public QuizQuestion(string question, List<string> choices, string correctAnswer, string explanation = "")
        {
            QuestionText = question;
            Choices = choices;
            CorrectAnswer = correctAnswer;
            Explanation = string.IsNullOrEmpty(explanation) ? "" : explanation;
        }
    }
}