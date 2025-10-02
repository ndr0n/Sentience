using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Sentience
{
    public partial struct UpdatePositionSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<UpdatePositionComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new ProcessUpdatePositionJob().ScheduleParallel();

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            foreach ((RefRW<UpdatePositionComponent> updatePosition, Entity entity) in SystemAPI.Query<RefRW<UpdatePositionComponent>>().WithEntityAccess())
            {
                ecb.RemoveComponent<UpdatePositionComponent>(entity);
            }

            /*
            // Debug.Log($"RUNNING SYSTEM");
            EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            foreach ((RefRW<ID> id, RefRW<UpdatePositionComponent> updatePosition) in SystemAPI.Query<RefRW<ID>, RefRW<UpdatePositionComponent>>())
            {
                if (state.EntityManager.HasComponent<Inventory>(id.ValueRO.Entity))
                {
                    ProcessInventoryPositions(ref state, id, id.ValueRO.Entity);
                }
                ecb.RemoveComponent<UpdatePositionComponent>(id.ValueRO.Entity);
            }
            ecb.Playback(state.EntityManager);
            */
        }

        public void ProcessInventoryPositions(ref SystemState state, RefRW<ID> id, Entity inv)
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

    public partial struct ProcessUpdatePositionJob : IJobEntity
    {
        void Execute(UpdatePositionComponent updatePos, ID id)
        {
            EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
            if (em.HasComponent<Inventory>(id.Entity))
            {
                Inventory inventory = em.GetComponentObject<Inventory>(id.Entity);
                foreach (var item in inventory.Items)
                {
                    ID itemId = item.Item.GetData<ID>();
                    itemId.position = id.Position;
                    item.Item.SetData(itemId);
                    Debug.Log($"{inventory.Data.Name} - {item.Name} set to position: {id.Position}");
                }
            }
        }
    }
}