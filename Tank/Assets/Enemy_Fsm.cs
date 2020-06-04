using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy_Fsm : MonoBehaviour
{       
        //enums are nice to states
        public enum ENEMY_STATE {PATROL,CHASE,ATTACK };
        [SerializeField]
        private ENEMY_STATE currentState;
    public ENEMY_STATE CurrentState        
    {
        get { return currentState; }
        set
        {
            currentState = value;
            //Stop aa coroutines
            StopAllCoroutines();
            switch(currentState)
            {
                case ENEMY_STATE.PATROL:
                StartCoroutine(EnemyPatrol());
                    break;
                case ENEMY_STATE.CHASE:
                    StartCoroutine(EnemyChase());
                    break;
                case ENEMY_STATE.ATTACK:
                    StartCoroutine(EnemyAttack());
                    break;

            }
        }
    }
    // Lets have some reference
    private checkmyvision checkMyVision;
    private NavMeshAgent agent = null;
    private Transform playerTransform = null;
    private Transform patrolDestination = null;
    private Health playerHealth = null;
    public float maxDamage = 10f;
    private void Awake()
    {
        checkMyVision = GetComponent<checkmyvision>();
        agent = GetComponent<NavMeshAgent>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerTransform = playerHealth.GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //for random destination finding
        GameObject[] destination = GameObject.FindGameObjectsWithTag("Dest");
        patrolDestination = destination[Random.Range(0, destination.Length)].GetComponent<Transform>();
        currentState = ENEMY_STATE.PATROL;
    }
    public IEnumerator EnemyPatrol()
    {
        while (currentState == ENEMY_STATE.PATROL)
        {
            checkMyVision.sensitivity = checkmyvision.enmsensitivity.HIGH;
            agent.isStopped=false;
            agent.SetDestination(patrolDestination.position);
            while (agent.pathPending)
                yield return null; // for ensuring we wait for path
            if (checkmyvision.targetInsight)
            {
                agent.isStopped=true;
                currentState = ENEMY_STATE.CHASE;
                yield break;
            }
            yield break;
        }
        
    }
    public IEnumerator EnemyChase()
    {
        while (currentState == ENEMY_STATE.CHASE)
        {
            checkMyVision.sensitivity = checkmyvision.enmsensitivity.LOW;
            agent.isStopped = false;
            agent.SetDestination(checkMyVision.LastKnownSighting);
            while (agent.pathPending)
            {
                yield return null; // for ensuring we wait for path
            }
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                //what if we reach destination but cannot see the player
                if (!checkMyVision.targetInsight)
                {
                    currentState = ENEMY_STATE.PATROL;
                }
                else
                    currentState = ENEMY_STATE.ATTACK;
                yield break;
            }
        }
        yield break;
    }
    public IEnumerator EnemyAttack()
    {
        while (currentState == ENEMY_STATE.ATTACK)
        {
            agent.isStopped = false; 
            agent.SetDestination(playerTransform.position);
            while (agent.pathPending)
                yield return null;
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                CurrentState = ENEMY_STATE.CHASE;
            }
            else
            {
                playerHealth.healthPoints -= maxDamage * Time.deltaTime;
            }
            yield break;
        }
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
