using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace Sentience
{
    public partial struct PositionChangeSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PositionChangedComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            Debug.Log($"RUNNING SYSTEM");
            EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            foreach ((RefRW<ID> id, RefRW<PositionChangedComponent> posChange, Entity entity) in SystemAPI.Query<RefRW<ID>, RefRW<PositionChangedComponent>>().WithEntityAccess())
            {
                if (state.EntityManager.HasComponent<Inventory>(entity))
                {
                    ProcessInventoryPositions(ref state, id, posChange, entity);
                }
                ecb.RemoveComponent<PositionChangedComponent>(entity);
            }
            ecb.Playback(state.EntityManager);
        }

        public void ProcessInventoryPositions(ref SystemState state, RefRW<ID> id, RefRW<PositionChangedComponent> posChanged, Entity inv)
        {
            Inventory inventory = state.EntityManager.GetComponentData<Inventory>(inv);

            foreach (var item in inventory.Items)
            {
                ID itemId = item.Item.GetData<ID>();
                itemId.position = id.ValueRO.Position;
                item.Item.SetData(itemId);
                Debug.Log($"{inventory.Data.Name} - {item.Name} set to position: {id.ValueRO.Position}");
            }
        }
    }
}