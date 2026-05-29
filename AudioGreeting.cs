using System;
using System.IO;

namespace CybersecurityBot
{
    // Question 1: Voice Greeting
    // To use: record a WAV file named "greeting.wav" and place it in the output folder.
    // On Windows, System.Media.SoundPlayer plays WAV files.
    // On Linux/Mac the file check still runs but playback is skipped gracefully.
    class AudioGreeting
    {
        public static void PlayGreeting()
        {
            string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

            if (File.Exists(audioPath))
            {
                try
                {
                    // System.Media is Windows-only; wrapped in try/catch for cross-platform safety
                    var player = new System.Media.SoundPlayer(audioPath);
                    player.PlaySync();
                }
                catch
                {
                    // Silently skip if platform does not support System.Media
                }
            }
            else
            {
                // Simulate greeting with a short pause so it feels intentional
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\n  [Voice Greeting] Hello! Welcome to the Cybersecurity Awareness Bot.");
                Console.WriteLine("                   I'm here to help you stay safe online.");
                Console.ResetColor();
                Thread.Sleep(1500);
            }
        }
    }
}
