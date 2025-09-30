using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MindTheatre;
using Unity.Entities;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sentience
{
    public abstract class Interaction : ScriptableObject
    {
        public string Name = "Interaction";
        public string Description = "";
        public string Tags = "";

        public abstract bool HasInteraction(Entity self, Entity interactor, Entity target);

        public bool TryInteract(Entity self, Entity interactor, Entity target)
        {
            if (!HasInteraction(self, interactor, target)) return false;
            if (OnTryInteract(self, interactor, target))
            {
                CheckForQuestInteraction(self, interactor, target);
                return true;
            }
            return false;
        }

        protected abstract bool OnTryInteract(Entity self, Entity interactor, Entity target);

        protected void CheckForQuestInteraction(Entity self, Entity interactor, Entity target)
        {
            if (EntityLibrary.Has<Journal>(interactor))
            {
                Journal journal = EntityLibrary.Get<Journal>(interactor);
                List<Quest> toRemove = new();
                foreach (var q in journal.Quests)
                {
                    QuestStage stage = q.Data.Stages[q.Stage];
                    if (stage.InteractionData.Interaction == this)
                    {
                        Info selfInfo = EntityLibrary.Get<Info>(self);
                        if (!string.IsNullOrWhiteSpace(stage.InteractionData.Item))
                        {
                            if (stage.InteractionData.Item == selfInfo.Name)
                            {
                                Info targetInfo = EntityLibrary.Get<Info>(target);
                                if (stage.InteractionData.Target == targetInfo.Name)
                                {
                                    q.Stage += 1;
                                    if (q.Stage >= q.Data.Stages.Count) toRemove.Add(q);
                                    else q.OnAdvanceStage?.Invoke(q);
                                    Debug.Log($"Advanced Quest Stage!");
                                }
                            }
                        }
                        else if (stage.InteractionData.Target == selfInfo.Name)
                        {
                            q.Stage += 1;
                            if (q.Stage >= q.Data.Stages.Count) toRemove.Add(q);
                            else q.OnAdvanceStage?.Invoke(q);
                            Debug.Log($"Advanced Quest Stage!");
                        }
                    }
                }
                foreach (var q in toRemove) journal.Quests.Remove(q);
            }
        }

        public bool IsWithinRange(Body self, Body interactor, Vector2 range)
        {
            float distance = Vector3.Distance(self.Spawn.transform.position, interactor.Spawn.transform.position);
            if (distance >= range.x && distance <= (range.y + 0.5f)) return true;
            return false;
        }
    }
}