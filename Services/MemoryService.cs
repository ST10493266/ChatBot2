namespace CybersecurityBotGUI.Services
{
    /// <summary>
    /// Question 5 – Memory and Recall.
    /// Stores facts shared by the user and exposes them for personalised responses.
    /// </summary>
    public class MemoryService
    {
        // General key→value store (e.g. "name" → "Alice")
        private readonly Dictionary<string, string> _facts = new(StringComparer.OrdinalIgnoreCase);

        // Topics the user has expressed interest in
        private readonly List<string> _interests = new();

        // Topic the bot is currently discussing (for conversation-flow follow-ups)
        public string? CurrentTopic { get; set; }

        // ── Public API ────────────────────────────────────────────────────────

        public void Remember(string key, string value)
        {
            _facts[key] = value;
        }

        public string? Recall(string key)
        {
            return _facts.TryGetValue(key, out var v) ? v : null;
        }

        public void AddInterest(string topic)
        {
            if (!_interests.Contains(topic, StringComparer.OrdinalIgnoreCase))
                _interests.Add(topic);
        }

        public IReadOnlyList<string> GetInterests() => _interests.AsReadOnly();

        public bool HasInterest(string topic) =>
            _interests.Any(i => i.Contains(topic, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Builds a personalised context hint to prepend when bot brings up a topic
        /// the user has expressed interest in before.
        /// </summary>
        public string PersonalisedHint(string topic)
        {
            if (HasInterest(topic))
                return $"💡 As someone interested in {topic}, here's what you should know:\n\n";
            return string.Empty;
        }

        /// <summary>Tries to extract and store interests from user input.</summary>
        public string? TryExtractInterest(string input)
        {
            var topics = new[] { "password", "phishing", "browsing", "malware", "privacy", "2fa", "vpn", "scam" };
            var interestPhrases = new[] { "interested in", "i like", "i love", "tell me about", "i'm into", "curious about" };

            foreach (var phrase in interestPhrases)
                if (input.Contains(phrase, StringComparison.OrdinalIgnoreCase))
                    foreach (var topic in topics)
                        if (input.Contains(topic, StringComparison.OrdinalIgnoreCase))
                        {
                            AddInterest(topic);
                            return topic;
                        }

            return null;
        }
    }
}
