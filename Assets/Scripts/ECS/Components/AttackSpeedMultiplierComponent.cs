using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct AttackSpeedMultiplier : IComponentData
    {
        public float value;

        public AttackSpeedMultiplier(float value)
        {
            this.value = value;
        }
    }

    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class AttackSpeedMultiplierComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float attackSpeedMultiplier;


        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new AttackSpeedMultiplier(attackSpeedMultiplier));
        }
    }
}