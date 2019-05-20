using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components.Agent
{
    public struct AgentSteeringTarget : IComponentData
    {
        public float3 value;

        public AgentSteeringTarget(float3 value)
        {
            this.value = value;
        }
    }
}