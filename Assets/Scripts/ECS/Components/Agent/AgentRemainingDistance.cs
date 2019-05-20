using Unity.Entities;

namespace ECS.Components.Agent
{
    public struct AgentRemainingDistance : IComponentData
    {
        public float value;

        public AgentRemainingDistance(float value)
        {
            this.value = value;
        }
    }
}