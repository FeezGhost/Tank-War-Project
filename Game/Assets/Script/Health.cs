using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject parent;
    public bool check1;
    public bool check2;
   
  [SerializeField]public float healthpoints=100f;
  void Update(){
      
   if(healthpoints<=1){
       Destroy(parent);
       }
   }
}


