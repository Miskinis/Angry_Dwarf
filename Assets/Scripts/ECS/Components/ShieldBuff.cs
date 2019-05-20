using Unity.Entities;

namespace ECS.Components
{
    public struct ShieldBuff : IComponentData
    {
        public float duration;
        public float elapsedTime;

        public ShieldBuff(float duration)
        {
            this.duration = duration;
            elapsedTime   = 0f;
        }
    }
}