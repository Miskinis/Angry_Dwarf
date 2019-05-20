using ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    [UpdateBefore(typeof(DeathSystem))]
    public class HealthBarSystem : ComponentSystem
    {
        private EntityQuery _hpBarQuery;

        protected override void OnCreateManager()
        {
            _hpBarQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<PlayerHpBar>(), ComponentType.ReadOnly<Health>(), ComponentType.ReadWrite<PreviousHealth>()}
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_hpBarQuery).ForEach((PlayerHpBar playerHpBar, ref Health health, ref PreviousHealth previousHealth) =>
            {
                if (previousHealth.value < health.value)
                    for (var i = 0; i < health.value - previousHealth.value; i++)
                        Object.Instantiate(playerHpBar.heartPrefab, playerHpBar.heartsContainer);
                else if (previousHealth.value > health.value)
                    for (var i = 0; i < previousHealth.value - health.value; i++)
                        Object.Destroy(playerHpBar.heartsContainer.GetChild(playerHpBar.heartsContainer.childCount - 1).gameObject);

                previousHealth.value = health.value;
            });
        }
    }
}