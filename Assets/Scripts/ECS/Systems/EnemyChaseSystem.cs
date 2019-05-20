using ECS.Components;
using ECS.Components.Agent;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    public class EnemyChaseSystem : ComponentSystem
    {
        private EntityQuery _enemyChaseQuery;

        protected override void OnCreateManager()
        {
            _enemyChaseQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<CurrentTargetData>(),
                    ComponentType.ReadOnly<AgentStoppingDistance>(),
                    ComponentType.ReadWrite<Transform>()
                }
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_enemyChaseQuery).ForEach((Entity entity, Transform transform, ref CurrentTargetData currentTargetData, ref AgentStoppingDistance agentStoppingDistance) =>
            {
                PostUpdateCommands.AddComponent(entity, new AgentDestination(currentTargetData.position));
                transform.LookAt(currentTargetData.position);
            });
        }
    }
}