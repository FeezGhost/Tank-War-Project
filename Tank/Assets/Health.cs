using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float HealthPoints
    {
        get
        {
            return healthpoints;
        }
        set
        {
            healthpoints = value;
            //what if health reach 0
            if (HealthPoints <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    [SerializeField]
    private float healthpoints = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
