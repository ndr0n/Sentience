using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
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
            Debug.Log($"RUNNING SYSTEM");
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

        /*
        // Debug.Log($"RUNNING SYSTEM");
        // EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);
        // foreach ((RefRO<ID> id, RefRW<UpdatePositionComponent> updatePosition, Entity entity) in SystemAPI.Query<RefRO<ID>, RefRW<UpdatePositionComponent>>().WithEntityAccess())
        // {
        //     if (state.EntityManager.HasComponent<Inventory>(entity))
        //     {
        //         Inventory inventory = state.EntityManager.GetComponentData<Inventory>(entity);
        //         new ProcessInventoryPositionsJob()
        //         {
        //             Ecb = ecb,
        //             Inventory = inventory,
        //             ID = id,
        //         }.ScheduleParallel();
        //     }
        // }

        [BurstCompile]
        public partial struct ProcessInventoryPositionsJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter Ecb;
            public RefRO<ID> ID;
            public Inventory Inventory;

            // IJobEntity generates a component data query based on the parameters of its `Execute` method.
            // This example queries for all Spawner components and uses `ref` to specify that the operation
            // requires read and write access. Unity processes `Execute` for each entity that matches the
            // component data query.
            private void Execute([ChunkIndexInQuery] int chunkIndex)
            {
                Ecb.RemoveComponent<UpdatePositionComponent>(chunkIndex, ID.ValueRO.Entity);
                foreach (var item in Inventory.Items)
                {
                    ID itemId = item.Item.GetData<ID>();
                    itemId.position = ID.ValueRO.Position;
                    item.Item.SetData(itemId);
                    Debug.Log($"{Inventory.Data.Name} - {item.Name} set to position: {ID.ValueRO.Position}");
                }
            }
        }

        private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            return ecb.AsParallelWriter();
        }
        */
    }
}