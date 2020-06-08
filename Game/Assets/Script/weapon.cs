using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public LineOfSight checkMyVision;
    public double dis;

    public GameObject bulletPrefab;
    
    public GameObject bulletSpawn;
    public float bulletSpeed= 60;
    public float lifeTime= 30;

    // Start is called before the first frame update
    void Start()
    {
        checkMyVision=GetComponent<LineOfSight>();
        dis = checkMyVision.distance;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(checkMyVision.distance<25){
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
