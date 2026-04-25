using UnityEngine;

public class NpcComponent : MonoBehaviour {
    protected Npc npc;
    protected virtual void Awake() {
        npc = GetComponentInParent<Npc>();
    }
}
