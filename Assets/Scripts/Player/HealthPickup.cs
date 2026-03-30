using UnityEngine;

public class HealthPickup : Pickup
{
    protected override void ApplyEffect(GameObject player)
    {
        player.GetComponent<HealthHandler>().HealHandler(value);
        Debug.Log("Heal");
    }
}
