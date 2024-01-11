using System;
using Unity.Entities;
using UnityEngine;

namespace OddLock.Components.Generic
{
    
    [DisallowMultipleComponent]
    public class EntityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public Entity entity;
        private BeginFixedStepSimulationEntityCommandBufferSystem _entityCommandBufferSystem;
        
        private bool _isEntityEnabled = true;
        public bool IsEntityEnabled
        {
            get
            {
                return _isEntityEnabled;
            }
            set
            {
                if (value)
                {
                    if(World.DefaultGameObjectInjectionWorld == null) return;
                    var entityManager = _entityCommandBufferSystem.EntityManager;
                    if (entityManager.Exists(entity) == false || entityManager.HasComponent<Disabled>(entity) == false || Time.timeScale == 0f) return;
                    entityManager.RemoveComponent<Disabled>(entity);
                }
                else
                {
                    if(World.DefaultGameObjectInjectionWorld == null) return;
                    var entityManager = _entityCommandBufferSystem.EntityManager;
                    if (entityManager.Exists(entity) == false || entityManager.HasComponent<Disabled>(entity) || Time.timeScale == 0f) return;
                    entityManager.AddComponent<Disabled>(entity);
                }
                _isEntityEnabled = value;
            }
        }
        
        private void Awake()
        {
            _entityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BeginFixedStepSimulationEntityCommandBufferSystem>();
        }

        private void Start()
        {
            IsEntityEnabled = _isEntityEnabled;
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            this.entity = entity;
        }

        private void OnDisable()
        {
            IsEntityEnabled = false;
        }

        /*private void OnDestroy()
        {
            if(World.DefaultGameObjectInjectionWorld == null) return;
            var entityManager = _entityCommandBufferSystem.EntityManager;
            if (entityManager.Exists(entity) == false || entityManager.HasComponent<Disabled>(entity) || Time.timeScale == 0f) return;
            entityManager.DestroyEntity(entity);
        }*/
    }
}