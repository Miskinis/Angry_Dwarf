using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Components
{
    public struct PlayerMovementInput : IComponentData
    {
        public readonly float horizontal;
        public readonly float vertical;
        public float3 mousePosition;

        public PlayerMovementInput(float horizontal, float vertical, float3 mousePosition)
        {
            this.horizontal    = horizontal;
            this.vertical      = vertical;
            this.mousePosition = mousePosition;
        }
    }

    
    [DisallowMultipleComponent]
    public class PlayerMovementInputComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new PlayerMovementInput());
        }
    }
}