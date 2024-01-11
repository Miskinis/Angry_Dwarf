using System;
using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct PlayerHpBar : ISharedComponentData, IEquatable<PlayerHpBar>
    {
        public Transform heartsContainer;
        public GameObject heartPrefab;

        public PlayerHpBar(Transform heartsContainer, GameObject heartPrefab)
        {
            this.heartsContainer = heartsContainer;
            this.heartPrefab     = heartPrefab;
        }

        public bool Equals(PlayerHpBar other)
        {
            return Equals(heartsContainer, other.heartsContainer) && Equals(heartPrefab, other.heartPrefab);
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerHpBar other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(heartsContainer, heartPrefab);
        }

        public static bool operator ==(PlayerHpBar left, PlayerHpBar right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PlayerHpBar left, PlayerHpBar right)
        {
            return !left.Equals(right);
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