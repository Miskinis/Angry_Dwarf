using ECS.Components;
using UnityEngine;

public class AttackSpeedBuffController : PlayerBuff
{
    [Tooltip("Set attack speed multiplier to this number")]
    public float attackSpeedMultiplier = 1.5f;

    public float effectDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && entityManager.HasComponent<AttackSpeedBuff>(playerEntity) == false)
        {
            entityManager.AddComponentData(playerEntity, new AttackSpeedBuff(effectDuration, attackSpeedMultiplier));
            Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}