using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.SceneManagement;
public class gamePoint : MonoBehaviour
{
    public  Vector3 angle2;
    public double distance;
    public GameObject target;
    public Vector3 lastknownSight = Vector3.zero;
    public GameObject parent;
    public float delay=2f;
    // Start is called before the first frame update
    
    bool  IsClose(){
        parent=GameObject.FindGameObjectWithTag("check1");
        Vector3 dirToTarget = target.transform.position - parent.transform.position;
        float angle=Vector3.Angle(parent.transform.forward, dirToTarget);
        angle2= dirToTarget;
        distance=Mathf.Sqrt(Mathf.Pow(angle2.x,2) +Mathf.Pow(angle2.y,2)+Mathf.Pow(angle2.z,2));
        
        if(distance<= 10){
            return true;
        }
        else{
            return false;

        }

    }
    void endGame(){
        SceneManager.LoadScene(0);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsClose()){
            Invoke("endGame",delay);
        }
    }
}
