using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems
{
    [UpdateInGroup(typeof(TransformSystemGroup))]
    [UpdateAfter(typeof(CopyTransformFromGameObjectSystem))]
    [UpdateBefore(typeof(TRSToLocalToWorldSystem))]
    public partial class CustomCopyTransformFromGameObjectSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;
        private EntityQuery _localToWorldGroup;

        protected override void OnCreate()
        {
            _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _entityCommandBufferSystem.CreateCommandBuffer();
            
            Entities.WithBurst().WithAll<CopyTransformFromGameObject>().ForEach((Entity entity) =>
            {
                ecb.AddComponent(entity, new LocalToWorld {Value = float4x4.identity});
            }).Run();
            
            Entities.WithBurst().WithAll<CopyTransformFromGameObject>().ForEach((ref Translation translation, in LocalToWorld transformMatrix) =>
            {
                translation.Value = transformMatrix.Position;
            }).Run();
            
            Entities.WithBurst().WithAll<CopyTransformFromGameObject>().ForEach((ref Rotation rotation, in LocalToWorld transformMatrix) =>
            {
                rotation.Value = quaternion.LookRotation(transformMatrix.Forward, transformMatrix.Up);
            }).Run();
        }
    }
}