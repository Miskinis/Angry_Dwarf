using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct RotationSpeed : IComponentData
    {
        public float value;

        public RotationSpeed(float value)
        {
            this.value = value;
        }
    }

    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class RotationSpeedComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Tooltip("Rotation per second in degrees")]
        public float rotationSpeed = 80f;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new RotationSpeed(rotationSpeed));
        }
    }
}