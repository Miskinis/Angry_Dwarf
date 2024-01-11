using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct EnemyVisionRange : IComponentData
    {
        public float value;

        public EnemyVisionRange(float value)
        {
            this.value = value;
        }
    }

    
    [DisallowMultipleComponent]
    public class EnemyVisionRangeComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float enemyVisionRange = 10f;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new EnemyVisionRange(enemyVisionRange));
        }
    }
}