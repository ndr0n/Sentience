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
        public int PlantAmount = 3;

        public Information(string name, string description, int plantAmount)
        {
            Name = name;
            Description = description;
            PlantAmount = plantAmount;
        }

        public void GiveInformation(Journal journal)
        {
            if (!journal.Information.Contains(this)) journal.Information.Add(this);
        }
    }
}