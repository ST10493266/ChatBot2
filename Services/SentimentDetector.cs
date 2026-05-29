namespace CybersecurityBotGUI.Services
{
    /// <summary>
    /// Question 6 – Sentiment Detection.
    /// Detects simple emotional cues in user input and returns a sentiment label
    /// that the ResponseEngine uses to adjust its tone.
    /// </summary>
    public static class SentimentDetector
    {
        // Sentiment → keywords mapping
        private static readonly Dictionary<string, string[]> _map = new(StringComparer.OrdinalIgnoreCase)
        {
            ["worried"]   = ["worried", "scared", "afraid", "nervous", "anxious", "terrified", "fear", "frightened", "panic"],
            ["frustrated"]= ["frustrated", "annoyed", "angry", "mad", "furious", "fed up", "useless", "pointless", "hate"],
            ["confused"]  = ["confused", "don't understand", "don't get it", "unclear", "lost", "what does", "what is", "huh"],
            ["curious"]   = ["curious", "interested", "want to know", "how does", "tell me more", "explain", "what about"],
            ["happy"]     = ["great", "awesome", "thanks", "thank you", "helpful", "love", "good", "nice", "perfect"],
        };

        /// <summary>Returns the detected sentiment label, or "neutral" if none found.</summary>
        public static string Detect(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "neutral";

            foreach (var (sentiment, keywords) in _map)
                foreach (var kw in keywords)
                    if (input.Contains(kw, StringComparison.OrdinalIgnoreCase))
                        return sentiment;

            return "neutral";
        }

        /// <summary>
        /// Returns an empathetic prefix based on the detected sentiment,
        /// to prepend to the bot's normal response.
        /// </summary>
        public static string GetEmpathyPrefix(string sentiment) => sentiment switch
        {
            "worried"    => "💙 It's completely understandable to feel that way — cybersecurity can feel overwhelming. Let me help ease your concerns.\n\n",
            "frustrated" => "😔 I'm sorry you're feeling frustrated. Let me try to make this clearer for you.\n\n",
            "confused"   => "🤔 No worries at all — let me break this down simply for you.\n\n",
            "curious"    => "😊 Great curiosity! Learning about this is one of the best ways to stay safe.\n\n",
            "happy"      => "😄 Glad to hear that! Keep up the great security habits!\n\n",
            _            => string.Empty
        };
    }
}
