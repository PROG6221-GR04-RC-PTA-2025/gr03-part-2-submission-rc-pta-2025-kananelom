using System;
using System.Speech.Synthesis;
using System.Media;
using System.IO;
using System.Collections.Generic;

internal class Program
{
    private static Random rand = new Random();
    private static string favoriteTopic = "";

    private static void Main(string[] args)
    {
        PlayVoiceGreeting();
        DisplayAsciiArt();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("What is your name? ");
        Console.ResetColor();
        string userName = Console.ReadLine().Trim();
        if (string.IsNullOrEmpty(userName)) userName = "User";

        Console.ForegroundColor = ConsoleColor.Green;
        string greeting = $"Welcome, {userName}! I am Stacy, your Cybersecurity Awareness Assistant. How can I help you today?";
        Console.WriteLine(greeting);
        Console.ResetColor();

        SpeechSynthesizer synth = new SpeechSynthesizer();
        synth.Speak(greeting);

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nYou: ");
            Console.ResetColor();
            string userInput = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(userInput))
            {
                RespondWithVoice("I'm not sure I understand. Can you try rephrasing?", synth);
                continue;
            }

            string lowerInput = userInput.ToLower();

            if (lowerInput.Contains("exit") || lowerInput.Contains("quit") || lowerInput.Contains("bye"))
            {
                string goodbye = "Goodbye! Stay safe online!";
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(goodbye);
                Console.ResetColor();
                synth.Speak(goodbye);
                break;
            }

            if (lowerInput.Contains("i like "))
            {
                int index = lowerInput.IndexOf("i like ") + 7;
                favoriteTopic = userInput.Substring(index).Trim('.').Trim();
                RespondWithVoice($"Great! I'll remember that you're interested in {favoriteTopic}.", synth);
                continue;
            }

            if (lowerInput.Contains("my favorite topic is "))
            {
                int index = lowerInput.IndexOf("my favorite topic is ") + 22;
                favoriteTopic = userInput.Substring(index).Trim('.').Trim();
                RespondWithVoice($"Noted! I'll remember that your favorite topic is {favoriteTopic}.", synth);
                continue;
            }

            if (lowerInput.Contains("what's my favorite topic") || lowerInput.Contains("do you remember my favorite"))
            {
                string memoryResponse = string.IsNullOrEmpty(favoriteTopic)
                    ? "I don't think you've told me your favorite topic yet."
                    : $"You mentioned your favorite topic is {favoriteTopic}. Want to learn more about it?";
                RespondWithVoice(memoryResponse, synth);
                continue;
            }

            string response;
            if (lowerInput.Contains("how are you"))
            {
                response = "I'm a bot, but I'm ready and fully operational to help you stay secure online!";
            }
            else if (lowerInput.Contains("your purpose"))
            {
                response = "I'm here to educate and support you with best practices for staying safe in the digital world.";
            }
            else if (lowerInput.Contains("what can i ask"))
            {
                response = "You can ask me about cybersecurity topics like phishing, password safety, privacy, online scams, and more.";
            }
            else if (lowerInput.Contains("what is phishing"))
            {
                response = "Phishing is a type of cyber attack where attackers trick you into giving up personal or sensitive information by pretending to be trustworthy entities.";
            }
            else if (lowerInput.Contains("what is password safety"))
            {
                response = "Password safety means using strong, unique passwords for each account, and storing them securely using a password manager.";
            }
            else if (lowerInput.Contains("what is privacy"))
            {
                response = "Privacy refers to protecting your personal data and limiting what others can see or access about you online.";
            }
            else if (lowerInput.Contains("phishing"))
            {
                response = GetRandomPhishingTip();
            }
            else if (lowerInput.Contains("password"))
            {
                response = GetRandomPasswordTip();
            }
            else if (lowerInput.Contains("safe browsing"))
            {
                response = GetRandomBrowsingTip();
            }
            else
            {
                response = RespondToKeyword(lowerInput) ?? "I'm not sure I understand. Can you try rephrasing?";
            }

            string sentiment = DetectSentiment(lowerInput);
            if (sentiment == "worried")
            {
                response += " It's okay to feel concerned—cybersecurity can be complex, but I'm here to guide you.";
            }
            else if (sentiment == "frustrated")
            {
                response += " I understand it can be frustrating. Let me help make things clearer for you.";
            }
            else if (sentiment == "curious")
            {
                response += " I'm glad you're curious! Learning about cybersecurity is a great step toward staying safe online.";
            }

            RespondWithVoice(response, synth);
        }
    }

    static void RespondWithVoice(string message, SpeechSynthesizer synth)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nStacy: {message}");
        Console.ResetColor();
        synth.Speak(message);
    }

    static void PlayVoiceGreeting()
    {
        string audioFilePath = "greeting.wav.wav";
        if (File.Exists(audioFilePath))
        {
            SoundPlayer player = new SoundPlayer(audioFilePath);
            player.PlaySync();
        }
    }

    static void DisplayAsciiArt()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@" 
  ____  _                 _         _     
 / ___|| |__   __ _ _ __ | | _____ | |_   
 \___ \| '_ \ / _` | '_ \| |/ / _ \| __|  
  ___) | | | | (_| | | | |   < (_) | |_   
 |____/|_| |_|\__,_|_| |_|_|\_\___/ \__|  
                                          ");
        Console.ResetColor();
    }

    static string RespondToKeyword(string input)
    {
        if (input.Contains("password"))
        {
            return GetRandomPasswordTip();
        }
        else if (input.Contains("scam"))
        {
            return "🚨 Be cautious of unsolicited messages, especially those that request personal info or urgent action.";
        }
        else if (input.Contains("privacy"))
        {
            return "🛡️ Limit sharing personal information on public platforms. Use privacy settings on all your accounts.";
        }
        return null;
    }

    static string GetRandomPhishingTip()
    {
        List<string> phishingTips = new List<string>
        {
            "🔎 Always inspect the sender's email address before clicking any link.",
            "📧 Be cautious with attachments from unknown sources—scan them before opening.",
            "🔐 Avoid entering credentials into popup windows or unknown websites.",
            "⚠️ Look for grammar errors or generic greetings—they're common in phishing emails."
        };
        return phishingTips[rand.Next(phishingTips.Count)];
    }

    static string GetRandomPasswordTip()
    {
        List<string> passwordTips = new List<string>
        {
            "🔐 Use a password manager to store and generate complex passwords.",
            "🧠 Avoid using personal details like birth dates or names in passwords.",
            "🔁 Change your passwords regularly and don't reuse them across sites.",
            "💡 Combine uppercase, lowercase, numbers, and symbols in your passwords."
        };
        return passwordTips[rand.Next(passwordTips.Count)];
    }

    static string GetRandomBrowsingTip()
    {
        List<string> browsingTips = new List<string>
        {
            "🌐 Always check for HTTPS before entering sensitive information.",
            "🚫 Avoid clicking suspicious pop-ups or ads on websites.",
            "🛑 Don’t download files from unknown or untrusted sources.",
            "🔄 Keep your browser updated for the latest security features."
        };
        return browsingTips[rand.Next(browsingTips.Count)];
    }

    static string DetectSentiment(string input)
    {
        if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious"))
            return "worried";
        if (input.Contains("confused") || input.Contains("angry") || input.Contains("frustrated"))
            return "frustrated";
        if (input.Contains("interested") || input.Contains("curious") || input.Contains("learn"))
            return "curious";
        return "neutral";
    }
}
