using System;
using System.Collections.Generic;

namespace CybersecurityBot
{
    // Question 4: Basic Response System
    // Question 5: Input Validation
    class ResponseEngine
    {
        private readonly string _name;

        // Keyword → response mapping
        private readonly Dictionary<string, string> _responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // ── Conversational ────────────────────────────────────────────────
            {
                "how are you",
                "I'm running at full capacity, thanks for asking! 😊 Ready to help you stay cyber-safe today."
            },
            {
                "what's your purpose",
                "My purpose is to educate users like you on cybersecurity best practices — covering passwords, phishing, safe browsing, privacy, and more!"
            },
            {
                "purpose",
                "I'm a Cybersecurity Awareness Bot. I help users understand online threats and how to defend against them."
            },
            {
                "what can i ask you about",
                "Great question! You can ask me about: passwords, phishing, safe browsing, malware, privacy, 2FA, VPNs, and more. Type 'help' to see the full list."
            },

            // ── Question 4: Cybersecurity Topics ─────────────────────────────
            {
                "password",
                "🔐 Password Safety Tips:\n\n" +
                "      • Use at least 12 characters with a mix of letters, numbers & symbols.\n" +
                "      • Never reuse passwords across different sites.\n" +
                "      • Use a reputable password manager (e.g. Bitwarden, 1Password).\n" +
                "      • Avoid using personal info like birthdays or names.\n" +
                "      • Change passwords immediately if a breach is suspected."
            },
            {
                "phishing",
                "🎣 Spotting Phishing Scams:\n\n" +
                "      • Check the sender's email address carefully — spoofed domains look similar.\n" +
                "      • Hover over links before clicking to preview the real URL.\n" +
                "      • Legitimate companies never ask for passwords via email.\n" +
                "      • Look for urgent or threatening language — it's a red flag.\n" +
                "      • When in doubt, go directly to the website instead of clicking links."
            },
            {
                "browsing",
                "🌐 Safe Browsing Habits:\n\n" +
                "      • Always look for HTTPS (🔒) in the address bar before entering data.\n" +
                "      • Keep your browser and extensions up to date.\n" +
                "      • Avoid downloading files from untrusted websites.\n" +
                "      • Use a reputable ad-blocker to reduce malicious ad risk.\n" +
                "      • Clear cookies and cache periodically."
            },
            {
                "malware",
                "🦠 Malware & Virus Protection:\n\n" +
                "      • Install reputable antivirus software and keep it updated.\n" +
                "      • Never open email attachments from unknown senders.\n" +
                "      • Keep your operating system patched with the latest updates.\n" +
                "      • Avoid pirated software — it often contains hidden malware.\n" +
                "      • Back up your data regularly to guard against ransomware."
            },
            {
                "privacy",
                "🕵️ Protecting Your Privacy:\n\n" +
                "      • Review app permissions — only grant what's necessary.\n" +
                "      • Use a privacy-focused browser or search engine (e.g. Brave, DuckDuckGo).\n" +
                "      • Limit personal information shared on social media.\n" +
                "      • Opt out of data-sharing where possible in app settings.\n" +
                "      • Use encrypted messaging apps like Signal for sensitive chats."
            },
            {
                "2fa",
                "🔑 Two-Factor Authentication (2FA):\n\n" +
                "      • Enable 2FA on all important accounts (email, banking, social media).\n" +
                "      • Prefer an authenticator app (Google Authenticator, Authy) over SMS.\n" +
                "      • 2FA means attackers need your password AND your device to log in.\n" +
                "      • Never share your 2FA codes with anyone — even 'support staff'.\n" +
                "      • Store backup codes safely in case you lose your device."
            },
            {
                "vpn",
                "🛡️ Using a VPN:\n\n" +
                "      • A VPN encrypts your internet traffic, especially useful on public Wi-Fi.\n" +
                "      • Choose a reputable, no-log VPN provider (e.g. ProtonVPN, Mullvad).\n" +
                "      • Free VPNs often monetise your data — avoid them.\n" +
                "      • A VPN hides your IP address but does NOT make you completely anonymous.\n" +
                "      • Always enable the kill switch feature to prevent data leaks."
            },
        };

        public ResponseEngine(string name)
        {
            _name = name;
        }

        // ── Question 4 & 5: Match input to a response ─────────────────────────
        public string GetResponse(string input)
        {
            // Question 5: Input Validation — handle empty/null input
            if (string.IsNullOrWhiteSpace(input))
                return "I didn't quite understand that. Could you rephrase? (Type 'help' for topic list)";

            string trimmed = input.Trim();

            // Direct match
            if (_responses.TryGetValue(trimmed, out string exact))
                return exact;

            // Keyword/partial match
            foreach (var kvp in _responses)
            {
                if (trimmed.IndexOf(kvp.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                    return kvp.Value;
            }

            // Question 5: Default fallback response
            return $"I didn't quite understand that, {_name}. Could you rephrase?\n\n" +
                   "      Try asking about: password, phishing, browsing, malware, privacy, 2fa, or vpn.\n" +
                   "      Type 'help' to see all topics.";
        }

        public bool IsExitCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            string t = input.Trim().ToLower();
            return t == "exit" || t == "quit" || t == "bye" || t == "goodbye";
        }

        public bool IsHelpCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            return input.Trim().Equals("help", StringComparison.OrdinalIgnoreCase);
        }
    }
}
