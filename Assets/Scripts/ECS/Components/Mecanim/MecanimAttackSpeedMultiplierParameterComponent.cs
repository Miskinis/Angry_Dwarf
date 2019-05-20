using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimAttackSpeedMultiplierParameter : IComponentData
    {
        public readonly int hashedParameter;

        public MecanimAttackSpeedMultiplierParameter(int hashedParameter)
        {
            this.hashedParameter = hashedParameter;
        }
    }

    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MecanimAttackSpeedMultiplierParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "AttackSpeedMultiplier";

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimAttackSpeedMultiplierParameter(Animator.StringToHash(parameter)));
        }
    }
}