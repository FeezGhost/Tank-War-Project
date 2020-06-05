using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
   private void OnTriggerEnter(Collider other){
       print("hit" + other.name +"!" );
       string name="hit" + other.name +"!";
       if(other.CompareTag("Enemy")){
           Destroy(other);
           Destroy(other.gameObject);
        //    Destroy(other.)
       }
       Destroy(gameObject);
   }
}
