using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public enum enmSensitivity {High, Low};
    public enmSensitivity sensitivity = enmSensitivity.High;
    public bool targetInSight = false;
    public float fieldOfVision = 45f;
    private Transform target = null;
    public Transform myEyes = null;
    public Transform npcTransform = null;
    private SphereCollider sphereCollider = null;
    public Vector3 lastSighting = Vector3.zero;

    private void Awake()
    {
        npcTransform = GetComponent<Transform>();
        sphereCollider = GetComponent<SphereCollider>();
        lastSighting = npcTransform.position;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    bool InFieldOfVision()
    {
        Vector3 dirToTarget = target.position - myEyes.position;
        float angle = Vector3.Angle(myEyes.forward, dirToTarget);
        if (angle <= fieldOfVision)
            return true;
        else
            return false;

    }
    bool ClearLineOfSight()
    {
        RaycastHit hit;
        if(Physics.Raycast(myEyes.position , (target.position - myEyes.position).normalized, out hit , sphereCollider.radius))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
    void UpdateSight()
    {
        switch (sensitivity)
        {
            case enmSensitivity.High:
                targetInSight = InFieldOfVision() && ClearLineOfSight();
                break;
            case enmSensitivity.Low:
                targetInSight = InFieldOfVision() || ClearLineOfSight();
                break;

        }
    }
    private void OnTriggerStay(Collider other)
    {
        UpdateSight();
        if (targetInSight)
            lastSighting = target.position;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        targetInSight = false;
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
