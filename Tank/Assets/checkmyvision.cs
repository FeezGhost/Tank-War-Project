using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkmyvision : MonoBehaviour
{   // how senesitive we are about our vision
    public enum enmsensitivity {HIGH,LOW};
    // VARIABLE to check sensitivity
    public enmsensitivity sensitivity = enmsensitivity.HIGH;
    // Are we abel to see the target right now
    public bool targetInsight = false;
    // field of vision
    public float fieldofvision = 45f;
    // We need a refernce to our here as well
    private Transform target = null;
    // Reference to our eyes
    public Transform myEyes = null;
    // my transform component
    public Transform npcTransform = null;
    //My sphere collider
    private SphereCollider sphereCollider = null;
    // last known sight of object
    public Vector3 LastKnownSighting = Vector3.zero;
    private void Awake()
    {
        npcTransform = GetComponent<Transform>();
        sphereCollider = GetComponent<SphereCollider>();
        LastKnownSighting = npcTransform.position;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }
    bool InMyFieldofVision()
    {
        Vector3 dirToTarget = target.position - myEyes.position;
        //Get angel between forward and view direction
        float angle = Vector3.Angle(myEyes.forward,dirToTarget);
        if (angle <= fieldofvision)
            return true;
        else
            return false;
    }
    // A function for checking line of sight
    bool ClearLineofSight()
    {
        bool x=false;
        RaycastHit hit;
        if(Physics.Raycast(myEyes.position, (target.position - myEyes.position).normalized,
            out hit, sphereCollider.radius))
        {
            if (hit.transform.CompareTag("Player"))
            {
                x = true;
                //return true;
            }
            x = false;
           // return false;
        }
        return x;
    }
    void UpdateSight()
    {
        switch (sensitivity)
        {
            case enmsensitivity.HIGH:
                targetInsight = InMyFieldofVision() && ClearLineofSight();
                break;
            case enmsensitivity.LOW:
                targetInsight = InMyFieldofVision() || ClearLineofSight();
                break;
        }
    }
    private void onTriggerStay(Collider other)
    {
        UpdateSight();
        //Update last known SIGHTING
        if (targetInsight)
            LastKnownSighting = target.position;
    }
    private void onTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        targetInsight = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
