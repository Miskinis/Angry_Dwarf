using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct MovementSpeed : IComponentData
    {
        public float value;

        public MovementSpeed(float value)
        {
            this.value = value;
        }
    }

    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MovementSpeedComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float movementSpeed = 5f;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MovementSpeed(movementSpeed));
        }
    }
}