using ECS;
using MecanimBehaviors;
using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class RangedAttackController : MonoBehaviour
{
    public Transform gunExhaustPoint;
    public ConvertHierarchyToEntities rootEntityObject;
    public Animator animator;
    public float maxHitDistance = 100f;
    public ushort damage;
    public GameObject bulletPrefab;

    private GunShotBehavior[] _gunShotBehaviors;

    private Entity _entity;
    private EntityManager _entityManager;

    private void Awake()
    {
        _entity        = rootEntityObject.HierarchyRootEntity;
        _entityManager = World.Active.EntityManager;

        _gunShotBehaviors = animator.GetBehaviours<GunShotBehavior>();
        foreach (var attackBehavior in _gunShotBehaviors)
            attackBehavior.onFrameAction = () =>
            {
                if (_entityManager == null || _entityManager.Exists(_entity) == false)
                {
                    Destroy(this);
                    return;
                }

                Instantiate(bulletPrefab, gunExhaustPoint.position, gunExhaustPoint.rotation, rootEntityObject.transform);
            };
    }

    private void OnDestroy()
    {
        foreach (var attackBehavior in _gunShotBehaviors) attackBehavior.onFrameAction = null;
    }
}