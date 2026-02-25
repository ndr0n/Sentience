using System;
using System.Collections.Generic;
using System.Linq;
using bitLoner;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sentience
{
    [System.Serializable]
    public class Persona : EntityComponent
    {
        public string Description = "";
        public string Desire;
        public QuestData DesireQuest;
        Sentient sentient;
        Identity identity;

        public override void OnInit(EntityData data, System.Random random)
        {
            base.OnInit(data, random);
            sentient = data.Get<Sentient>();
            identity = data.Get<Identity>();
            if (identity != null && sentient != null && string.IsNullOrWhiteSpace(sentient.Personality)) _ = GeneratePersona();
        }

        public async Awaitable GeneratePersona()
        {
            try
            {
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
                List<(EntityData owner, EntityData item)> items = new();
                foreach (var entity in entities)
                {
                    if (entity == Data) continue;
                    if (entity.Has<Inventory>())
                    {
                        Inventory inv = entity.Get<Inventory>();
                        foreach (var slot in inv.Items) items.Add((entity, slot.Item));
                    }
                }

                (EntityData owner, EntityData item) desire = items[Random.Range(0, items.Count)];
                Desire = desire.item.Name;

                DesireQuest = new();
                DesireQuest.Name = $"{Data.Name}'s desire";
                DesireQuest.Location = identity.Location;
                DesireQuest.Stages = new();
                QuestStage retrieveStage = new()
                {
                    Description = $"{Data.Name} desires {desire.item.Name} from {desire.owner.Name}",
                    Objective = $"Find {desire.item.Name}",
                    InteractionData = new InteractionData(desire.item.Name, desire.owner.Name, Persistent.Instance.Data.RetrieveInteraction)
                };
                DesireQuest.Stages.Add(retrieveStage);
                QuestStage giveStage = new()
                {
                    Description = $"{Data.Name} desires {desire.item.Name} from {desire.owner.Name}",
                    Objective = $"Give {desire.item.Name} to {Data.Name}",
                    InteractionData = new InteractionData(desire.item.Name, Data.Name, Persistent.Instance.Data.RetrieveInteraction)
                };
                DesireQuest.Stages.Add(giveStage);
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