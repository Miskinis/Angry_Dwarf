using Unity.Entities;

namespace ECS.Components.Agent
{
    public struct AgentAcceleration : IComponentData
    {
        public float value;

        public AgentAcceleration(float value)
        {
            this.value = value;
        }
    }
}