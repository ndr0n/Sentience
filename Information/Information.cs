using System.Threading.Tasks;
using Sentience;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    public class Information
    {
        public string Name = "";
        [Multiline] public string Description = "";

        public Information(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void GiveInformation(Journal journal)
        {
            if (!journal.Information.Contains(this)) journal.Information.Add(this);
        }
    }
}