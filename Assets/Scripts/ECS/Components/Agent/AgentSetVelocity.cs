using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components.Agent
{
    public struct AgentSetVelocity : IComponentData
    {
        public readonly float3 value;

        public AgentSetVelocity(float3 value)
        {
            this.value = value;
        }
    }
}