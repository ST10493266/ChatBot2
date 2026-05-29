using CybersecurityBotGUI.Services;

namespace CybersecurityBotGUI.Services
{
    /// <summary>
    /// Question 2 – Keyword Recognition
    /// Question 3 – Random Responses
    /// Question 4 – Conversation Flow
    /// Question 7 – Error Handling
    /// Question 8 – Code Optimisation (OOP, Dictionary, List)
    /// </summary>
    public class ResponseEngine
    {
        private readonly Random       _rng    = new();
        private readonly MemoryService _memory;
        private          string        _userName;

        public ResponseEngine(string userName, MemoryService memory)
        {
            _userName = userName;
            _memory   = memory;
        }

        // ── Question 3: Multiple random responses per topic ───────────────────
        private static readonly Dictionary<string, List<string>> _randomResponses =
            new(StringComparer.OrdinalIgnoreCase)
        {
            ["phishing"] = new()
            {
                "🎣 Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations — always verify the sender's address.",
                "🎣 Phishing tip: Hover over links before clicking. The real destination URL often reveals a fake domain designed to look legitimate.",
                "🎣 Watch out for urgency tactics! Phishing emails often say things like 'Your account will be closed in 24 hours' to make you act without thinking.",
                "🎣 Legitimate companies never ask for your password via email or SMS. If you receive such a request, go directly to the official website instead.",
                "🎣 Check for spelling mistakes and poor grammar — they're classic signs of a phishing attempt crafted outside your country.",
            },
            ["scam"] = new()
            {
                "🚨 Scam alert: If something sounds too good to be true, it almost certainly is. Lottery wins, inheritance notices, and crypto doubling schemes are classic scams.",
                "🚨 Never send money or gift cards to someone you've only met online. Romance scammers invest weeks building trust before asking.",
                "🚨 Tech-support scams: Microsoft and Apple will NEVER call you out of the blue. Hang up and call the official number if you're unsure.",
                "🚨 Verify before you trust — always call your bank or institution directly if you receive a suspicious message, even if it looks official.",
            },
            ["password"] = new()
            {
                "🔐 Use at least 12 characters mixing uppercase, lowercase, numbers, and symbols. Avoid dictionary words — attackers use automated tools to crack them.",
                "🔐 Never reuse passwords across sites. If one service is breached, attackers try the same credentials everywhere — called 'credential stuffing'.",
                "🔐 A password manager (e.g. Bitwarden, 1Password) generates and stores strong unique passwords for every site, so you only remember one master password.",
                "🔐 Avoid personal details like birthdays, pet names, or your address — these are the first things attackers try.",
            },
        };

