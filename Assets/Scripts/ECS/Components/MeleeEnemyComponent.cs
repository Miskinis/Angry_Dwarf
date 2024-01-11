using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct MeleeEnemy : IComponentData
    {
        public float attackDistance;

        public MeleeEnemy(float attackDistance)
        {
            this.attackDistance = attackDistance;
        }
    }

    
    [DisallowMultipleComponent]
    public class MeleeEnemyComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float attackDistance = 2f;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MeleeEnemy(attackDistance));
        }
    }
}