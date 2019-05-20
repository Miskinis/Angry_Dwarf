using Unity.Entities;

namespace ECS.Components.Agent
{
    public struct AgentStoppingDistance : IComponentData
    {
        public float value;

        public AgentStoppingDistance(float value)
        {
            this.value = value;
        }
    }
}