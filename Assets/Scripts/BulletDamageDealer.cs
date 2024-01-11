using System.Collections;
using ECS;
using ECS.Components;
using OddLock.Components.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using EntityAuthoring = OddLock.Components.Generic.EntityAuthoring;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BulletDamageDealer : MonoBehaviour
{
    public ushort damage = 1;
    public float shotForce = 600f;
    public float selfDestructDelay = 5f;
    public GameObject shotEffect;
    public GameObject hitEffect;

    private Entity _entity;
    private BeginFixedStepSimulationEntityCommandBufferSystem _entityCommandBufferSystem;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _entity                    = GetComponentInParent<EntityAuthoring>().entity;
        _entityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BeginFixedStepSimulationEntityCommandBufferSystem>();
        _rigidbody                 = GetComponent<Rigidbody>();
        _rigidbody.isKinematic     = false;
        transform.parent           = null;
        _rigidbody.AddForce(transform.forward * shotForce);
        Instantiate(shotEffect, transform.position, quaternion.identity);
    }

    private IEnumerator SelfDestruct(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(SelfDestruct(selfDestructDelay));

        var entityManager = _entityCommandBufferSystem.EntityManager;
        if (entityManager == null || entityManager.Exists(_entity) == false)
        {
            Destroy(this);
            return;
        }

        var otherEntityObject = other.GetComponent<EntityAuthoring>();
        if (otherEntityObject == null || other.GetComponent<HealthComponent>() == null) return;

        var otherEntity = otherEntityObject.entity;

        _entityCommandBufferSystem.CreateCommandBuffer().AddComponent(otherEntity, new DealDamage(damage));
        Instantiate(hitEffect, other.transform.position, Quaternion.identity);
    }
}