using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components
{
    public struct CurrentTargetData : IComponentData
    {
        public float distance;
        public float3 position;

        public CurrentTargetData(float distance, float3 position)
        {
            this.distance = distance;
            this.position = position;
        }
    }
}