using System.IO;

namespace CybersecurityBotGUI.Services
{
    /// <summary>
    /// Question 1 – Voice Greeting (carried over from Part 1).
    /// Attempts to play greeting.wav from the application directory.
    /// Silently skips if the file is absent or the platform lacks audio support.
    /// </summary>
    public static class AudioService
    {
        public static void PlayGreeting()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
                if (!File.Exists(path)) return;

                // System.Media.SoundPlayer is Windows-only; safe on WPF/Windows target
                var player = new System.Media.SoundPlayer(path);
                player.Play(); // async so UI is not blocked
            }
            catch
            {
                // Silently skip — audio is an enhancement, not a requirement
            }
        }
    }
}
