using ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    public class PlayerBuffSystem : ComponentSystem
    {
        private EntityQuery _attackSpeedBuffQuery;
        private EntityQuery _shieldBuffQuery;

        protected override void OnCreateManager()
        {
            _attackSpeedBuffQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<AttackSpeedBuff>(), ComponentType.ReadWrite<AttackSpeedMultiplier>()}
            });

            _shieldBuffQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<ShieldBuff>()}
            });
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.deltaTime;

            Entities.With(_attackSpeedBuffQuery).ForEach((Entity entity, ref AttackSpeedBuff attackSpeedBuff, ref AttackSpeedMultiplier attackSpeedMultiplier) =>
            {
                attackSpeedMultiplier.value =  attackSpeedBuff.attackSpeed;
                attackSpeedBuff.elapsedTime += deltaTime;
                if (attackSpeedBuff.elapsedTime > attackSpeedBuff.duration)
                {
                    attackSpeedMultiplier.value = 1f;
                    PostUpdateCommands.RemoveComponent<AttackSpeedBuff>(entity);
                }
            });

            Entities.With(_shieldBuffQuery).ForEach((Entity entity, ref ShieldBuff shieldBuff) =>
            {
                shieldBuff.elapsedTime += deltaTime;
                if (shieldBuff.elapsedTime > shieldBuff.duration) PostUpdateCommands.RemoveComponent<ShieldBuff>(entity);
            });
        }
    }
}