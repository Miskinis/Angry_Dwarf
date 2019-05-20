using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentObject(entity, GetComponent<CharacterController>());
        }
    }
}