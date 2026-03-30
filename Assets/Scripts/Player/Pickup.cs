using System;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField]protected int value;
    [SerializeField] protected float timeBeforeSpawn;
    [SerializeField] protected Collider pickupTrigger;
    [SerializeField] protected GameObject pickupVisual;
    [SerializeField] protected float timer = 0f;

    protected virtual void Update()
    {
        if (!pickupTrigger.enabled && !pickupVisual.activeInHierarchy)
        {
            timer += Time.deltaTime;
        }

        if (timer >= timeBeforeSpawn)
        {
            timer = 0f;
            pickupVisual.SetActive(true);
            pickupTrigger.enabled = true;
        }
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            ApplyEffect(col.gameObject);
            DeactivatePickup();
        }
    }

    protected virtual void ApplyEffect(GameObject player) { }

    protected virtual void DeactivatePickup()
    {
        pickupVisual.SetActive(false);
        pickupTrigger.enabled = false;
    }
}
