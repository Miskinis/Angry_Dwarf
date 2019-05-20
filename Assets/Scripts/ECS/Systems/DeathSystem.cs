using ECS.Components;
using ECS.Components.Mecanim;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems
{
    public class DeathSystem : ComponentSystem
    {
        private EntityQuery _dieQuery;
        private EntityQuery _playerDieQuery;
        private EntityQuery _enemyDieQuery;
        private EntityQuery _bossDieQuery;

        protected override void OnCreateManager()
        {
            _dieQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Animator>(),
                    ComponentType.ReadOnly<Health>(),
                    ComponentType.ReadOnly<MecanimDieParameter>()
                }
            });

            _playerDieQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<Dead>(),
                    ComponentType.ReadOnly<PlayerMovementInput>()
                }
            });

            _enemyDieQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<Dead>(),
                    ComponentType.ReadOnly<EnemyDeathDrop>(),
                    ComponentType.ReadOnly<Translation>()
                }
            });

            _bossDieQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<Dead>(),
                    ComponentType.ReadOnly<Boss>()
                }
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_dieQuery).ForEach((Entity entity, Animator animator, ref Health health, ref MecanimDieParameter mecanimDieParameter) =>
            {
                if (health.value == 0)
                {
                    animator.SetTrigger(mecanimDieParameter.hashedParameter);
                    PostUpdateCommands.AddComponent(entity, new Dead());
                }
            });

            Entities.With(_playerDieQuery).ForEach(entity =>
            {
                UiController.main.gameOverPanel.SetActive(true);
                PostUpdateCommands.DestroyEntity(entity);
            });

            Entities.With(_enemyDieQuery).ForEach((Entity entity, EnemyDeathDrop enemyDeathDrop, ref Translation translation) =>
            {
                if (Random.Range(0, 100) < enemyDeathDrop.dropChance)
                    Object.Instantiate(enemyDeathDrop.buffPrefabs[Random.Range(0, enemyDeathDrop.buffPrefabs.Length - 1)], translation.Value, Quaternion.identity);
                PostUpdateCommands.DestroyEntity(entity);
            });

            Entities.With(_bossDieQuery).ForEach((Entity entity, Boss boss) =>
            {
                boss.victoryPanel.SetActive(true);
                PostUpdateCommands.DestroyEntity(entity);
            });
        }
    }
}