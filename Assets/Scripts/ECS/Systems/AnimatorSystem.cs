using ECS.Components;
using ECS.Components.Mecanim;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Systems
{
    [UpdateAfter(typeof(DeathSystem))]
    public class AnimatorSystem : ComponentSystem
    {
        private EntityQuery _isWalkingQuery;
        private EntityQuery _attackSpeedMultiplierQuery;
        private EntityQuery _switchWeaponQuery;
        private EntityQuery _attackQuery;

        
        protected override void OnCreate()
        {
            _isWalkingQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Animator>(),
                    ComponentType.ReadOnly<Velocity>(),
                    ComponentType.ReadOnly<MecanimIsWalkingParameter>()
                }
            });

            _attackSpeedMultiplierQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Animator>(),
                    ComponentType.ReadOnly<AttackSpeedMultiplier>(),
                    ComponentType.ReadOnly<MecanimAttackSpeedMultiplierParameter>()
                }
            });

            _switchWeaponQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Animator>(),
                    ComponentType.ReadOnly<MecanimSwitchWeaponParameter>(),
                    ComponentType.ReadOnly<PlayerSwitchWeapon>()
                }
            });

            _attackQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Animator>(),
                    ComponentType.ReadOnly<MecanimAttackParameter>(),
                    ComponentType.ReadOnly<PlayerAttack>()
                }
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_isWalkingQuery).ForEach((Animator animator, ref Velocity velocity, ref MecanimIsWalkingParameter mecanimIsWalkingParameter) =>
            {
                animator.SetBool(mecanimIsWalkingParameter.hashedParameter, math.any(velocity.value.xz != float2.zero));
            });

            Entities.With(_attackSpeedMultiplierQuery).ForEach(
                (Entity entity, Animator animator, ref AttackSpeedMultiplier attackSpeedMultiplier, ref MecanimAttackSpeedMultiplierParameter mecanimAttackSpeedMultiplierParameter) =>
                {
                    animator.SetFloat(mecanimAttackSpeedMultiplierParameter.hashedParameter, attackSpeedMultiplier.value);
                });

            Entities.With(_switchWeaponQuery).ForEach((Animator animator, ref MecanimSwitchWeaponParameter mecanimSwitchWeaponParameter, ref PlayerSwitchWeapon playerSwitchWeapon) =>
            {
                if (Input.GetKeyDown(playerSwitchWeapon.switchKey)) animator.SetTrigger(mecanimSwitchWeaponParameter.hashedParameter);
            });

            Entities.With(_attackQuery).ForEach((Animator animator, ref MecanimAttackParameter mecanimAttackParameter, ref PlayerAttack playerAttack) =>
            {
                if (Input.GetKeyDown(playerAttack.attackKey)) animator.SetTrigger(mecanimAttackParameter.hashedParameter);
            });
        }
    }
}