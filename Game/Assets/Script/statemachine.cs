using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statemachine : MonoBehaviour
{
    public enum enemy_state {PATROL,CHASE,ATTACK,RETREAT,LASTSTANCE};
    [SerializeField]
    public enemy_state currentstate;
    private int scores;
    public Health playerhealth = null;
    public float maxDamage = 10f;
    public bool seei;//test
    public Text score;
    public double dd;
    public LineOfSight checkmyVision;
    public UnityEngine.AI.NavMeshAgent agent = null;
    public GameObject playertransform = null;
    public GameObject patrolDestination = null;
    public GameObject retreatDestination = null;
    public GameObject bulletSpawn;
    public float bulletSpeed= 100;
    public float lifeTime= 50;

    private void Awake(){
        checkmyVision=GetComponent<LineOfSight>();
        agent=GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerhealth= playertransform.GetComponent<Health>();
        score=score.GetComponentInChildren<Text>();
        scores=0;

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
            seei=checkmyVision.targetInSight;
            dd=checkmyVision.distance; 
            if(checkmyVision.targetInSight){
                agent.isStopped=true;
                currentstate=enemy_state.CHASE;
                yield break;
            }
             if(scores>=5){
                currentstate=enemy_state.RETREAT;
                yield break;
            }
            else if(dd<=61){
                agent.isStopped=true;
                currentstate=enemy_state.CHASE;
                yield break;
            }
            else{
                while(agent.pathPending){
                yield return null;}
            }
            yield break;
        }
        
    }
    public IEnumerator Retreat(){
        while(currentstate==enemy_state.RETREAT){
            checkmyVision.sensitivity = LineOfSight.Sensitivity.HIGH;
            agent.isStopped=false;
            agent.SetDestination(retreatDestination.transform.position);
            seei=checkmyVision.targetInSight;
            dd=checkmyVision.distance; 
            if(checkmyVision.targetInSight){
                agent.isStopped=true;
                currentstate=enemy_state.CHASE;
                yield break;
            }
            if(dd<=15){
                agent.isStopped=true;
                currentstate=enemy_state.LASTSTANCE;
                yield break;
            }
            else{
                while(agent.pathPending){
                yield return null;}
            }
            yield break;
        }
        
    }
    
    public IEnumerator LastStance(){
        while(currentstate==enemy_state.LASTSTANCE){
            
            score=score.GetComponentInChildren<Text>();
            scores+=4;
            score.text=scores+"";
            agent.isStopped=false;
            seei=checkmyVision.targetInSight;
            dd=checkmyVision.distance;
            
            agent.SetDestination(playertransform.transform.position);
            if(dd<=5){
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
    public IEnumerator EnemyChase(){
        while(currentstate==enemy_state.CHASE){
            agent.isStopped=false;
            agent.SetDestination(checkmyVision.lastknownSight);
            dd=checkmyVision.distance;
            seei=checkmyVision.targetInSight;
            
             if(dd<=12){
                currentstate=enemy_state.ATTACK;
                yield break;
            }
             else if(dd>=60){
                seei=false;
                agent.SetDestination(patrolDestination.transform.position);
                currentstate=enemy_state.PATROL;
            }
            else if(scores>=5){
                currentstate=enemy_state.RETREAT;
                yield break;
            }
            yield break;
        }
        
    }
    
    public IEnumerator EnemyAttack(){
        while(currentstate==enemy_state.ATTACK){
            
            score=score.GetComponentInChildren<Text>();
            scores+=2;
            score.text=scores+"";
            agent.isStopped=false;
            seei=checkmyVision.targetInSight;
            dd=checkmyVision.distance;
            
            agent.SetDestination(playertransform.transform.position);
            if(dd<=5){
                agent.isStopped=true;
            }
            else if(scores>=5){
                currentstate=enemy_state.RETREAT;
                yield break;
            }
            else if(dd>=13){
                currentstate=enemy_state.CHASE;
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
        seei=checkmyVision.targetInSight;
        dd=checkmyVision.distance;
        StopAllCoroutines();
            switch(currentstate){
                case enemy_state.PATROL:
                StartCoroutine(EnemyPatrol());
                break;
                case enemy_state.CHASE:
                StartCoroutine(EnemyChase());
                break;
                case enemy_state.ATTACK:
                StartCoroutine(EnemyAttack());
                break;
                case enemy_state.RETREAT:
                StartCoroutine(EnemyAttack());
                break;
                case enemy_state.LASTSTANCE:
                StartCoroutine(LastStance());
                break;
            }
    }
}
