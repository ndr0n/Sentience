using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bitLoner;
using UnityEngine;
using Avatar = bitLoner.Avatar;
using Random = UnityEngine.Random;

namespace Sentience
{
    [System.Serializable]
    public class Persona : EntityComponent
    {
        public string Description = "";
        public string Desire;
        Sentient sentient;
        Identity identity;

        public override void OnInit(EntityData data, System.Random random)
        {
            base.OnInit(data, random);
            sentient = data.Get<Sentient>();
            identity = data.Get<Identity>();
            if (identity != null && sentient != null && string.IsNullOrWhiteSpace(sentient.Personality)) _ = GeneratePersona();
        }

        public async Task GeneratePersona()
        {
            try
            {
                Avatar avatar = Data.Get<Avatar>();
                if (avatar != null)
                {
                    bool hasClothing = false;
                    foreach (var slot in avatar.Slot)
                    {
                        if (slot != null)
                        {
                            if (hasClothing == false)
                            {
                                Description = ".\nYou are wearing ";
                                hasClothing = true;
                            }
                            else Description += ", ";

                            Description += slot.Description;
                        }
                    }

                    if (hasClothing) Description += ".";
                }

                SentienceCharacter character = await SentienceCharacter.GenerateSentienceCharacter(Description, identity.Location);
                await identity.LoadSentienceCharacter(character, identity.Faction, new System.Random(Random.Range(int.MinValue, int.MaxValue)));
                sentient.InitIdentity(identity);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void RefreshDesire(List<EntityData> entities)
        {
            if (string.IsNullOrWhiteSpace(Desire))
            {
                List<EntityData> items = new();
                foreach (var entity in entities)
                {
                    if (entity == Data) continue;
                    if (entity.Has<Inventory>())
                    {
                        Inventory inv = entity.Get<Inventory>();
                        foreach (var slot in inv.Items) items.Add(slot.Item);
                    }
                }

                if (items.Count > 0) Desire = items[Random.Range(0, items.Count)].Name;
            }
        }
    }

    [System.Serializable]
    public class PersonaAuthoring : EntityAuthoring
    {
        public string Description = "";
        public string Desire = "";

        public override IEntityComponent Spawn(System.Random random)
        {
            Persona persona = new();
            persona.Description = Description;
            persona.Desire = Desire;
            return persona;
        }
    }
}