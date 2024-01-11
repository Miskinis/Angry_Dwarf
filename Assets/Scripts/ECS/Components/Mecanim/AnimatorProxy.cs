using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class AnimatorProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentObject(entity, GetComponent<Animator>());
        }
    }
}