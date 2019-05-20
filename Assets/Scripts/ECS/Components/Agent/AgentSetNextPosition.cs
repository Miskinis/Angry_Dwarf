using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components.Agent
{
    public struct AgentSetNextPosition : IComponentData
    {
        public readonly float3 value;

        public AgentSetNextPosition(float3 value)
        {
            this.value = value;
        }
    }
}