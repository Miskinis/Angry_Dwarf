using System;
using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct EnemyDeathDrop : ISharedComponentData, IEquatable<EnemyDeathDrop>
    {
        public GameObject[] buffPrefabs;
        public float dropChance;


        public EnemyDeathDrop(GameObject[] buffPrefabs, float dropChance)
        {
            this.buffPrefabs = buffPrefabs;
            this.dropChance  = dropChance;
        }

        public bool Equals(EnemyDeathDrop other)
        {
            return Equals(buffPrefabs, other.buffPrefabs) && dropChance.Equals(other.dropChance);
        }

        public override bool Equals(object obj)
        {
            return obj is EnemyDeathDrop other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(buffPrefabs, dropChance);
        }

        public static bool operator ==(EnemyDeathDrop left, EnemyDeathDrop right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EnemyDeathDrop left, EnemyDeathDrop right)
        {
            return !left.Equals(right);
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