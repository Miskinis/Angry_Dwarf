using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components.Agent
{
    public struct AgentDestination : IComponentData
    {
        public float3 value;

        public AgentDestination(float3 value)
        {
            this.value = value;
        }
    }
}