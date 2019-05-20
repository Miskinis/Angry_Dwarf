using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components.Agent
{
    public struct AgentMove : IComponentData
    {
        public float3 value;

        public AgentMove(float3 value)
        {
            this.value = value;
        }
    }
}