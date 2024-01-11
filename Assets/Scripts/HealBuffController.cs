using ECS.Components;
using UnityEngine;

public class HealBuffController : PlayerBuff
{
    [Tooltip("Set health to this number")] public ushort heal = 5;

    private void OnTriggerEnter(Collider other)
    {
        var entityManager = _entityCommandBufferSystem.EntityManager;
        if (other.CompareTag("Player") && entityManager.GetComponentData<Health>(playerEntity).value < heal)
        {
            _entityCommandBufferSystem.CreateCommandBuffer().SetComponent(playerEntity, new Health(heal));
            Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}