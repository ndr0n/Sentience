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
        }

        [BurstCompile]
        public partial struct UpdatePositionJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter pecb;

            void Execute(Entity entity, [ChunkIndexInQuery] int sortKey, RefRW<UpdatePositionComponent> id, RefRW<LocalTransform> tf, RefRW<LocalToWorld> ltw)
            {
                tf.ValueRW = LocalTransform.FromPosition(new Vector3(id.ValueRW.WorldPosition.x, id.ValueRW.WorldPosition.y, id.ValueRW.WorldPosition.z));
                ltw.ValueRW.Value = tf.ValueRW.ToMatrix();

                // ltw.ValueRW.Value = LocalTransform.FromPosition(id.ValueRW.WorldPosition).ToMatrix();
                // ltw.ValueRW.Value = LocalTransform.FromPosition(new Vector3(id.ValueRW.WorldPosition.x, id.ValueRW.WorldPosition.y, id.ValueRW.WorldPosition.z)).ToMatrix();
                pecb.RemoveComponent<UpdatePositionComponent>(sortKey, entity);
            }
        }
    }
}