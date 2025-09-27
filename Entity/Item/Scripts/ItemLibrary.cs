using Newtonsoft.Json;

namespace Sentience
{
    public static class ItemLibrary
    {
        public static ItemType GenerateItem(string name, ItemType similarItemType)
        {
            ItemType i = UnityEngine.Object.Instantiate(similarItemType);
            i.name = name;
            i.Name = name;
            return i;
        }

        public static string SerializeItem(ItemType itemType)
        {
            string type = $"{itemType.GetType()}|";
            string serialized = type;
            serialized += JsonConvert.SerializeObject(itemType, Formatting.None);
            return serialized;
        }

        public static ItemType Deserialize(string serializedItem)
        {
            string type = serializedItem.Split('|')[0];

            if (type == typeof(ItemType).ToString())
            {
                ItemType itemType = JsonConvert.DeserializeObject<ItemType>(serializedItem.Split('|')[1]);
                return itemType;
            }
            // if (type == typeof(Scrap).ToString())
            // {
            //     Scrap scrap = JsonConvert.DeserializeObject<Scrap>(serializedItem.Split('|')[1]);
            //     return scrap;
            // }
            // if (type == typeof(Consumable).ToString())
            // {
            //     Consumable consumable = JsonConvert.DeserializeObject<Consumable>(serializedItem.Split('|')[1]);
            //     return consumable;
            // }
            // if (type == typeof(Weapon).ToString())
            // {
            //     Weapon weapon = JsonConvert.DeserializeObject<Weapon>(serializedItem.Split('|')[1]);
            //     return weapon;
            // }
            // if (type == typeof(Armor).ToString())
            // {
            //     Armor armor = JsonConvert.DeserializeObject<Armor>(serializedItem.Split('|')[1]);
            //     return armor;
            // }
            return null;
        }
    }
}