using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEngine;
using World = MindTheatre.World;

namespace Sentience
{
    [BurstCompile]
    public partial struct InteractionSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            foreach (RefRW<InteractionComponent> interactionComponent in SystemAPI.Query<RefRW<InteractionComponent>>())
            {
                ProcessInteraction(ref state, interactionComponent);
                ecb.RemoveComponent<InteractionComponent>(interactionComponent.ValueRO.Self);
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        void ProcessInteraction(ref SystemState state, RefRW<InteractionComponent> comp)
        {
            var entry = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(comp.ValueRO.InteractionPath.Value));
            Interaction interaction = (Interaction) entry.MainAsset;
            Debug.Log($"Has Interaction Component!!");
            interaction.Interact(ref state, comp);
            CheckQuests(ref state, comp, interaction);
            Debug.Log($"Interacted! {interaction.Name}");
        }

        void CheckQuests(ref SystemState state, RefRW<InteractionComponent> comp, Interaction interaction)
        {
            if (state.EntityManager.HasComponent<Journal>(comp.ValueRO.Interactor))
            {
                Journal journal = state.EntityManager.GetComponentObject<Journal>(comp.ValueRO.Interactor);
                List<Quest> toRemove = new();
                foreach (var q in journal.Quests)
                {
                    QuestStage stage = q.Data.Stages[q.Stage];
                    if (stage.InteractionData.Interaction == interaction)
                    {
                        ID selfId = state.EntityManager.GetComponentObject<ID>(comp.ValueRO.Self);
                        if (!string.IsNullOrWhiteSpace(stage.InteractionData.Item))
                        {
                            if (stage.InteractionData.Item == selfId.Name)
                            {
                                ID targetId = state.EntityManager.GetComponentObject<ID>(comp.ValueRO.Target);
                                if (stage.InteractionData.Target == targetId.Name)
                                {
                                    q.Stage += 1;
                                    if (q.Stage >= q.Data.Stages.Count) toRemove.Add(q);
                                    else q.OnAdvanceStage?.Invoke(q);
                                    Debug.Log($"Advanced Quest Stage!");
                                }
                            }
                        }
                        else if (stage.InteractionData.Target == selfId.Name)
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
    }
}