        // ── Question 2: Single-response keyword map ───────────────────────────
        private static readonly Dictionary<string, string> _keywordResponses =
            new(StringComparer.OrdinalIgnoreCase)
        {
            ["browsing"] =
                "🌐 Safe Browsing Tips:\n\n" +
                "  • Always look for HTTPS (🔒) before entering any personal data.\n" +
                "  • Keep your browser and extensions updated — outdated plugins are a common attack vector.\n" +
                "  • Use a reputable ad-blocker (e.g. uBlock Origin) to block malicious ads.\n" +
                "  • Clear your cookies and cache periodically to reduce tracking.\n" +
                "  • Avoid downloading files from untrusted or unfamiliar websites.",

            ["malware"] =
                "🦠 Malware & Virus Protection:\n\n" +
                "  • Install reputable antivirus software and keep virus definitions updated.\n" +
                "  • Never open email attachments from unknown senders — even PDFs can carry exploits.\n" +
                "  • Keep your operating system patched; most malware exploits known, unpatched vulnerabilities.\n" +
                "  • Avoid pirated software — it frequently contains hidden trojans or ransomware.\n" +
                "  • Back up your data regularly (3-2-1 rule: 3 copies, 2 media types, 1 offsite).",

            ["privacy"] =
                "🕵️ Protecting Your Privacy:\n\n" +
                "  • Review app permissions — revoke anything that doesn't need access to your camera, mic, or location.\n" +
                "  • Use privacy-focused tools: Brave browser, DuckDuckGo search, Signal messenger.\n" +
                "  • Limit personal info on social media — attackers use it to craft convincing phishing attacks.\n" +
                "  • Opt out of data sharing in app and account settings wherever possible.\n" +
                "  • Use encrypted messaging (Signal, WhatsApp end-to-end) for sensitive conversations.",

            ["2fa"] =
                "🔑 Two-Factor Authentication (2FA):\n\n" +
                "  • Enable 2FA on every important account — email, banking, social media.\n" +
                "  • Prefer an authenticator app (Google Authenticator, Authy) over SMS — SIM-swap attacks can intercept SMS codes.\n" +
                "  • 2FA means an attacker needs both your password AND your physical device to log in.\n" +
                "  • Never share a 2FA code with anyone — real support staff will never ask for it.\n" +
                "  • Store your backup codes securely (printed or in a password manager).",

            ["vpn"] =
                "🛡️ VPN Usage:\n\n" +
                "  • A VPN encrypts your traffic — essential on public Wi-Fi (cafés, airports, hotels).\n" +
                "  • Choose a reputable no-log provider: ProtonVPN, Mullvad, or ExpressVPN.\n" +
                "  • Free VPNs often sell your browsing data to advertisers — they're not truly 'free'.\n" +
                "  • A VPN hides your IP but does NOT make you fully anonymous online.\n" +
                "  • Enable the kill switch feature so traffic stops if the VPN drops unexpectedly.",

            ["ransomware"] =
                "💰 Ransomware Protection:\n\n" +
                "  • Regular backups are your best defence — ransomware can't hold data hostage if you have clean copies.\n" +
                "  • Keep all software patched; WannaCry spread through an unpatched Windows vulnerability.\n" +
                "  • Never pay the ransom — there's no guarantee you'll get your data back, and it funds more attacks.\n" +
                "  • Use application whitelisting to prevent unauthorised programs from running.\n" +
                "  • Isolate infected machines from the network immediately to prevent spread.",

            ["firewall"] =
                "🔥 Firewall Basics:\n\n" +
                "  • A firewall monitors incoming and outgoing network traffic and blocks suspicious connections.\n" +
                "  • Always keep your OS firewall enabled — it's your first line of defence.\n" +
                "  • Consider a hardware firewall for added protection on your home network.\n" +
                "  • Regularly review firewall rules and close ports you don't actively use.",

            ["social engineering"] =
                "🎭 Social Engineering Awareness:\n\n" +
                "  • Social engineering manipulates people rather than systems — it's often more effective than hacking.\n" +
                "  • Common tactics: pretexting, baiting, tailgating, quid pro quo, and vishing (voice phishing).\n" +
                "  • Always verify identity before sharing sensitive information, even with 'IT support'.\n" +
                "  • When in doubt, hang up and call back on an official, verified number.",

            ["how are you"] =
                "I'm operating at peak efficiency — fully patched and threat-free! 😊 Ready to help you stay cyber-safe.",

            ["purpose"] =
                "🤖 I'm the Cybersecurity Awareness Bot. My mission is to educate you on online threats and best practices — covering passwords, phishing, scams, privacy, malware, 2FA, VPNs, and more!",

            ["what can you do"] =
                "I can help you with:\n\n  • Password safety  • Phishing & scams  • Safe browsing\n  • Malware & ransomware  • Privacy  • 2FA  • VPNs\n  • Firewalls  • Social engineering\n\nJust type a topic or click one of the quick-access buttons!",
        };

        // ── Follow-up / conversation-flow triggers ────────────────────────────
        private static readonly string[] _moreKeywords =
            ["another tip", "more", "tell me more", "explain more", "go on", "continue", "elaborate", "another one"];

