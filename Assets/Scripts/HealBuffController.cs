using ECS.Components;
using UnityEngine;

public class HealBuffController : PlayerBuff
{
    [Tooltip("Set health to this number")] public ushort heal = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && entityManager.GetComponentData<Health>(playerEntity).value < heal)
        {
            entityManager.SetComponentData(playerEntity, new Health(heal));
            Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}