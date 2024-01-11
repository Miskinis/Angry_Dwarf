using System;
using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct Boss : ISharedComponentData, IEquatable<Boss>
    {
        public GameObject victoryPanel;

        public Boss(GameObject victoryPanel)
        {
            this.victoryPanel = victoryPanel;
        }

        public bool Equals(Boss other)
        {
            return Equals(victoryPanel, other.victoryPanel);
        }

        public override bool Equals(object obj)
        {
            return obj is Boss other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (victoryPanel != null ? victoryPanel.GetHashCode() : 0);
        }

        public static bool operator ==(Boss left, Boss right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Boss left, Boss right)
        {
            return !left.Equals(right);
        }
    }

    public class BossComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public GameObject victoryPanel;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddSharedComponentData(entity, new Boss(victoryPanel));
        }
    }
}