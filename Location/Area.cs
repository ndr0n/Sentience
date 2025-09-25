using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    [System.Serializable]
    public class Area
    {
        public string Name = "Loner";
        public string Description = "a colossal colony ship lost in space.";
        public List<Location> Location = new();
        public List<SentienceQuest> Quests = new();

        public Area(string name, string description)
        {
            Name = name;
            Description = description;
            Location = new();
            Quests = new();
        }

        public Area(Area area)
        {
            Name = area.Name;
            Description = area.Description;
            Location = new();
            foreach (var loc in area.Location) Location.Add(loc);
            Quests = new();
            foreach (var q in area.Quests) Quests.Add(q);
        }
    }
}