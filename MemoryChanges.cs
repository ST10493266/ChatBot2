using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessBot.Service
{
    /// <summary>
    /// Stores facts shared by the user and exposes them for personalised responses.
    /// </summary>
    internal class MemoryService
    {// General key→value store (e.g. "name" → "Alice")
        private readonly Dictionary<string, string> _facts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Topics the user has expressed interest in
        private readonly List<string> _interests = new List<string>();

        // Topic the bot is currently discussing (for conversation-flow follow-ups)
        public string CurrentTopic { get; set; }

        // ── Public API ────────────────────────────────────────────────────────

        public void Remember(string key, string value)
        {
            _facts[key] = value;
        }

        public string Recall(string key)
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
            _interests.Any(i => i.IndexOf(topic, StringComparison.OrdinalIgnoreCase) >= 0);

        /// <summary>
        /// Builds a personalised context hint to prepend when bot brings up a topic
        /// the user has expressed interest in before.
        /// </summary>
        public string PersonalisedHint(string topic)
        {
            if (HasInterest(topic))
                return $" As someone interested in {topic}, here's what you should know:\n\n";
            return string.Empty;
        }

        /// <summary>Tries to extract and store interests from user input.</summary>
        public string TryExtractInterest(string input)
        {
            var topics = new string[] { "password", "phishing", "browsing", "malware", "privacy", "2fa", "vpn", "scam" };
            var interestPhrases = new string[] { "interested in", "i like", "i love", "tell me about", "i'm into", "curious about" };

            foreach (var phrase in interestPhrases)
                if (input.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0)
                    foreach (var topic in topics)
                        if (input.IndexOf(topic, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            AddInterest(topic);
                            return topic;
                        }

            return null;
        }
    }
}
