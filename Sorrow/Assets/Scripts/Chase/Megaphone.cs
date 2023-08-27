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
        Transform bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).transform;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
            bullet.rotation = Quaternion.Euler((hit.point - firePoint.transform.position).normalized);

        print("rotacion fr: " + Quaternion.Euler((hit.point - firePoint.transform.position).normalized));
        print("rotacion firepoint: " + firePoint.rotation);
        shootCooldown = 1 / bulletPerSecond; 
    }

    private void Update() => shootCooldown -= Time.deltaTime;
}
