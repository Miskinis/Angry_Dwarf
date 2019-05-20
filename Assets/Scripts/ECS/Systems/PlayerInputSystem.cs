using ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Systems
{
    [UpdateAfter(typeof(DeathSystem))]
    public class PlayerInputSystem : ComponentSystem
    {
        private EntityQuery _rotatePlayerGroup;
        private EntityQuery _movePlayerGroup;
        private EntityQuery _uncontrolledGravityGroup;

        protected override void OnCreateManager()
        {
            _rotatePlayerGroup = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Transform>(),
                    ComponentType.ReadOnly<RotationSpeed>(),
                    ComponentType.ReadOnly<PlayerMovementInput>(),
                    ComponentType.ReadOnly<CharacterController>()
                }
            });

            _movePlayerGroup = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<CharacterController>(),
                    ComponentType.ReadOnly<PlayerMovementInput>(),
                    ComponentType.ReadWrite<PlayerMovementControl>(),
                    ComponentType.ReadOnly<MovementSpeed>()
                }
            });

            _uncontrolledGravityGroup = GetEntityQuery(new EntityQueryDesc
            {
                All  = new[] {ComponentType.ReadWrite<CharacterController>()},
                None = new[] {ComponentType.ReadOnly<PlayerMovementInput>()}
            });
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((ref PlayerMovementInput playerInput) =>
            {
                playerInput = new PlayerMovementInput(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Input.mousePosition);
            });

            var deltaTime = Time.deltaTime;

            Entities.With(_rotatePlayerGroup).ForEach((Transform transform, ref RotationSpeed rotationSpeed, ref PlayerMovementInput playerInput) =>
            {
                var playerPlane = new Plane(Vector3.up, transform.position);

                // Generate a ray from the cursor position
                var ray = PlayerReferences.MainCamera.ScreenPointToRay(playerInput.mousePosition);

                if (playerPlane.Raycast(ray, out var distance))
                {
                    // Get the point along the ray that hits the calculated distance.
                    var targetPoint = ray.GetPoint(distance);

                    // Determine the target rotation.  This is the rotation if the transform looks at the target point.
                    var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

                    // Smoothly rotate towards the target point.
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed.value * deltaTime);
                }
            });

            Entities.With(_uncontrolledGravityGroup).ForEach((CharacterController characterController) => { characterController.Move(new Vector3(0f, -9.81f * deltaTime, 0f)); });

            Entities.With(_movePlayerGroup).ForEach((CharacterController characterController, ref PlayerMovementInput playerMovementInput,
                ref PlayerMovementControl playerMovementControl, ref MovementSpeed movementSpeed) =>
            {
                var cameraTransform = PlayerReferences.MainCamera.transform;
                var moveDirection   = float3.zero;
                var cameraForward   = cameraTransform.forward;
                cameraForward.y = 0f;
                cameraForward.Normalize();
                var cameraRight = cameraTransform.right;
                cameraRight.y = 0f;
                cameraRight.Normalize();
                var horizontal = playerMovementInput.horizontal;
                var vertical   = playerMovementInput.vertical;

                // If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
                var inputModifyFactor = horizontal != 0.0f && vertical != 0.0f && playerMovementControl.limitDiagonalSpeed ? .7071f * movementSpeed.value : movementSpeed.value;

                if (playerMovementControl.isGrounded)
                {
                    moveDirection   = (cameraForward * vertical + cameraRight * horizontal) * inputModifyFactor;
                    moveDirection.y = -playerMovementControl.antiBumpFactor;
                    //moveDirection = new float3(horizontal * inputModifyFactor, -playerMovementControl.antiBumpFactor, vertical * inputModifyFactor);
                }

                // Apply gravity
                moveDirection.y -= 9.81f * deltaTime;

                // Move the controller, and set grounded true or false depending on whether we're standing on something
                var velocity = moveDirection * deltaTime;
                playerMovementControl.isGrounded = (characterController.Move(velocity) & CollisionFlags.Below) != 0;
            });
        }
    }
}