using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Npc : MonoBehaviour {
    [HideInInspector]
    public NavMeshAgent Agent;
    
    [HideInInspector]
    public Animator Animator;

    public float CurrentSpeed;
    
    private void Awake() {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }
    private void Update() {
        CurrentSpeed = Agent.velocity.magnitude;
    }
}

