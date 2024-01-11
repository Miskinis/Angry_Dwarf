using ECS.Components;
using ECS.Components.Agent;
using Unity.Entities;
using UnityEngine.AI;

namespace ECS.Systems
{
    public class SetNavMeshAgentSystem : ComponentSystem
    {
        private struct IsNavMeshAgentEnabled : IComponentData
        {
        }

        private EntityQuery _getNextPositionGroup;
        private EntityQuery _getSteeringTarget;
        private EntityQuery _getHasPath;
        private EntityQuery _getPathPending;
        private EntityQuery _getVelocity;
        private EntityQuery _getRemainingDistance;
        private EntityQuery _setNextPositionGroup;
        private EntityQuery _setSpeed;
        private EntityQuery _setAngularSpeed;
        private EntityQuery _setUpdatePosition;
        private EntityQuery _setUpdateRotation;
        private EntityQuery _setAutoBreaking;
        private EntityQuery _setIsStopped;
        private EntityQuery _setStoppingDistance;
        private EntityQuery _setResetPath;
        private EntityQuery _setVelocity;
        private EntityQuery _setMove;
        private EntityQuery _setAcceleration;
        private EntityQuery _setDestination;

        private EntityQuery _enableAgent;
        private EntityQuery _disableAgent;
        private EntityQuery _destroyAgent;

