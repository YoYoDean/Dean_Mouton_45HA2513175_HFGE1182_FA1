using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;


public class ShootHandler : MonoBehaviour
{
    [Header("ACTIONS")]
    public PlayerInputActions InputAction;

    [SerializeField] private float maxDistance;
    [SerializeField] private float damage;
    [SerializeField] private float spread;
    [SerializeField] private int projectiles;
    [SerializeField] private LayerMask layerMask;
    
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int totalAmmo;

    private Camera cam;
    
    
    private void Awake()
    {
        InputAction = new PlayerInputActions();
        cam = Camera.main;
    }

    private void OnEnable()
    {
        InputAction.Player.Enable();
        InputAction.Player.Attack.performed += OnShoot;
        InputAction.Player.Reload.performed += OnReload;
    }

    private void OnDisable()
    {
        InputAction.Player.Attack.performed -= OnShoot;
        InputAction.Player.Reload.performed -= OnReload;
        InputAction.Player.Disable();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (currentAmmo <= 0)
        {
            OnReload(new InputAction.CallbackContext());
            return;
        }

        currentAmmo--;
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        for (int i = 0; i < projectiles; i++)
{
    Vector3 dir = cam.transform.forward;

    dir += cam.transform.up * Random.Range(-spread, spread);
    dir += cam.transform.right * Random.Range(-spread, spread);
    dir.Normalize();

    Debug.DrawRay(cam.transform.position, dir * maxDistance, Color.red, 1f);

    if (Physics.Raycast(cam.transform.position, dir, out RaycastHit hitI, maxDistance, layerMask))
    {
        HealthHandler health = hitI.collider.GetComponentInParent<HealthHandler>();
        if (health != null)
        {
            health.DamageHandler("Player", damage);
            health.DamageHandler("Enemy", damage);
        }
    }
}
    }
    
    private void OnReload(InputAction.CallbackContext context)
    {
        if (totalAmmo < maxAmmo)
        {
            currentAmmo = totalAmmo;
            totalAmmo = 0;
        }
        else
        {
            totalAmmo -= maxAmmo;
            currentAmmo = maxAmmo;
        }
    }
    
    public void AddAmmo(int amount)
    {
        totalAmmo += amount;
    }
}
