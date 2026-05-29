using System;
using System.Threading;

namespace CybersecurityBot
{
    // Question 2: ASCII Image Display
    // Question 3: Text-Based Greeting and User Interaction
    // Question 6: Enhanced Console UI with Visual Elements
    class Display
    {
        // ── Colours ──────────────────────────────────────────────────────────
        private static void C(ConsoleColor fg) => Console.ForegroundColor = fg;
        private static void R() => Console.ResetColor();

        // ── Question 2: ASCII Logo ────────────────────────────────────────────
        public static void ShowLogo()
        {
            Console.Clear();
            C(ConsoleColor.Cyan);
            Console.WriteLine(@"
  ╔══════════════════════════════════════════════════════════════════╗
  ║                                                                  ║
  ║    ██████╗██╗   ██╗██████╗ ███████╗██████╗     ██████╗  ██████╗ ║
  ║   ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗    ██╔══██╗██╔═══██╗║
  ║   ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝    ██████╔╝██║   ██║║
  ║   ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗    ██╔══██╗██║   ██║║
  ║   ╚██████╗   ██║   ██████╔╝███████╗██║  ██║    ██████╔╝╚██████╔╝║
  ║    ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝    ╚═════╝  ╚═════╝ ║
  ║                                                                  ║
  ║          🔐  Cybersecurity Awareness Bot  v1.0  🔐               ║
  ║                  Keeping You Safe Online                         ║
  ╚══════════════════════════════════════════════════════════════════╝
");
            R();
            Thread.Sleep(800);
        }

        // ── Question 3: Ask for user name ─────────────────────────────────────
        public static string GetUserName()
        {
            Divider();
            C(ConsoleColor.Yellow);
            Console.Write("  👤  Please enter your name: ");
            R();

            string name = Console.ReadLine()?.Trim();

            // Input validation – default name if blank
            if (string.IsNullOrWhiteSpace(name))
                name = "User";

            Console.WriteLine();
            C(ConsoleColor.Green);
            Console.WriteLine($"  ✅  Welcome, {name}! I'm your Cybersecurity Awareness Bot.");
            Console.WriteLine("      Ask me anything about staying safe online.");
            R();
            Divider();
            Thread.Sleep(600);
            return name;
        }

        // ── UI Helpers ────────────────────────────────────────────────────────
        public static void Divider()
        {
            C(ConsoleColor.DarkGray);
            Console.WriteLine("  " + new string('─', 64));
            R();
        }

        public static void BotSay(string name, string message)
        {
            // Typing effect (Question 6)
            C(ConsoleColor.Cyan);
            Console.Write($"\n  🤖  Bot  ▶  ");
            R();

            foreach (char ch in message)
            {
                Console.Write(ch);
                Thread.Sleep(12); // simulate typing
            }
            Console.WriteLine("\n");
        }

        public static void UserPrompt(string name)
        {
            C(ConsoleColor.Yellow);
            Console.Write($"  👤  {name}  ▶  ");
            R();
        }

        public static void ShowHelp()
        {
            Divider();
            C(ConsoleColor.Magenta);
            Console.WriteLine("  📋  Topics I can help you with:\n");
            Console.WriteLine("      • password        – password safety tips");
            Console.WriteLine("      • phishing        – how to spot phishing scams");
            Console.WriteLine("      • browsing        – safe browsing habits");
            Console.WriteLine("      • malware         – malware & virus protection");
            Console.WriteLine("      • privacy         – protecting your privacy");
            Console.WriteLine("      • 2fa             – two-factor authentication");
            Console.WriteLine("      • vpn             – VPN usage");
            Console.WriteLine("      • how are you     – chat with me");
            Console.WriteLine("      • purpose         – learn what I do");
            Console.WriteLine("      • help            – show this menu");
            Console.WriteLine("      • exit / quit     – leave the chatbot\n");
            R();
            Divider();
        }

        public static void ShowError(string msg)
        {
            C(ConsoleColor.Red);
            Console.WriteLine($"\n  ⚠️   {msg}\n");
            R();
        }
    }
}
