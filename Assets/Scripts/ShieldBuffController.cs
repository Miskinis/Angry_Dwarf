using ECS.Components;
using UnityEngine;

public class ShieldBuffController : PlayerBuff
{
    public float effectDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && entityManager.HasComponent<ShieldBuff>(playerEntity) == false)
        {
            entityManager.AddComponentData(playerEntity, new ShieldBuff(effectDuration));
            Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}