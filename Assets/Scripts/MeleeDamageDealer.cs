using ECS.Components;
using OddLock.Components.Generic;
using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class MeleeDamageDealer : MonoBehaviour
{
    public OddLock.Components.Generic.EntityAuthoring rootEntityObject;
    public ushort damage = 1;

    private Entity _entity;
    private BeginFixedStepSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    private void Start()
    {
        _entity                    = rootEntityObject.entity;
        _entityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BeginFixedStepSimulationEntityCommandBufferSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var entityManager = _entityCommandBufferSystem.EntityManager;
        if (entityManager == null || entityManager.Exists(_entity) == false)
        {
            Destroy(this);
            return;
        }

        var otherEntityObject = other.GetComponent<EntityAuthoring>();
        if (otherEntityObject == null || other.GetComponent<HealthComponent>() == null) return;

        var otherEntity = otherEntityObject.entity;

        entityManager.AddComponentData(otherEntity, new DealDamage(damage));
    }
}