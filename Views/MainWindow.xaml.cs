using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CybersecurityBotGUI.Models;
using CybersecurityBotGUI.Services;

namespace CybersecurityBotGUI.Views
{
    /// <summary>
    /// Question 1 – GUI design and implementation (WPF, all Part 1 features ported)
    /// Question 4 – Conversation Flow
    /// Question 5 – Memory and Recall (sidebar updates)
    /// Question 6 – Sentiment Detection (mood indicator)
    /// </summary>
    public partial class MainWindow : Window
    {
        private ResponseEngine? _engine;
        private MemoryService   _memory  = new();
        private string          _userName = "User";

        // Sentiment → display info
        private static readonly Dictionary<string, (string emoji, string label, string hex)> SentimentDisplay = new()
        {
            ["worried"]    = ("😟", "Worried",    "#F0C040"),
            ["frustrated"] = ("😤", "Frustrated", "#FF4C4C"),
            ["confused"]   = ("🤔", "Confused",   "#BC8CFF"),
            ["curious"]    = ("🧐", "Curious",    "#00D4FF"),
            ["happy"]      = ("😄", "Happy",      "#3FB950"),
            ["neutral"]    = ("😐", "Neutral",    "#8B949E"),
        };

        public MainWindow()
        {
            InitializeComponent();
            // Q1: Play voice greeting on startup
            AudioService.PlayGreeting();
            NameInput.Focus();
        }

        // ── Name entry ─────────────────────────────────────────────────────────

