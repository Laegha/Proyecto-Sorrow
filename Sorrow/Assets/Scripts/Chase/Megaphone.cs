using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaphone : Item
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletPerSecond;
    Transform firePoint;
    float shootCooldown;

    private void Start() => firePoint = transform.Find("FirePoint");

    public override void ItemEffect()
    {
        if (shootCooldown > 0)
            return;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        shootCooldown = 1 / bulletPerSecond; 
    }

    private void Update() => shootCooldown -= Time.deltaTime;
}
