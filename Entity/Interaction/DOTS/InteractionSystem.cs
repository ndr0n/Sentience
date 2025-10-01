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
            Interaction inter = (Interaction) entry.MainAsset;
            Debug.Log($"Has Interaction Component!!");
            Debug.Log($"NAME: {inter.Name}");
        }
    }
}