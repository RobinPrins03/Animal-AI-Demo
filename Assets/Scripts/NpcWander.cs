using UnityEngine;

public class NpcWander : NpcComponent {
    [SerializeField] 
    public Area Area;

    private void Start() {
        SetRandomDestination();
    }

    private void Update() {
        if (HasArrived()) {
            SetRandomDestination();
        }
    }

    bool HasArrived() {
        return npc.Agent.remainingDistance <= npc.Agent.stoppingDistance;
    }

    private void SetRandomDestination() {
        npc.Agent.SetDestination(Area.GetRandomPoint());
    }
}
