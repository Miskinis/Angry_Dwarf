using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct PlayerSwitchWeapon : IComponentData
    {
        public KeyCode switchKey;

        public PlayerSwitchWeapon(KeyCode switchKey)
        {
            this.switchKey = switchKey;
        }
    }

    
    [DisallowMultipleComponent]
    public class PlayerSwitchWeaponComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public KeyCode switchKey = KeyCode.Q;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new PlayerSwitchWeapon(switchKey));
        }
    }
}