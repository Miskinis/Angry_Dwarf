using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components.Agent
{
    public struct AgentGetVelocity : IComponentData
    {
        public float3 Value;

        public AgentGetVelocity(float3 Value)
        {
            this.Value = Value;
        }
    }
}