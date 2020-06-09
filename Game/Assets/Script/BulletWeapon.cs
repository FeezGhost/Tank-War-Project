using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    
    public GameObject bulletSpawn;
    public float bulletSpeed= 100;
    public float lifeTime= 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Fire();
        }
    }
    private void Fire(){
        GameObject bullet=Instantiate(bulletPrefab);
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), bulletSpawn.GetComponentInParent<Collider>());
        bullet.transform.position= bulletSpawn.transform.position;
        Vector3 rotation =  bullet.transform.rotation.eulerAngles;
        bullet.transform.rotation= Quaternion.Euler(rotation.x,transform.eulerAngles.y, rotation.z);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.transform.forward*bulletSpeed, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAterTime(bullet,lifeTime));
    }
    private IEnumerator DestroyBulletAterTime(GameObject bullet, float delay){
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
