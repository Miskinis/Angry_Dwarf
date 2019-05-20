using ECS.Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems
{
    public class VelocitySystem : ComponentSystem
    {
        private EntityQuery _velocityQuery;

        protected override void OnCreateManager()
        {
            _velocityQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<Velocity>(), ComponentType.ReadOnly<Translation>(), ComponentType.ReadWrite<PreviousPosition>()}
            });
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.deltaTime;
            Entities.With(_velocityQuery).ForEach((ref Velocity velocity, ref Translation translation, ref PreviousPosition previousPosition) =>
            {
                velocity.value         = (translation.Value - previousPosition.value) / deltaTime;
                previousPosition.value = translation.Value;
            });
        }
    }
}