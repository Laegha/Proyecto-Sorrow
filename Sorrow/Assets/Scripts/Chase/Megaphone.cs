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

        Quaternion rotation = Quaternion.identity;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
            rotation = Quaternion.LookRotation((hit.point - firePoint.position).normalized);
        
        Instantiate(bulletPrefab, firePoint.position, rotation != Quaternion.identity ? rotation : firePoint.rotation);
        
        shootCooldown = 1 / bulletPerSecond; 
    }

    private void Update() => shootCooldown -= Time.deltaTime;
}
