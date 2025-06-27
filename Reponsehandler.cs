using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_for_Prog_last_part
{
    public class ResponseHandler
    {
        public string DetectTopic(string input)
        {
            input = input.ToLower();

            if (input.Contains("privacy")) return "privacy";
            if (input.Contains("password")) return "password";
            if (input.Contains("phishing") || input.Contains("scam")) return "scam";
            if (input.Contains("two-factor") || input.Contains("2fa")) return "2fa";
            if (input.Contains("backup")) return "backup";
            if (input.Contains("update")) return "update";

            return null;
        }

        public string GetResponse(string topic)
        {
            // Traditional switch statement instead of switch expression
            switch (topic)
            {
                case "privacy":
                    return "Review your account privacy settings to ensure your data is protected";
                case "password":
                    return "Use strong, unique passwords for each account";
                case "scam":
                    return "Be cautious of emails asking for personal information";
                case "2fa":
                    return "Enable two-factor authentication for enhanced security";
                case "backup":
                    return "Create regular backups of your important data";
                case "update":
                    return "Keep your software updated to the latest version";
                default:
                    return "I can help you with various cybersecurity topics";
            }
        }

        public string GetFollowUp(string topic)
        {
            // Traditional switch statement instead of switch expression
            switch (topic)
            {
                case "privacy":
                    return "Also consider using a VPN for additional privacy protection";
                case "password":
                    return "Consider using a password manager to generate and store strong passwords";
                case "scam":
                    return "Always verify unexpected requests by contacting the company directly";
                case "2fa":
                    return "Use authenticator apps instead of SMS for better security";
                case "backup":
                    return "Store backups in multiple locations including offline storage";
                case "update":
                    return "Enable automatic updates whenever possible";
                default:
                    return "Here's another security tip: Always lock your devices when not in use";
            }
        }
    }
}
