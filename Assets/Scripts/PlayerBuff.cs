using ECS;
using Unity.Entities;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{
    public GameObject pickupEffectPrefab;
    protected Entity playerEntity;
    protected EntityManager entityManager;

    private void Awake()
    {
        playerEntity  = GameObject.FindWithTag("Player").GetComponent<ConvertHierarchyToEntities>().HierarchyRootEntity;
        entityManager = World.Active.EntityManager;
    }
}