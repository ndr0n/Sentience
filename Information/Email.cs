using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sentience;
using UnityEngine;
using UnityEngine.Rendering.UI;

namespace Sentience
{
    [System.Serializable]
    public struct EmailParser
    {
        public string subject;
        public string sender;
        public string receiver;
        public string body;
    }

    [System.Serializable]
    public class Email
    {
        public string Subject;
        public string Sender;
        public string Receiver;
        public string Body;
        [SerializeReference] public Information Information;

        public Email(EmailParser parser, Information information)
        {
            Subject = parser.subject;
            Sender = parser.sender;
            Receiver = parser.receiver;
            Body = parser.body;
            Information = information;
        }

        public static async Task<Email> GenerateFromInformation(Information information, string location, Action<Email> onEmailGenerated = null)
        {
            string answer;
            string rules = "I will tell you the description of something that is happening and you must generate an old email that contains this information.\n" +
                           "You must only answer in the following JSON format:\n" +
                           "{\n" +
                           "\"subject\": \"<the email subject>\",\n" +
                           "\"sender\": \"<the email address that sent the email>\",\n" +
                           "\"receiver\": \"<the email address that received the email>\",\n" +
                           "\"body\": \"<the body of the email containing the information>\",\n" +
                           "}";
            string msg = "";
            msg += $"Information Location: {location}\n";
            msg += $"Information Description: {information.Description}\n";
            try
            {
                answer = await DungeonMaster.Instance.AskQuestionToGenerator(rules, msg, null);
                EmailParser parser = JsonConvert.DeserializeObject<EmailParser>(answer);
                Email email = new Email(parser, information);
                onEmailGenerated?.Invoke(email);
                return email;
            }
            catch
            {
                Debug.Log("ERROR! Could not not generate email from Information!");
                await GenerateFromInformation(information, location);
            }

            return null;
        }
    }
}