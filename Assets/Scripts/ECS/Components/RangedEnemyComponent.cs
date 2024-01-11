using ECS.Components.Mecanim;
using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct RangedEnemy : IComponentData
    {
        public float attackDistance;

        public RangedEnemy(float attackDistance)
        {
            this.attackDistance = attackDistance;
        }
    }

    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator), typeof(MecanimSwitchWeaponParameterComponent))]
    public class RangedEnemyComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float attackDistance = 10f;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            GetComponent<Animator>().SetTrigger(GetComponent<MecanimSwitchWeaponParameterComponent>().parameter);
            dstManager.AddComponentData(entity, new RangedEnemy(attackDistance));
        }
    }
}