using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessBot.Service
{
    internal class AudioService
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
