using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct EnemyDeathDrop : ISharedComponentData
    {
        public GameObject[] buffPrefabs;
        public float dropChance;


        public EnemyDeathDrop(GameObject[] buffPrefabs, float dropChance)
        {
            this.buffPrefabs = buffPrefabs;
            this.dropChance  = dropChance;
        }
    }

    public class EnemyDeathDropComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public GameObject[] buffPrefabs;
        [Range(1, 100)] public float dropChance;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddSharedComponentData(entity, new EnemyDeathDrop(buffPrefabs, dropChance));
        }
    }
}