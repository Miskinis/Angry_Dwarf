using Unity.Entities;

namespace ECS.Components.Agent
{
    public struct AgentUpdateRotation : IComponentData
    {
        private byte _value;

        public bool Value
        {
            get => _value == 1;
            set => _value = (byte) (value ? 1 : 0);
        }

        public AgentUpdateRotation(bool value)
        {
            _value = (byte) (value ? 1 : 0);
        }
    }
}