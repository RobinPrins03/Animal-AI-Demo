using UnityEngine;

public class NpcAnimator : NpcComponent {
    private void Update() {
        npc.Animator.SetFloat("Speed", npc.CurrentSpeed);
    }
}
