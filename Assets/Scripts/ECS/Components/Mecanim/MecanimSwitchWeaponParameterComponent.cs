using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimSwitchWeaponParameter : IComponentData
    {
        public int hashedParameter;

        public MecanimSwitchWeaponParameter(int hashedParameter)
        {
            this.hashedParameter = hashedParameter;
        }
    }

    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MecanimSwitchWeaponParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "SwitchWeapon";

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimSwitchWeaponParameter(Animator.StringToHash(parameter)));
        }
    }
}