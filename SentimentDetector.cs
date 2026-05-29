using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CybersecurityAwarenessBot.Service
{
    internal class SentimentDetector
    {
        // Sentiment → keywords mapping
        private static readonly Dictionary<string, string[]> _map = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            { "worried",    new string[] { "worried", "scared", "afraid", "nervous", "anxious", "terrified", "fear", "frightened", "panic" } },
            { "frustrated", new string[] { "frustrated", "annoyed", "angry", "mad", "furious", "fed up", "useless", "pointless", "hate" } },
            { "confused",   new string[] { "confused", "don't understand", "don't get it", "unclear", "lost", "what does", "what is", "huh" } },
            { "curious",    new string[] { "curious", "interested", "want to know", "how does", "tell me more", "explain", "what about" } },
            { "happy",      new string[] { "great", "awesome", "thanks", "thank you", "helpful", "love", "good", "nice", "perfect" } },
        };

        /// <summary>Returns the detected sentiment label, or "neutral" if none found.</summary>
        public static string Detect(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "neutral";

            foreach (var kvp in _map)
                foreach (var kw in kvp.Value)
                    if (input.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
                        return kvp.Key;

            return "neutral";
        }

        /// <summary>
        /// Returns an empathetic prefix based on the detected sentiment,
        /// to prepend to the bot's normal response.
        /// </summary>
        public static string GetEmpathyPrefix(string sentiment)
        {
            switch (sentiment)
            {
                case "worried": return " It's completely understandable to feel that way — cybersecurity can feel overwhelming. Let me help ease your concerns.\n\n";
                case "frustrated": return " I'm sorry you're feeling frustrated. Let me try to make this clearer for you.\n\n";
                case "confused": return " No worries at all — let me break this down simply for you.\n\n";
                case "curious": return " Great curiosity! Learning about this is one of the best ways to stay safe.\n\n";
                case "happy": return " Glad to hear that! Keep up the great security habits!\n\n";
                default: return string.Empty;
            }
        }
    }
}


    
