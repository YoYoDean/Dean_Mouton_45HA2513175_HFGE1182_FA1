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
        for (int i = 0; i < projectiles; i++)
        {
            Vector3 dir = cam.transform.forward;
            Vector3 spreadOffset = Vector3.zero;
            spreadOffset += cam.transform.up * Random.Range(-spread, spread);
            spreadOffset += cam.transform.right * Random.Range(-spread, spread);
            dir += (dir + spreadOffset).normalized;
            
            if (Physics.Raycast(cam.transform.position, dir, out hit, maxDistance, layerMask))
            {
                if (hit.collider.GetComponent<HealthHandler>())
                {

                    hit.collider.gameObject.GetComponent<HealthHandler>().DamageHandler("Player", damage);

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
