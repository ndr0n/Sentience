using System.Collections.Generic;
using MindTheatre;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public abstract class IdentityInteraction : ScriptableObject
    {
        public string Name = "Interaction";
        public string Description = "";
        public abstract bool IsPossible(Identity self);

        public bool TryInteract(Identity self, Identity interactor)
        {
            if (OnTryInteract(self, interactor))
            {
                CheckForQuestInteraction(self, interactor);
                return true;
            }
            return false;
        }

        protected abstract bool OnTryInteract(Identity self, Identity interactor);

        protected void CheckForQuestInteraction(Identity self, Identity interactor)
        {
            if (interactor.Data is PlayerData pd)
            {
                List<Quest> toRemove = new();
                foreach (var q in pd.Journal.Quests)
                {
                    if (q.QuestData.Stages[q.Stage].InteractionData.Target == self.Data)
                    {
                        if (q.QuestData.Stages[q.Stage].InteractionData.TargetInteraction == this)
                        {
                            Debug.Log($"INTERACTING WITH QUEST TARGET!");
                            q.Stage += 1;
                            if (q.Stage >= q.QuestData.Stages.Count) toRemove.Add(q);
                        }
                    }
                }
                foreach (var q in toRemove) pd.Journal.Quests.Remove(q);
            }
        }
    }
}