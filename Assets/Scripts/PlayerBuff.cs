using System;
using ECS;
using OddLock.Components.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{
    public GameObject pickupEffectPrefab;
    protected Entity playerEntity;
    protected BeginFixedStepSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    private void Start()
    {
        playerEntity               = GameObject.FindWithTag("Player").GetComponent<EntityAuthoring>().entity;
        _entityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BeginFixedStepSimulationEntityCommandBufferSystem>();
    }
}