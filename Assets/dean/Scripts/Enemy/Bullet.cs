using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    
    [SerializeField] private float damage;
    [SerializeField] private float lifeTime;
    [SerializeField] private float bulletForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<HealthHandler>().DamageHandler(this.gameObject.tag, damage);
            Debug.Log(col.gameObject.tag);
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<HealthHandler>().DamageHandler(this.gameObject.tag, damage);
            Debug.Log(col.gameObject.tag);
        }
    }
}
