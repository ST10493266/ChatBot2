# ChatBot2
Cybersecurity Awareness Bot 2
# Cybersecurity Awareness Bot — PROG6221 POE Part 2

A WPF GUI application expanding on the Part 1 console chatbot.

## New Features (Part 2)

| # | Feature | Implementation |
|---|---------|---------------|
| 1 | **GUI Design** | WPF dark-theme UI with chat bubbles, sidebar, ASCII art logo, voice greeting |
| 2 | **Keyword Recognition** | `ResponseEngine` matches 10+ cybersecurity keywords (password, phishing, scam, privacy, malware, 2fa, vpn, ransomware, firewall, social engineering) |
| 3 | **Random Responses** | `List<string>` per topic for phishing, scam, and password — randomly selected each time |
| 4 | **Conversation Flow** | Handles "tell me more", "another tip", "explain more" without restarting; remembers current topic |
| 5 | **Memory & Recall** | `MemoryService` stores user name + interests; sidebar shows tracked interests; responses personalised |
| 6 | **Sentiment Detection** | `SentimentDetector` identifies worried/curious/frustrated/confused/happy; sidebar mood indicator; empathy prefix added to responses |
| 7 | **Error Handling** | Null/empty input handled gracefully; fallback response with rephrasing suggestion |
| 8 | **Code Optimisation** | OOP: separated classes per responsibility; `Dictionary`, `List`, `string[]` for data; no magic strings |

## Project Structure

```
CybersecurityBotGUI/
├── App.xaml / App.xaml.cs          – Application entry point + global styles
├── Models/
│   └── ChatMessage.cs              – Message data model
├── Services/
│   ├── AudioService.cs             – Q1 voice greeting (WAV playback)
│   ├── MemoryService.cs            – Q5 memory and recall
│   ├── ResponseEngine.cs           – Q2 keywords, Q3 random, Q4 flow, Q7 error handling
│   └── SentimentDetector.cs        – Q6 sentiment detection
├── Views/
│   ├── MainWindow.xaml             – Q1 GUI layout (dark theme, bubbles, sidebar)
│   └── MainWindow.xaml.cs          – Q1/Q4/Q5/Q6 UI logic
└── CybersecurityBotGUI.csproj      – .NET 8 WPF project file
```

## How to Run

```bash
cd CybersecurityBotGUI
dotnet run
```

Requires **.NET 8 SDK** and **Windows** (WPF is Windows-only).

## Voice Greeting Setup

Place a `greeting.wav` file in the same folder as the compiled executable.  
The app runs fine without it — audio is skipped gracefully.

## Suggested Git Commit Messages

1. Part 2 init: Migrated console app to WPF GUI project structure.
2. Added dark-theme MainWindow with chat bubble layout and sidebar.
3. Implemented SentimentDetector for mood-aware empathetic responses.
4. Implemented MemoryService for user interest tracking and recall.
5. Expanded ResponseEngine with random responses and conversation flow.
6. Added keyword recognition for 10+ cybersecurity topics.
7. Wired up sentiment indicator and interest sidebar in MainWindow.
8. Added quick-topic buttons, help panel, and clear chat feature.

