using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    public GameObject bulletPrefab;
    public Transform BulletSpawnPoint;
    public float bulletSpeed = 5f;
    [SerializeField] Camera cam;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public override void Use()
    {
      
       Shoot();

    }
    void Shoot()
    {
       
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f,0.5f));
        ray.origin=cam.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit))
           hit.collider.gameObject.GetComponent<IDamageable>() ?.TakeDamage(((GunInfo)itemInfo).damage);
           PV.RPC("RPC_Shoot", RpcTarget.All, hit.point,hit.normal);
       
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length != 0)
        {
            float tiempodesa = 0.0001f;
         
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * tiempodesa, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 5f);
           
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
        var bullet = Instantiate(bulletPrefab, BulletSpawnPoint.position, BulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = BulletSpawnPoint.forward * bulletSpeed;
        Destroy(bullet, 1f);
    }



}
