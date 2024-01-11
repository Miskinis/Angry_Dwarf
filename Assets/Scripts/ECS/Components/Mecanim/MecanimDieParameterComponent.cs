using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimDieParameter : IComponentData
    {
        public readonly int hashedParameter;

        public MecanimDieParameter(int hashedParameter)
        {
            this.hashedParameter = hashedParameter;
        }
    }

    
    [DisallowMultipleComponent]
    public class MecanimDieParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "Die";

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimDieParameter(Animator.StringToHash(parameter)));
        }
    }
}