using Unity.Entities;

namespace ECS.Components.Agent
{
    public struct AgentUpdatePosition : IComponentData
    {
        private readonly byte _value;

        public bool Value => _value == 1;

        public AgentUpdatePosition(bool value)
        {
            _value = (byte) (value ? 1 : 0);
        }
    }
}