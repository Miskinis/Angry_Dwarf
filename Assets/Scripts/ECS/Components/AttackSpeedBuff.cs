using Unity.Entities;

namespace ECS.Components
{
    public struct AttackSpeedBuff : IComponentData
    {
        public float duration;
        public float attackSpeed;
        public float elapsedTime;

        public AttackSpeedBuff(float duration, float attackSpeed)
        {
            this.duration    = duration;
            this.attackSpeed = attackSpeed;
            elapsedTime      = 0f;
        }
    }
}