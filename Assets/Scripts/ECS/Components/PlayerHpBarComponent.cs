using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct PlayerHpBar : ISharedComponentData
    {
        public Transform heartsContainer;
        public GameObject heartPrefab;

        public PlayerHpBar(Transform heartsContainer, GameObject heartPrefab)
        {
            this.heartsContainer = heartsContainer;
            this.heartPrefab     = heartPrefab;
        }
    }

    [RequireComponent(typeof(HealthComponent))]
    public class PlayerHpBarComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public Transform heartsContainer;
        public GameObject heartPrefab;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddSharedComponentData(entity, new PlayerHpBar(heartsContainer, heartPrefab));
            var health = GetComponent<HealthComponent>().health;
            for (var i = 0; i < health; i++) Instantiate(heartPrefab, heartsContainer);
        }
    }
}