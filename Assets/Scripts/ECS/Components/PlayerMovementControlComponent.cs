using ECS.Components.Mecanim;
using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct PlayerMovementControl : IComponentData
    {
        public bool isGrounded;
        public readonly float antiBumpFactor;
        public readonly bool limitDiagonalSpeed;

        public PlayerMovementControl(float antiBumpFactor, bool limitDiagonalSpeed) : this()
        {
            this.antiBumpFactor     = antiBumpFactor;
            this.limitDiagonalSpeed = limitDiagonalSpeed;
        }
    }

    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterControllerProxy), typeof(PlayerMovementInputComponent), typeof(RotationSpeedComponent))]
    [RequireComponent(typeof(MovementSpeedComponent), typeof(CustomCopyTransformFromGameObject), typeof(MecanimIsWalkingParameterComponent))]
    [RequireComponent(typeof(AnimatorProxy))]
    public class PlayerMovementControlComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Tooltip("Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast")]
        public float antiBumpFactor = 0.75f;

        [Tooltip("If true, diagonal speed (when strafing + moving forward or back) can't exceed normal move speed; otherwise it's about 1.4 times faster")]
        public bool limitDiagonalSpeed = true;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new PlayerMovementControl(antiBumpFactor, limitDiagonalSpeed));
        }
    }
}