using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MindTheatre;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public abstract class Interaction : ScriptableObject
    {
        public string Name = "Interaction";
        public string Description = "";

        public abstract bool HasInteraction(EntityData self, EntityData interactor, EntityData target);

        public bool TryInteract(EntityData self, EntityData interactor, EntityData target)
        {
            if (!HasInteraction(self, interactor, target)) return false;
            if (OnTryInteract(self, interactor, target))
            {
                CheckForQuestInteraction(self, interactor);
                return true;
            }
            return false;
        }

        protected abstract bool OnTryInteract(EntityData self, EntityData interactor, EntityData target);

        protected void CheckForQuestInteraction(EntityData self, EntityData interactor)
        {
            if (interactor is PlayerData pd)
            {
                List<Quest> toRemove = new();
                foreach (var q in pd.Journal.Quests)
                {
                    if (q.QuestData.Stages[q.Stage].InteractionData.Interaction == this)
                    {
                        // Is Quest Target
                        if (q.QuestData.Stages[q.Stage].InteractionData.Target == self.Name)
                        {
                            q.Stage += 1;
                            if (q.Stage >= q.QuestData.Stages.Count) toRemove.Add(q);
                        }
                    }
                }
                foreach (var q in toRemove) pd.Journal.Quests.Remove(q);
            }
        }

        public bool IsWithinRange(IdentityData self, IdentityData interactor, Vector2 range)
        {
            float distance = Vector3.Distance(self.Spawn.transform.position, interactor.Spawn.transform.position);
            if (distance >= range.x && distance <= (range.y + 0.5f)) return true;
            return false;
        }
    }
}