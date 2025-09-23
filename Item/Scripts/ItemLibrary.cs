using Newtonsoft.Json;

namespace Sentience
{
    public static class ItemLibrary
    {
        public static Item GenerateItem(string name, Item similarItem)
        {
            Item i = UnityEngine.Object.Instantiate(similarItem);
            i.name = name;
            i.Name = name;
            return i;
        }

        public static string SerializeItem(Item item)
        {
            string type = $"{item.GetType()}|";
            string serialized = type;
            serialized += JsonConvert.SerializeObject(item, Formatting.None);
            return serialized;
        }

        public static Item Deserialize(string serializedItem)
        {
            string type = serializedItem.Split('|')[0];

            if (type == typeof(Item).ToString())
            {
                Item item = JsonConvert.DeserializeObject<Item>(serializedItem.Split('|')[1]);
                return item;
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