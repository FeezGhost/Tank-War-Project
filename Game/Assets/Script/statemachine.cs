using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statemachine : MonoBehaviour
{
    public enum enemy_state {PATROL,CHASE,ATTACK,RETREAT,LASTSTANCE};
    [SerializeField]
    public enemy_state currentstate;
    private int scores=1;
    public Health playerhealth = null;
    public float maxDamage = 10f;
    private bool seei;//test
    public Text score;
    public double dd;
    public LineOfSight checkmyVision;
    public UnityEngine.AI.NavMeshAgent agent = null;
    public GameObject playertransform = null;
    public GameObject patrolDestination = null;
    public GameObject retreatDestination = null;
    public GameObject bulletPrefab;
    public bool retreatCheck;
    
    public GameObject bulletSpawn;
    public float bulletSpeed= 100;
    public float lifeTime= 50;

    private void Awake(){
        checkmyVision=GetComponent<LineOfSight>();
        agent=GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerhealth= playertransform.GetComponent<Health>();
        

    }

    // Start is called before the first frame update

    void Start()
    {
        currentstate= enemy_state.PATROL;
        dd=checkmyVision.distance;

        
    }
    public IEnumerator EnemyPatrol(){
        while(currentstate==enemy_state.PATROL){
            checkmyVision.sensitivity = LineOfSight.Sensitivity.HIGH;
            agent.isStopped=false;
            agent.SetDestination(patrolDestination.transform.position);

            while(agent.pathPending)
                yield return null;
            
            seei=checkmyVision.targetInSight;
            dd=checkmyVision.distance;
            
            
            if(checkmyVision.check1Dist<=10){
                agent.isStopped=true;
                currentstate=enemy_state.RETREAT;
                yield break;
            }
            else if(checkmyVision.targetInSight){
                agent.isStopped=true;
                currentstate=enemy_state.CHASE;
                yield break;
            }
            else{

            }
            yield break;
        }
        
    }
    
    public IEnumerator EnemyChase(){
        while(currentstate==enemy_state.CHASE){
            score=score.GetComponentInChildren<Text>();
            scores++;
            score.text=scores+"";
            agent.isStopped=false;
            agent.SetDestination(checkmyVision.lastknownSight);
            dd=checkmyVision.distance;
            if(checkmyVision.check1Dist<=10){
                agent.isStopped=true;
                currentstate=enemy_state.RETREAT;
                yield break;
            }
            else if(retreatCheck){
                
                agent.isStopped=true;
                currentstate=enemy_state.LASTSTANCE;
                yield break;
            }
            else if(dd>=70){
                seei=false;
                agent.SetDestination(patrolDestination.transform.position);
                currentstate=enemy_state.PATROL;
            }
            else if(dd<=15){
                currentstate=enemy_state.ATTACK;
                yield break;
            }
            else{
                
            }
            yield break;
        }
        
    }
    public IEnumerator EnemyLS(){
        while(currentstate==enemy_state.ATTACK){
            
            score=score.GetComponentInChildren<Text>();
            scores+=20;
            score.text=scores+"";
            agent.isStopped=false;
            dd=checkmyVision.distance;
            
            agent.SetDestination(playertransform.transform.position);
            if(dd<=6){
                agent.isStopped=true;
            }
            else if(dd>=16){
                currentstate=enemy_state.CHASE;
            }
            else{
                
            }
            yield return null;
        }
        yield break;
        
    }
    
    public IEnumerator EnemyRetreat(){
        while(currentstate==enemy_state.RETREAT){
            retreatCheck=true;
            checkmyVision.sensitivity = LineOfSight.Sensitivity.HIGH;
            agent.isStopped=false;
            agent.SetDestination(retreatDestination.transform.position);

            while(agent.pathPending)
                yield return null;
            
            seei=checkmyVision.targetInSight;
            dd=checkmyVision.distance;
            
            if(checkmyVision.targetInSight){
                agent.isStopped=true;
                currentstate=enemy_state.CHASE;
                yield break;
            }
            yield break;
        }
        
    }
    public IEnumerator EnemyAttack(){
        while(currentstate==enemy_state.ATTACK){
            
            score=score.GetComponentInChildren<Text>();
            scores+=10;
            score.text=scores+"";
            agent.isStopped=false;
            dd=checkmyVision.distance;
            
            agent.SetDestination(playertransform.transform.position);
            if(dd<=6){
                agent.isStopped=true;
            }
            else if(dd>=16){
                currentstate=enemy_state.CHASE;
            }
            else if(checkmyVision.check1Dist<=10){
                agent.isStopped=true;
                currentstate=enemy_state.RETREAT;
                yield break;
            }
            else{
                
            }
            yield return null;
        }
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        StopAllCoroutines();
            switch(currentstate){
                case enemy_state.PATROL:
                StartCoroutine(EnemyPatrol());
                break;
                case enemy_state.CHASE:
                StartCoroutine(EnemyChase());
                break;
                case enemy_state.RETREAT:
                StartCoroutine(EnemyRetreat());
                break;
                case enemy_state.LASTSTANCE:
                StartCoroutine(EnemyLS());
                break;
                case enemy_state.ATTACK:
                Fire();
                StartCoroutine(EnemyAttack());
                break;
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
