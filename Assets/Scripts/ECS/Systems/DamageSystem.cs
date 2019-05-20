using ECS.Components;
using Unity.Entities;

namespace ECS.Systems
{
    public class DamageSystem : ComponentSystem
    {
        private EntityQuery _dealDamageQuery;

        protected override void OnCreateManager()
        {
            _dealDamageQuery = GetEntityQuery(new EntityQueryDesc
            {
                All  = new[] {ComponentType.ReadOnly<DealDamage>(), ComponentType.ReadWrite<Health>()},
                None = new[] {ComponentType.ReadWrite<ShieldBuff>()}
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_dealDamageQuery).ForEach((Entity entity, ref DealDamage dealDamage, ref Health health) =>
            {
                health.value -= dealDamage.damage;
                PostUpdateCommands.RemoveComponent<DealDamage>(entity);
            });
        }
    }
}