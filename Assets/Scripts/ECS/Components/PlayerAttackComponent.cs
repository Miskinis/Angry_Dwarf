using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct PlayerAttack : IComponentData
    {
        public KeyCode attackKey;

        public PlayerAttack(KeyCode attackKey)
        {
            this.attackKey = attackKey;
        }
    }

    public class PlayerAttackComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public KeyCode attackKey = KeyCode.Mouse0;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new PlayerAttack(attackKey));
        }
    }
}