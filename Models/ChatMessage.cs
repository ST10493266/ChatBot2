namespace CybersecurityBotGUI.Models
{
    /// <summary>Represents a single message in the chat history.</summary>
    public class ChatMessage
    {
        public string Sender  { get; set; } = string.Empty;   // "Bot" or user name
        public string Content { get; set; } = string.Empty;
        public string Time    { get; set; } = string.Empty;
        public bool   IsBot   { get; set; }
        public string SentimentTag { get; set; } = string.Empty; // e.g. "worried", "curious"
    }
}
