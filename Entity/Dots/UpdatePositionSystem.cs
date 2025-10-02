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

            // Debug.Log("UpdatePositionSystem - Update");
        }

        [BurstCompile]
        public partial struct UpdatePositionJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter pecb;

            void Execute(Entity entity, [ChunkIndexInQuery] int sortKey, RefRW<UpdatePositionComponent> id, RefRW<LocalTransform> tf)
            {
                tf.ValueRW = LocalTransform.FromPosition(id.ValueRW.WorldPosition);
                pecb.RemoveComponent<UpdatePositionComponent>(sortKey, entity);
            }
        }
    }
}