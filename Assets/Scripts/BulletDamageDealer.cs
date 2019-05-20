using System.Collections;
using ECS;
using ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

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
    private EntityManager _entityManager;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _entity                = GetComponentInParent<ConvertHierarchyToEntities>().HierarchyRootEntity;
        _entityManager         = World.Active.EntityManager;
        _rigidbody             = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = false;
        transform.parent = null;
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

        if (_entityManager == null || _entityManager.Exists(_entity) == false)
        {
            Destroy(this);
            return;
        }

        var otherEntityObject = other.GetComponent<ConvertHierarchyToEntities>();
        if (otherEntityObject == null || other.GetComponent<HealthComponent>() == null) return;

        var otherEntity = otherEntityObject.HierarchyRootEntity;

        _entityManager.AddComponentData(otherEntity, new DealDamage(damage));
        Instantiate(hitEffect, other.transform.position, Quaternion.identity);
    }
}