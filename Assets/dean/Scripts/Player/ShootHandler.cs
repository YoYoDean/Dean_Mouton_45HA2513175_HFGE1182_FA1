using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class ShootHandler : MonoBehaviour
{
    [Header("ACTIONS")]
    public PlayerInputActions InputAction;
    
    [Header("PARAMETERS")]
    [SerializeField] private float maxDistance;
    [SerializeField] private float damage;
    [SerializeField] private float spread;
    [SerializeField] private int projectiles;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject muzzle;
    
    [Header("AMMO")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int totalAmmo;

    private Camera cam;
    
    [Header("ANIMATION")]
    [SerializeField] private Animator animatorHands;
    [SerializeField] private Animator animatorGun;
    
    [Header("PARTICLE")]
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private float particleDuration;
    
    
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
        
        animatorGun.SetTrigger("Shoot");
        animatorHands.SetTrigger("Shoot");
        currentAmmo--;
        RaycastHit hit;
        
        for (int i = 0; i < projectiles; i++)
        {
            Vector3 dir = muzzle.transform.right;
            Vector3 spreadOffset = Vector3.zero;
            spreadOffset += muzzle.transform.up * Random.Range(-spread, spread);
            spreadOffset += muzzle.transform.right * Random.Range(-spread, spread);
            dir += (dir + spreadOffset).normalized;
            //Debug.DrawRay(muzzle.transform.position, dir, Color.red, 3f);
            
            if (Physics.Raycast(muzzle.transform.position, dir, out hit, maxDistance, layerMask))
            {
                
                if (hit.collider.GetComponent<HealthHandler>())
                {
                    hit.collider.gameObject.GetComponent<HealthHandler>().DamageHandler("Player", damage);
                    if (particlePrefab == null)
                    {
                        return;
                    }
                    
                    GameObject hitParticle = Instantiate(particlePrefab, hit.transform.position, Quaternion.identity);
                    Destroy(hitParticle, particleDuration);
                }
            }
        }
    }
    
    private void OnReload(InputAction.CallbackContext context)
    {
        animatorGun.SetTrigger("Reload");
        animatorHands.SetTrigger("Reload");
        
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

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
    
    public int GetTotalAmmo()
    {
        return totalAmmo;
    }

    public void SetCurrentAmmo(int amount)
    {
        currentAmmo = amount;
    }
    public void SetTotalAmmo(int amount)
    {
        totalAmmo = amount;
    }
}
