using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components
{
    public struct PreviousPosition : IComponentData
    {
        public float3 value;

        public PreviousPosition(float3 value)
        {
            this.value = value;
        }
    }
}