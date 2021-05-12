using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float sightRange;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask player;
    void Awake()
    {
        target=GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.CheckSphere(transform.position,sightRange,player)){
            agent.SetDestination(target.position);
        }
        else{
            agent.SetDestination(transform.position);
        }
    }
}
