using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class HealthHandler : MonoBehaviour
{
    [SerializeField] private string[] damageTags;
    [SerializeField] public float health;
    [SerializeField] private float healthMax;
   /// private UI uI;
    
    [SerializeField] private bool destroyOnDeath;
    [SerializeField] private float destroyOnDeathDelay;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = healthMax;
       // uI.instance.UpdateHealth(health);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            DeathHandler();
        }
    }

    public void DamageHandler(string damageTag, float damageAmount)
    {
        if (damageTags.Contains(damageTag))
        {
            if (damageAmount <= health)
            {
                health -= damageAmount;
                //uI.instance.UpdateHealth(health);
            }
            else
            {
                health = 0;
            }
            
        }
    }
    

    public void HealHandler(float healAmount)
    {
        if (health + healAmount >= healthMax)
        {
            health = healthMax;
           //uI.instance.UpdateHealth(health);
        }
        else
        {
            health += healAmount;
           // uI.instance.UpdateHealth(health);
        }
    }

    public void DeathHandler()
    {
        if (destroyOnDeath)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            
        }
    }

}
