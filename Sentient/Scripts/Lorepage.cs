using System.Collections.Generic;
using UnityEngine;

namespace Sentience
{
    [CreateAssetMenu(fileName = "Lorepage", menuName = "Sentience/Lore/Lorepage", order = 1)]
    public class Lorepage : ScriptableObject
    {
        [Multiline]
        public List<string> Lore;
    }
}