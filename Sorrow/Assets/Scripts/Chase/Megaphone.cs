using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaphone : Item
{
    Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletForce;

    [SerializeField] float bulletPerSecond;
    float shootCooldown;

    private void Start()
    {
        firePoint = transform.Find("FirePoint");
    }

    public override void ItemEffect()
    {
        if (shootCooldown > 0)
            return;
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>().AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        shootCooldown = 1 / bulletPerSecond; 
    }
    private void Update()
    {
        if (shootCooldown > 0)
            shootCooldown -= Time.deltaTime;
    }
}
