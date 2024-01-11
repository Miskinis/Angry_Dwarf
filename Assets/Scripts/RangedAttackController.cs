using MecanimBehaviors;
using OddLock.Components.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class RangedAttackController : MonoBehaviour
{
    public Transform gunExhaustPoint;
    private EntityAuthoring _rootEntityObject;
    public Animator animator;
    public float maxHitDistance = 100f;
    public ushort damage;
    public GameObject bulletPrefab;

    private GunShotBehavior[] _gunShotBehaviors;

    private Entity _entity;
    private BeginFixedStepSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    private void Start()
    {
        _rootEntityObject          = GetComponent<EntityAuthoring>();
        _entity                    = _rootEntityObject.entity;
        _entityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BeginFixedStepSimulationEntityCommandBufferSystem>();

        _gunShotBehaviors = animator.GetBehaviours<GunShotBehavior>();

        var entityManager = _entityCommandBufferSystem.EntityManager;
        foreach (var attackBehavior in _gunShotBehaviors)
            attackBehavior.onFrameAction = () =>
            {
                if (entityManager == null || entityManager.Exists(_entity) == false)
                {
                    Destroy(this);
                    return;
                }

                Instantiate(bulletPrefab, gunExhaustPoint.position, gunExhaustPoint.rotation, _rootEntityObject.transform);
            };
    }

    private void OnDestroy()
    {
        foreach (var attackBehavior in _gunShotBehaviors) attackBehavior.onFrameAction = null;
    }
}