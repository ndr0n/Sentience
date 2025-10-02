using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Sentience
{
    public partial struct SetParentSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SetParentComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

            new SetParentJob() {pecb = ecb.AsParallelWriter()}.ScheduleParallel();

            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        [BurstCompile]
        public partial struct SetParentJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter pecb;

            void Execute(Entity entity, [ChunkIndexInQuery] int sortKey, RefRW<SetParentComponent> setParent, RefRW<Parent> parent)
            {
                parent.ValueRW.Value = setParent.ValueRO.Parent;
                pecb.RemoveComponent<SetParentComponent>(sortKey, entity);
                Debug.Log("Running Set Parent Component!");
            }
        }
    }
}