        private void NameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) StartChat_Click(sender, e);
        }

        private void StartChat_Click(object sender, RoutedEventArgs e)
        {
            string name = NameInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(name)) name = "User";

            _userName = name;
            _memory.Remember("name", name);
            _engine = new ResponseEngine(_userName, _memory);

            // Update sidebar
            SidebarUserName.Text = _userName;

            // Switch views
            NamePanel.Visibility    = Visibility.Collapsed;
            ChatScroller.Visibility = Visibility.Visible;
            MessageInput.IsEnabled  = true;
            SendBtn.IsEnabled       = true;

            // Welcome message
            AddBotMessage(
                $"👋 Hi {_userName}! I'm your Cybersecurity Awareness Bot.\n\n" +
                "I can help you with: passwords, phishing, scams, safe browsing, malware, privacy, 2FA, VPNs, ransomware, firewalls, and social engineering.\n\n" +
                "Type a question, pick a quick topic from the left panel, or just say 'help' to get started! 🔐");

            MessageInput.Focus();
        }

        // ── Sending messages ──────────────────────────────────────────────────

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(MessageInput.Text))
                Send_Click(sender, e);
        }

        private void MessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            // Clear placeholder styling if needed
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string input = MessageInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input) || _engine == null) return;

            MessageInput.Clear();

            // Display user bubble
            AddUserMessage(input);

            // Q6: Detect sentiment and update sidebar
            string sentiment = SentimentDetector.Detect(input);
            UpdateSentimentIndicator(sentiment);
            string empathyPrefix = SentimentDetector.GetEmpathyPrefix(sentiment);

            // Check for exit
            if (_engine.IsExitCommand(input))
            {
                AddBotMessage($"👋 Goodbye, {_userName}! Stay safe online! 🔐\n\nFeel free to reopen the app anytime.");
                MessageInput.IsEnabled = false;
                SendBtn.IsEnabled      = false;
                return;
            }

            // Check for help
            if (_engine.IsHelpCommand(input))
            {
                AddBotMessage(GetHelpText());
                return;
            }

            // Get response (Q2 keyword recognition, Q3 random, Q4 flow, Q5 memory)
            string response = _engine.GetResponse(input);

            // Prepend empathy prefix if sentiment detected
            if (!string.IsNullOrEmpty(empathyPrefix))
                response = empathyPrefix + response;

            AddBotMessage(response);

            // Q5: Update sidebar interests
            UpdateInterestsSidebar();
        }

        private void QuickTopic_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string query)
            {
                MessageInput.Text = query;
                Send_Click(sender, e);
            }
        }

        private void ClearChat_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Children.Clear();
            if (_engine != null)
                AddBotMessage("Chat cleared! Ask me anything about cybersecurity. 🔐");
        }

        // ── Message bubble builders ──────────────────────────────────────────

        private void AddUserMessage(string text)
        {
            var bubble = BuildBubble(text, isBot: false, _userName);
            ChatPanel.Children.Add(bubble);
            ScrollToBottom();
        }

        private void AddBotMessage(string text)
        {
            var bubble = BuildBubble(text, isBot: true, "🤖 CyberBot");
            ChatPanel.Children.Add(bubble);
            ScrollToBottom();
        }

        /// <summary>Builds a styled chat bubble UIElement.</summary>
        private UIElement BuildBubble(string text, bool isBot, string senderLabel)
        {
            // Outer row
            var row = new Grid { Margin = new Thickness(0, 6, 0, 6) };
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = isBot ? GridLength.Auto : new GridLength(1, GridUnitType.Star) });
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = isBot ? new GridLength(1, GridUnitType.Star) : GridLength.Auto });

            // Avatar circle
            var avatar = new Border
            {
                Width           = 32,
                Height          = 32,
                CornerRadius    = new CornerRadius(16),
                Background      = isBot
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00D4FF"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3FB950")),
                Margin          = isBot ? new Thickness(0, 0, 8, 0) : new Thickness(8, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
            };
            var avatarText = new TextBlock
            {
                Text              = isBot ? "🤖" : _userName.Substring(0, 1).ToUpper(),
                FontSize          = isBot ? 16 : 14,
                FontWeight        = FontWeights.Bold,
                Foreground        = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment   = VerticalAlignment.Center,
            };
            avatar.Child = avatarText;

            // Bubble content
            var bubbleStack = new StackPanel { MaxWidth = 560 };

            var nameLabel = new TextBlock
            {
                Text       = senderLabel + "  " + DateTime.Now.ToString("HH:mm"),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B949E")),
                FontSize   = 10,
                Margin     = new Thickness(isBot ? 2 : 0, 0, isBot ? 0 : 2, 3),
                HorizontalAlignment = isBot ? HorizontalAlignment.Left : HorizontalAlignment.Right,
            };
            bubbleStack.Children.Add(nameLabel);

            var bubble = new Border
            {
                Background    = isBot
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1C2128"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0D419D")),
                CornerRadius  = isBot
                    ? new CornerRadius(4, 12, 12, 12)
                    : new CornerRadius(12, 4, 12, 12),
                Padding       = new Thickness(14, 10, 14, 10),
                BorderBrush   = isBot
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#30363D"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A4B9B")),
                BorderThickness = new Thickness(1),
                HorizontalAlignment = isBot ? HorizontalAlignment.Left : HorizontalAlignment.Right,
            };

            var msgText = new TextBlock
            {
                Text            = text,
                Foreground      = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6EDF3")),
                FontSize        = 13,
                TextWrapping    = TextWrapping.Wrap,
                LineHeight      = 20,
            };
            bubble.Child = msgText;
            bubbleStack.Children.Add(bubble);

            // Layout: avatar left for bot, right for user
            if (isBot)
            {
                Grid.SetColumn(avatar,     0);
                Grid.SetColumn(bubbleStack, 0);
                var innerGrid = new Grid();
                innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Grid.SetColumn(avatar,      0);
                Grid.SetColumn(bubbleStack, 1);
                innerGrid.Children.Add(avatar);
                innerGrid.Children.Add(bubbleStack);
                Grid.SetColumn(innerGrid, 0);
                row.Children.Add(innerGrid);
                // Spacer on right
                var spacer = new Border();
                Grid.SetColumn(spacer, 1);
                row.Children.Add(spacer);
            }
            else
            {
                // Spacer on left
                var spacer = new Border();
                Grid.SetColumn(spacer, 0);
                row.Children.Add(spacer);

                var innerGrid = new Grid();
                innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                Grid.SetColumn(bubbleStack, 0);
                Grid.SetColumn(avatar,      1);
                innerGrid.Children.Add(bubbleStack);
                innerGrid.Children.Add(avatar);
                Grid.SetColumn(innerGrid, 1);
                row.Children.Add(innerGrid);
            }

            // Fade-in animation
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));
            row.BeginAnimation(OpacityProperty, fadeIn);

            return row;
        }

        // ── Sidebar updates ───────────────────────────────────────────────────

        private void UpdateSentimentIndicator(string sentiment)
        {
            if (!SentimentDisplay.TryGetValue(sentiment, out var info)) return;

            SentimentLabel.Text = $"{info.emoji}  {info.label}";
            try
            {
                SentimentBorder.Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(info.hex + "22")); // translucent
            }
            catch { /* ignore colour parse errors */ }
        }

        private void UpdateInterestsSidebar()
        {
            var interests = _memory.GetInterests();
            SidebarInterests.Text = interests.Count > 0
                ? "Interested in: " + string.Join(", ", interests)
                : "No interests tracked yet.";
        }

        private void ScrollToBottom()
        {
            ChatScroller.ScrollToEnd();
        }

        // ── Help text ─────────────────────────────────────────────────────────

        private static string GetHelpText() =>
            "📋 Here's what I can help you with:\n\n" +
            "  🔐 password       – strong password practices\n" +
            "  🎣 phishing       – spotting phishing emails\n" +
            "  🚨 scam           – common scam tactics\n" +
            "  🌐 browsing       – safe browsing habits\n" +
            "  🦠 malware        – malware & virus protection\n" +
            "  🕵️ privacy        – protecting your privacy\n" +
            "  🔑 2fa            – two-factor authentication\n" +
            "  🛡️ vpn             – VPN usage guide\n" +
            "  💰 ransomware     – ransomware prevention\n" +
            "  🔥 firewall       – firewall basics\n" +
            "  🎭 social engineering – human hacking tactics\n\n" +
            "  💬 'tell me more' / 'another tip' – follow-up on current topic\n" +
            "  📖 'explain more' – deeper explanation of current topic\n" +
            "  🚪 exit / quit    – close the chat\n\n" +
            "Or use the quick-topic buttons on the left panel!";
    }
}
