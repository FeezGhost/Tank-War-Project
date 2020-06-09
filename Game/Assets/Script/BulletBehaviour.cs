using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BulletBehaviour : MonoBehaviour
{
    
    public Health playerhealth = null;
    public GameObject Enemy;
    public Text score;
   private void OnTriggerEnter(Collider other){
       print("hit" + other.name +"!" );
       string name="hit" + other.name +"!";
       if(other.CompareTag("Enemy")||other.CompareTag("Player")){
           playerhealth=other.GetComponent<Health>();
           playerhealth.healthpoints-=10;
           if(other.CompareTag("Enemy")){
               Enemy=GameObject.FindGameObjectWithTag("Enemy");
            Text mText = score.GetComponent<Text>();
             mText.text="100"+10;
           }
       }
       Destroy(gameObject);
   }
}
