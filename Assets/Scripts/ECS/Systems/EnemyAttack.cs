using ECS.Components;
using ECS.Components.Agent;
using ECS.Components.Mecanim;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    public class EnemyAttack : ComponentSystem
    {
        private EntityQuery _meleeEnemyAttackQuery;
        private EntityQuery _rangedEnemyAttackQuery;

        protected override void OnCreate()
        {
            _meleeEnemyAttackQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<MeleeEnemy>(),
                    ComponentType.ReadOnly<CurrentTargetData>(),
                    ComponentType.ReadOnly<MecanimAttackParameter>(),
                    ComponentType.ReadWrite<Animator>(),
                    ComponentType.ReadWrite<AgentStoppingDistance>()
                }
            });

            _rangedEnemyAttackQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<RangedEnemy>(),
                    ComponentType.ReadOnly<CurrentTargetData>(),
                    ComponentType.ReadOnly<MecanimAttackParameter>(),
                    ComponentType.ReadWrite<Animator>(),
                    ComponentType.ReadWrite<AgentStoppingDistance>()
                }
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_meleeEnemyAttackQuery).ForEach(
                (Animator animator, ref MeleeEnemy meleeEnemy, ref MecanimAttackParameter mecanimAttackParameter, ref CurrentTargetData currentTargetData,
                    ref AgentStoppingDistance agentStoppingDistance) =>
                {
                    agentStoppingDistance.value = meleeEnemy.attackDistance;
                    if (currentTargetData.distance < meleeEnemy.attackDistance) animator.SetTrigger(mecanimAttackParameter.hashedParameter);
                });

            Entities.With(_rangedEnemyAttackQuery).ForEach(
                (Animator animator, ref RangedEnemy rangedEnemy, ref MecanimAttackParameter mecanimAttackParameter, ref CurrentTargetData currentTargetData,
                    ref AgentStoppingDistance agentStoppingDistance) =>
                {
                    agentStoppingDistance.value = rangedEnemy.attackDistance;
                    if (currentTargetData.distance < rangedEnemy.attackDistance) animator.SetTrigger(mecanimAttackParameter.hashedParameter);
                });
        }
    }
}