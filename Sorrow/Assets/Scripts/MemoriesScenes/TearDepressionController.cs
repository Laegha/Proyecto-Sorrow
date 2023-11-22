using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TearDepressionController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator agentAnimator;
    [SerializeField] bool waitForPlayer;
    [SerializeField] float distanceToWait;
    Transform player => GameObject.FindGameObjectWithTag("Player").transform;
    NavMeshAgent agent;

    bool isWaiting = false;

    private void Awake() => agent= GetComponent<NavMeshAgent>();

    private void Start()
    {
        if (!waitForPlayer)
            agentAnimator.Play("Walk");
        agent.SetDestination(target.position);
    }

    private void Update()
    {
        float distToDestination = Vector3.Distance(transform.position, target.position);

        if (distToDestination < .1f)
            EndWalk();

        if (!waitForPlayer)
            return;

        //si queremos que el agente espere al jugador, calculamos la distancia y vemos si esta suficientemente cerca como para seguir
        float distToPlayer = Vector3.Distance(player.position, transform.position);
        if (distToPlayer > distanceToWait)
        {
            switch(isWaiting)
            {
                //si recien entra en la distancia necesaria, detenemos el agente y activamos el bool isWaiting
                case false:
                    agent.isStopped = true;
                    agentAnimator.Play("Idle");
                    isWaiting = true;
                    break;
                //si ya estaba lejos, giramos al agente para que siempre mire al jugador estando quieto
                case true:
                    transform.LookAt(new Vector3 (player.position.x, transform.position.y, player.position.z));
                    break;
            }
            return;
        }
        //si la distancia esta bien y el agente esta quieto que se arranque a mover denuevo
        if(agent.isStopped)
        {
            agent.isStopped = false;
            agentAnimator.Play("Walk");
            isWaiting = false;
        }
    }

    void EndWalk()
    {
        //Girarse para mirar al player
        agentAnimator.Play("Idle");
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        //Destruir este script
        Destroy(this);
    }
}
