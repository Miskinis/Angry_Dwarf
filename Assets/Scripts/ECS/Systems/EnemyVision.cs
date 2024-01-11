using ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems
{
    public class EnemyVision : ComponentSystem
    {
        private EntityQuery _playerQuery;
        private EntityQuery _enemyQuery;
        private EntityQuery _cleanupQuery;

        protected override void OnCreate()
        {
            _cleanupQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadOnly<CurrentTargetData>()}
            });

            _playerQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<PlayerMovementInput>(),
                    ComponentType.ReadOnly<Translation>()
                }
            });

            _enemyQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<EnemyVisionRange>(),
                    ComponentType.ReadOnly<Translation>()
                }
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_cleanupQuery).ForEach(entity => { PostUpdateCommands.RemoveComponent<CurrentTargetData>(entity); });

            Entities.With(_playerQuery).ForEach((ref Translation playerTranslation) =>
            {
                var playerPosition = playerTranslation.Value;
                Entities.With(_enemyQuery).ForEach((Entity entity, ref Translation enemyTranslation, ref EnemyVisionRange enemyVisionRange) =>
                {
                    var distance = math.distance(playerPosition, enemyTranslation.Value);
                    if (distance < enemyVisionRange.value)
                    {
#if UNITY_EDITOR
                        Debug.DrawLine(enemyTranslation.Value, playerPosition, Color.green);
#endif
                        PostUpdateCommands.AddComponent(entity, new CurrentTargetData(distance, playerPosition));
                    }
                });
            });
        }
    }
}