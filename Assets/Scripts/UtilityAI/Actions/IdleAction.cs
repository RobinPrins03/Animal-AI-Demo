using UnityEngine;

namespace UtilityAI {
    [CreateAssetMenu(menuName = "UtilityAI/Actions/IdleAction")]
    public class IdleAction : AIAction {
        public override void Execute(Context context) {
            context.agent.SetDestination(context.target.transform.position);
        }
    }
}
