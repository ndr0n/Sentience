using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sentience
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ItemData", menuName = "Sentience/Item/ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        public List<EntityType> Items = new();

        public EntityType GetItem(string itemName)
        {
            return Items.FirstOrDefault(x => x.Name == itemName);
        }
    }
}