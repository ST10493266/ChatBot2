using System;

namespace CybersecurityBot
{
    // Question 3 & 4 & 5 & 6: Main chatbot conversation loop
    class ChatBot
    {
        private readonly string _name;
        private readonly ResponseEngine _engine;

        public ChatBot(string name)
        {
            _name = name;
            _engine = new ResponseEngine(name);
        }

        public void Start()
        {
            Display.BotSay(_name,
                $"Hi {_name}! Type 'help' to see what I can assist you with, or just ask me anything about cybersecurity.");

            while (true)
            {
                // Question 6: User prompt with visual styling
                Display.UserPrompt(_name);
                string input = Console.ReadLine();

                // Exit commands
                if (_engine.IsExitCommand(input))
                {
                    Display.Divider();
                    Display.BotSay(_name, $"Goodbye, {_name}! Stay safe online! 🔐");
                    Display.Divider();
                    break;
                }

                // Help menu
                if (_engine.IsHelpCommand(input))
                {
                    Display.ShowHelp();
                    continue;
                }

                // Question 5: Input validation + Question 4: get response
                string response = _engine.GetResponse(input);
                Display.BotSay(_name, response);
                Display.Divider();
            }
        }
    }
}
