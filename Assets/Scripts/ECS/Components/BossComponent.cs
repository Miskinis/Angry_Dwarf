using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct Boss : ISharedComponentData
    {
        public GameObject victoryPanel;

        public Boss(GameObject victoryPanel)
        {
            this.victoryPanel = victoryPanel;
        }
    }

    public class BossComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public GameObject victoryPanel;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddSharedComponentData(entity, new Boss(victoryPanel));
        }
    }
}