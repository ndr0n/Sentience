using Newtonsoft.Json;

namespace Sentience
{
    public static class ItemLibrary
    {
        public static EntityType GenerateItem(string name, EntityType similarItemType)
        {
            EntityType i = UnityEngine.Object.Instantiate(similarItemType);
            i.name = name;
            i.Name = name;
            return i;
        }

        public static string SerializeItem(EntityType itemType)
        {
            string type = $"{itemType.GetType()}|";
            string serialized = type;
            serialized += JsonConvert.SerializeObject(itemType, Formatting.None);
            return serialized;
        }

        public static EntityType Deserialize(string serializedItem)
        {
            string type = serializedItem.Split('|')[0];

            if (type == typeof(EntityType).ToString())
            {
                EntityType itemType = JsonConvert.DeserializeObject<EntityType>(serializedItem.Split('|')[1]);
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