using ECS.Components;
using Unity.Entities;
using Unity.Transforms;

namespace ECS.Systems
{
    public class VelocityBootstrapSystem : ComponentSystem
    {
        private EntityQuery _velocityQuery;

        protected override void OnCreateManager()
        {
            _velocityQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<MovementSpeed>(),
                    ComponentType.ReadOnly<Translation>()
                },
                None = new[]
                {
                    ComponentType.ReadOnly<PreviousPosition>(),
                    ComponentType.ReadOnly<Velocity>()
                }
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_velocityQuery).ForEach((Entity entity, ref Translation translation) =>
            {
                PostUpdateCommands.AddComponent(entity, new PreviousPosition(translation.Value));
                PostUpdateCommands.AddComponent(entity, new Velocity());
            });
        }
    }
}