        protected override void OnCreate()
        {
            _getNextPositionGroup = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadWrite<AgentGetNextPosition>()}
            });

            _getSteeringTarget = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadWrite<AgentSteeringTarget>()}
            });

            _getHasPath = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>()}
            });

            _getPathPending = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>()}
            });

            _getVelocity = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadWrite<AgentGetVelocity>()}
            });

            _getRemainingDistance = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadWrite<AgentRemainingDistance>()}
            });

            _setNextPositionGroup = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<AgentSetNextPosition>()}
            });

            _setSpeed = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<MovementSpeed>()}
            });

            _setAngularSpeed = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<RotationSpeed>()}
            });

            _setUpdatePosition = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<AgentUpdatePosition>()}
            });

            _setUpdateRotation = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<AgentUpdateRotation>()}
            });

            _setAutoBreaking = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<AgentAutoBreaking>()}
            });

            _setIsStopped = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<AgentIsStopped>()}
            });

            _setStoppingDistance = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<AgentStoppingDistance>()}
            });

            _setResetPath = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<AgentResetPath>()}
            });

            _setVelocity = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<AgentSetVelocity>()}
            });

            _setMove = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<AgentMove>()}
            });

            _setAcceleration = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<AgentAcceleration>()}
            });

            _setDestination = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<AgentDestination>()}
            });

            _enableAgent = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>()},
                None = new[]
                {
                    ComponentType.ReadWrite<IsNavMeshAgentEnabled>(),
                    ComponentType.ReadWrite<AgentUpdateRotation>(),
                    ComponentType.ReadWrite<AgentAutoBreaking>(),
                    ComponentType.ReadWrite<AgentIsStopped>(),
                    ComponentType.ReadWrite<AgentStoppingDistance>(),
                    ComponentType.ReadWrite<AgentRemainingDistance>(),
                    ComponentType.ReadWrite<AgentUpdatePosition>(),
                    ComponentType.ReadWrite<AgentGetNextPosition>(),
                    ComponentType.ReadWrite<AgentSteeringTarget>(),
                    ComponentType.ReadWrite<AgentAcceleration>()
                }
            });

            _disableAgent = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadWrite<NavMeshAgent>(), ComponentType.ReadOnly<IsNavMeshAgentEnabled>(), ComponentType.ReadOnly<AgentDisabled>()}
            });

            _destroyAgent = GetEntityQuery(new EntityQueryDesc
            {
                All  = new[] {ComponentType.ReadOnly<IsNavMeshAgentEnabled>()},
                None = new[] {ComponentType.ReadWrite<NavMeshAgent>()}
            });
        }

        protected override void OnUpdate()
        {
            //Get NavMeshAgent.pathPending
            Entities.With(_getPathPending).ForEach((Entity entity, NavMeshAgent navMeshAgent) =>
            {
                if (navMeshAgent.pathPending && EntityManager.HasComponent<AgentPathPending>(entity) == false)
                    PostUpdateCommands.AddComponent(entity, new AgentPathPending());
                else if (navMeshAgent.pathPending == false && EntityManager.HasComponent<AgentPathPending>(entity)) PostUpdateCommands.RemoveComponent<AgentPathPending>(entity);
            });

            //Get NavMeshAgent.nextPosition
            Entities.With(_getNextPositionGroup).ForEach((NavMeshAgent navMeshAgent, ref AgentGetNextPosition agentGetNextPosition) =>
            {
                agentGetNextPosition.value = navMeshAgent.nextPosition;
            });

            //Get NavMeshAgent.HasPath
            Entities.With(_getHasPath).ForEach((Entity entity, NavMeshAgent navMeshAgent) =>
            {
                if (navMeshAgent.hasPath && EntityManager.HasComponent<AgentHasPath>(entity) == false)
                {
                    PostUpdateCommands.AddComponent(entity, new AgentHasPath());
                }
                else if (navMeshAgent.hasPath == false && EntityManager.HasComponent<AgentHasPath>(entity))
                {
                    navMeshAgent.ResetPath();
                    PostUpdateCommands.RemoveComponent<AgentHasPath>(entity);
                }
            });

            //Get NavMeshAgent.velocity
            Entities.With(_getVelocity).ForEach((NavMeshAgent navMeshAgent, ref AgentGetVelocity velocity) => { velocity.Value = navMeshAgent.velocity; });

            //Get NavMeshAgent SteeringTarget
            Entities.With(_getSteeringTarget).ForEach((NavMeshAgent navMeshAgent, ref AgentSteeringTarget agentSteeringTarget) =>
            {
                agentSteeringTarget.value = navMeshAgent.steeringTarget;
            });

            //Get NavMeshAgent.remainingDistance
            Entities.With(_getRemainingDistance).ForEach((NavMeshAgent navMeshAgent, ref AgentRemainingDistance agentRemainingDistance) =>
            {
                agentRemainingDistance.value = navMeshAgent.remainingDistance;
            });

            //Set NavMeshAgent.speed
            Entities.With(_setSpeed).ForEach((NavMeshAgent navMeshAgent, ref MovementSpeed moveSpeed) => { navMeshAgent.speed = moveSpeed.value; });

            //Set NavMeshAgent.angularSpeed
            Entities.With(_setAngularSpeed).ForEach((NavMeshAgent navMeshAgent, ref RotationSpeed rotationSpeed) => { navMeshAgent.angularSpeed = rotationSpeed.value; });

            //Set NavMeshAgent.updatePosition
            Entities.With(_setUpdatePosition).ForEach((NavMeshAgent navMeshAgent, ref AgentUpdatePosition agentUpdatePosition) =>
            {
                navMeshAgent.updatePosition = agentUpdatePosition.Value;
            });

            //Set NavMeshAgent.updateRotation
            Entities.With(_setUpdateRotation).ForEach((NavMeshAgent navMeshAgent, ref AgentUpdateRotation agentUpdateRotation) =>
            {
                navMeshAgent.updateRotation = agentUpdateRotation.Value;
            });

            //Set NavMeshAgent.autoBreaking
            Entities.With(_setAutoBreaking).ForEach((NavMeshAgent navMeshAgent, ref AgentAutoBreaking agentAutoBreaking) => { navMeshAgent.autoBraking = agentAutoBreaking.Value; });

            //Set NavMeshAgent.isStopped
            Entities.With(_setIsStopped).ForEach((NavMeshAgent navMeshAgent, ref AgentIsStopped agentIsStopped) => { navMeshAgent.isStopped = agentIsStopped.Value; });

            //Set NavMeshAgent.stoppingDistance
            Entities.With(_setStoppingDistance).ForEach((NavMeshAgent navMeshAgent, ref AgentStoppingDistance agentStoppingDistance) =>
            {
                navMeshAgent.stoppingDistance = agentStoppingDistance.value;
            });

            //Set NavMeshAgent.ResetPath()
            Entities.With(_setResetPath).ForEach((Entity entity, NavMeshAgent navMeshAgent) =>
            {
                navMeshAgent.ResetPath();
                PostUpdateCommands.RemoveComponent<AgentResetPath>(entity);
            });

            //Set NavMeshAgent.velocity
            Entities.With(_setVelocity).ForEach((NavMeshAgent navMeshAgent, ref AgentSetVelocity agentSetVelocity) => { navMeshAgent.velocity = agentSetVelocity.value; });

            //Set NavMeshAgent.nextPosition
            Entities.With(_setNextPositionGroup).ForEach((Entity entity, NavMeshAgent navMeshAgent, ref AgentSetNextPosition agentSetNextPosition) =>
            {
                navMeshAgent.nextPosition = agentSetNextPosition.value;
                PostUpdateCommands.RemoveComponent<AgentSetNextPosition>(entity);
            });

            //Set NavMeshAgent.Move()
            Entities.With(_setMove).ForEach((Entity entity, NavMeshAgent navMeshAgent, ref AgentMove agentMove) =>
            {
                navMeshAgent.Move(agentMove.value);
                PostUpdateCommands.RemoveComponent<AgentMove>(entity);
            });

            //Set NavMeshAgent.acceleration
            Entities.With(_setAcceleration).ForEach((NavMeshAgent navMeshAgent, ref AgentAcceleration agentAcceleration) => { navMeshAgent.acceleration = agentAcceleration.value; });

            //Set NavMeshAgent.destination
            Entities.With(_setDestination).ForEach((Entity entity, NavMeshAgent navMeshAgent, ref AgentDestination agentDestination) =>
            {
                navMeshAgent.destination = agentDestination.value;
                PostUpdateCommands.RemoveComponent<AgentDestination>(entity);
            });

            //Agent Disable
            Entities.With(_disableAgent).ForEach((Entity entity, NavMeshAgent navMeshAgent) =>
            {
                PostUpdateCommands.RemoveComponent<IsNavMeshAgentEnabled>(entity);
                PostUpdateCommands.RemoveComponent<AgentUpdateRotation>(entity);
                PostUpdateCommands.RemoveComponent<AgentAutoBreaking>(entity);
                PostUpdateCommands.RemoveComponent<AgentIsStopped>(entity);
                PostUpdateCommands.RemoveComponent<AgentStoppingDistance>(entity);
                PostUpdateCommands.RemoveComponent<AgentRemainingDistance>(entity);
                PostUpdateCommands.RemoveComponent<AgentUpdatePosition>(entity);
                PostUpdateCommands.RemoveComponent<AgentGetNextPosition>(entity);
                PostUpdateCommands.RemoveComponent<AgentSteeringTarget>(entity);
                PostUpdateCommands.RemoveComponent<AgentAcceleration>(entity);

                if (EntityManager.HasComponent<AgentPathPending>(entity)) PostUpdateCommands.RemoveComponent<AgentPathPending>(entity);

                if (EntityManager.HasComponent<AgentHasPath>(entity)) PostUpdateCommands.RemoveComponent<AgentHasPath>(entity);

                navMeshAgent.enabled = false;
            });

            //Agent Destroyed
            Entities.With(_destroyAgent).ForEach(entity =>
            {
                PostUpdateCommands.RemoveComponent<IsNavMeshAgentEnabled>(entity);
                PostUpdateCommands.RemoveComponent<AgentUpdateRotation>(entity);
                PostUpdateCommands.RemoveComponent<AgentAutoBreaking>(entity);
                PostUpdateCommands.RemoveComponent<AgentIsStopped>(entity);
                PostUpdateCommands.RemoveComponent<AgentStoppingDistance>(entity);
                PostUpdateCommands.RemoveComponent<AgentRemainingDistance>(entity);
                PostUpdateCommands.RemoveComponent<AgentUpdatePosition>(entity);
                PostUpdateCommands.RemoveComponent<AgentGetNextPosition>(entity);
                PostUpdateCommands.RemoveComponent<AgentSteeringTarget>(entity);
                PostUpdateCommands.RemoveComponent<AgentAcceleration>(entity);

                if (EntityManager.HasComponent<AgentPathPending>(entity)) PostUpdateCommands.RemoveComponent<AgentPathPending>(entity);

                if (EntityManager.HasComponent<AgentHasPath>(entity)) PostUpdateCommands.RemoveComponent<AgentHasPath>(entity);
            });


            //Agent Enabled
            Entities.With(_enableAgent).ForEach((Entity entity, NavMeshAgent navMeshAgent) =>
            {
                if (navMeshAgent.enabled)
                {
                    PostUpdateCommands.AddComponent(entity, new IsNavMeshAgentEnabled());
                    PostUpdateCommands.AddComponent(entity, new AgentUpdateRotation(navMeshAgent.updateRotation));
                    PostUpdateCommands.AddComponent(entity, new AgentAutoBreaking(navMeshAgent.autoBraking));
                    PostUpdateCommands.AddComponent(entity, new AgentIsStopped(navMeshAgent.isStopped));
                    PostUpdateCommands.AddComponent(entity, new AgentStoppingDistance(navMeshAgent.stoppingDistance));
                    PostUpdateCommands.AddComponent(entity, new AgentRemainingDistance(navMeshAgent.remainingDistance));
                    PostUpdateCommands.AddComponent(entity, new AgentUpdatePosition(navMeshAgent.updatePosition));
                    PostUpdateCommands.AddComponent(entity, new AgentGetNextPosition(navMeshAgent.nextPosition));
                    PostUpdateCommands.AddComponent(entity, new AgentSteeringTarget(navMeshAgent.steeringTarget));
                    PostUpdateCommands.AddComponent(entity, new AgentAcceleration(navMeshAgent.acceleration));

                    if (EntityManager.HasComponent<AgentDisabled>(entity)) PostUpdateCommands.RemoveComponent<AgentDisabled>(entity);
                }
            });
        }
    }
}