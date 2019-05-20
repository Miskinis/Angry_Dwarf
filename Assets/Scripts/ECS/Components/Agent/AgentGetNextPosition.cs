using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components.Agent
{
    public struct AgentGetNextPosition : IComponentData
    {
        public float3 value;

        public AgentGetNextPosition(float3 value)
        {
            this.value = value;
        }
    }
}