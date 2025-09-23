using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    [CreateAssetMenu(fileName = "Lorebook", menuName = "Sentience/Lore/Lorebook", order = 1)]
    public class Lorebook : ScriptableObject
    {
        public List<Lorepage> Page;

        public string GetLore()
        {
            string lore = "";
            foreach (var page in Page)
            {
                foreach (var l in page.Lore)
                {
                    lore += $"{l} \n";
                }
            }
            return lore;
        }
    }
}