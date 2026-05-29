using System;
using System.Threading;

namespace CybersecurityBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Question 1: Voice Greeting (plays audio if file exists)
            AudioGreeting.PlayGreeting();

            // Question 2: ASCII Image Display
            Display.ShowLogo();

            // Question 3: Text-Based Greeting and User Interaction
            string userName = Display.GetUserName();

            // Start chatbot loop
            ChatBot bot = new ChatBot(userName);
            bot.Start();
        }
    }
}
