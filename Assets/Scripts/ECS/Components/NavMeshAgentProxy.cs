using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace ECS.Components
{
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshAgentProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentObject(entity, GetComponent<NavMeshAgent>());
        }
    }
}