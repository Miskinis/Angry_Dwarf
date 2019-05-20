using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems
{
    [ExecuteAlways]
    [UpdateInGroup(typeof(TransformSystemGroup))]
    [UpdateAfter(typeof(CopyTransformFromGameObjectSystem))]
    [UpdateBefore(typeof(EndFrameTRSToLocalToWorldSystem))]
    public class CustomCopyTransformFromGameObjectSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;
        private EntityQuery _localToWorldGroup;

        protected override void OnCreateManager()
        {
            _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            _localToWorldGroup = GetEntityQuery(new EntityQueryDesc
            {
                All  = new[] {ComponentType.ReadOnly<CopyTransformFromGameObject>()},
                Any  = new[] {ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<Rotation>()},
                None = new[] {ComponentType.ReadOnly<LocalToWorld>()}
            });
        }

        private struct LocalToWorldJob : IJobChunk
        {
            [WriteOnly] public EntityCommandBuffer.Concurrent commandBuffer;
            [ReadOnly] public ArchetypeChunkEntityType entityType;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var instanceCount = chunk.Count;
                var entities      = chunk.GetNativeArray(entityType);
                for (var i = 0; i < instanceCount; i++) commandBuffer.AddComponent(chunkIndex, entities[i], new LocalToWorld {Value = float4x4.identity});
            }
        }

        [BurstCompile]
        [RequireComponentTag(typeof(CopyTransformFromGameObject))]
        private struct TranslationJob : IJobForEach<LocalToWorld, Translation>
        {
            public void Execute([ReadOnly] ref LocalToWorld transformMatrix, [WriteOnly] ref Translation translation)
            {
                translation.Value = transformMatrix.Position;
            }
        }

        [BurstCompile]
        [RequireComponentTag(typeof(CopyTransformFromGameObject))]
        private struct RotationJob : IJobForEach<LocalToWorld, Rotation>
        {
            public void Execute([ReadOnly] ref LocalToWorld transformMatrix, [WriteOnly] ref Rotation rotation)
            {
                rotation.Value = quaternion.LookRotation(transformMatrix.Forward, transformMatrix.Up);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var localToWorldJob = new LocalToWorldJob
            {
                commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                entityType    = GetArchetypeChunkEntityType()
            }.Schedule(_localToWorldGroup);
            var translationJob = new TranslationJob().Schedule(this, inputDeps);
            var rotationJob    = new RotationJob().Schedule(this, inputDeps);
            _entityCommandBufferSystem.AddJobHandleForProducer(localToWorldJob);
            return JobHandle.CombineDependencies(localToWorldJob, translationJob, rotationJob);
        }
    }
}