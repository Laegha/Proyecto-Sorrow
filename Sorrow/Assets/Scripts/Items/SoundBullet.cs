using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBullet : MonoBehaviour
{
    [SerializeField] float bulletForce;
    [SerializeField] int bounces;
    [SerializeField] float time;
    Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    void OnEnable() => rb.AddForce(transform.forward * bulletForce, ForceMode.VelocityChange);

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
            Destroy(gameObject);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (bounces is 0 || collision.transform.CompareTag("BulletReactable"))
            Destroy(gameObject);

        
        // Reflect the bullet's rotation with the normal of the surface it collided with.
        transform.rotation = Quaternion.LookRotation(Vector3.Reflect(transform.forward, collision.contacts[0].normal));
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * bulletForce, ForceMode.VelocityChange);
    }
}

