using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI ammo;
    public TextMeshProUGUI health;
    public int iAmmo;
    public float fHealth;
    public UI instance;
    // private HealthHandler healthHandler;
    //private ShootHandler shootHandler;

    void Awake()
    {
        instance = this;
        iAmmo = GameObject.FindGameObjectWithTag("Player").GetComponent<ShootHandler>().currentAmmo;
        fHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthHandler>().health;
    }

    void Update()
    {
        iAmmo = GameObject.FindGameObjectWithTag("Player").GetComponent<ShootHandler>().currentAmmo;
        fHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthHandler>().health;
        ammo.text = "Ammo: " + iAmmo.ToString();
        health.text = "Health: " +  fHealth.ToString();
    }

}