        private static readonly string[] _confusionKeywords =
            ["don't understand", "confused", "what do you mean", "clarify", "can you explain", "huh", "unclear"];

        // ── Public entry point ────────────────────────────────────────────────

        /// <summary>
        /// Main method: detects sentiment, checks memory, matches keywords,
        /// and returns the appropriate response.
        /// </summary>
        public string GetResponse(string input)
        {
            // Question 7: Input validation
            if (string.IsNullOrWhiteSpace(input))
                return "I didn't quite catch that. Could you rephrase? You can also click a topic button below. 💬";

            string trimmed = input.Trim();

            // ── Question 5: Memory – extract and store user interests ──────────
            var detectedInterest = _memory.TryExtractInterest(trimmed);
            if (detectedInterest != null)
            {
                _memory.AddInterest(detectedInterest);
                _memory.CurrentTopic = detectedInterest;
                return $"✅ Got it! I'll remember that you're interested in **{detectedInterest}**. " +
                       $"It's a crucial part of staying safe online.\n\n" +
                       GetTopicResponse(detectedInterest);
            }

            // ── Question 4: Conversation flow – follow-up / "tell me more" ────
            if (_moreKeywords.Any(k => trimmed.Contains(k, StringComparison.OrdinalIgnoreCase)))
            {
                if (_memory.CurrentTopic != null)
                    return GetTopicResponse(_memory.CurrentTopic, forceRandom: true);
                return "Sure! What topic would you like to know more about? Try: password, phishing, privacy, 2fa, vpn, scam, malware, or browsing.";
            }

            // ── Question 4: Conversation flow – confusion / ask to explain ─────
            if (_confusionKeywords.Any(k => trimmed.Contains(k, StringComparison.OrdinalIgnoreCase)))
            {
                if (_memory.CurrentTopic != null)
                    return $"Let me clarify that for you! Here's a fuller explanation on **{_memory.CurrentTopic}**:\n\n" +
                           GetTopicResponse(_memory.CurrentTopic);
                return "No problem! Let me know which topic you'd like me to explain more clearly — just type it or click a button below.";
            }

            // ── Question 2: Keyword recognition ───────────────────────────────
            // Check random-response topics first
            foreach (var (topic, _) in _randomResponses)
                if (trimmed.Contains(topic, StringComparison.OrdinalIgnoreCase))
                {
                    _memory.CurrentTopic = topic;
                    return _memory.PersonalisedHint(topic) + GetTopicResponse(topic);
                }

            // Then check single-response keyword map
            foreach (var (keyword, response) in _keywordResponses)
                if (trimmed.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    _memory.CurrentTopic = keyword;
                    return _memory.PersonalisedHint(keyword) + response;
                }

            // ── Question 7: Default fallback ──────────────────────────────────
            return $"I'm not sure I understand that, {_userName}. Could you rephrase? 🤔\n\n" +
                   "Try asking about: password, phishing, scam, browsing, malware, privacy, 2fa, vpn, ransomware, or firewall.\n" +
                   "Or click one of the quick-topic buttons!";
        }

        /// <summary>Returns a response for a known topic, picking randomly where applicable.</summary>
        private string GetTopicResponse(string topic, bool forceRandom = false)
        {
            if (_randomResponses.TryGetValue(topic, out var list))
                return list[_rng.Next(list.Count)];

            if (_keywordResponses.TryGetValue(topic, out var single))
                return single;

            return GetResponse(topic); // recursive fallback
        }

        // ── Command checks ─────────────────────────────────────────────────────

        public bool IsExitCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            var t = input.Trim().ToLower();
            return t is "exit" or "quit" or "bye" or "goodbye";
        }

        public bool IsHelpCommand(string input) =>
            input.Trim().Equals("help", StringComparison.OrdinalIgnoreCase);

        public void UpdateUserName(string name) => _userName = name;
    }
}
