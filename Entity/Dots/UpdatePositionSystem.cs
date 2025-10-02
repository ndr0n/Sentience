using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

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
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            new UpdatePositionJob() {pecb = ecb.AsParallelWriter()}.ScheduleParallel();
            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            foreach ((RefRW<ItemComponent> item, RefRW<LocalTransform> tf) in SystemAPI.Query<RefRW<ItemComponent>, RefRW<LocalTransform>>())
            {
                LocalTransform ptf = state.EntityManager.GetComponentData<LocalTransform>(item.ValueRO.Parent);
                tf.ValueRW.Position = ptf.Position;
            }
        }

        [BurstCompile]
        public partial struct UpdatePositionJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter pecb;

            void Execute(Entity entity, [ChunkIndexInQuery] int sortKey, RefRW<UpdatePositionComponent> id, RefRW<LocalTransform> tf)
            {
                tf.ValueRW = LocalTransform.FromPosition(id.ValueRW.WorldPosition);
                pecb.RemoveComponent<UpdatePositionComponent>(sortKey, entity);
                // Debug.Log($"UPDATEPOSITION: {entity.Index} - Position: {tf.ValueRW.Position}");
            }
        }

        [BurstCompile]
        public partial struct UpdateItemPositionJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter pecb;

            void Execute(Entity entity, [ChunkIndexInQuery] int sortKey, RefRW<ItemComponent> item, RefRW<LocalTransform> tf, DynamicBuffer<Child> children)
            {
                // LocalTransform parentTransform = parent.ValueRO

                tf.ValueRW.Position = tf.ValueRO.Position;
                // tf.ValueRW.Value = item.ValueRO.ParentTransform.Value;
                pecb.RemoveComponent<UpdatePositionComponent>(sortKey, entity);
                // Debug.Log($"ITEM: {entity.Index} - Position: {tf.ValueRW.Position}");
            }
        }
    }
}