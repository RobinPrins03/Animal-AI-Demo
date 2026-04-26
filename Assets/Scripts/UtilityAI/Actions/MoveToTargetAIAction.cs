using UnityEngine;
using UtilityAI.Actions;

namespace UtilityAI {
    [CreateAssetMenu(menuName = "UtilityAI/Actions/MoveToTargetAIAction")]
    public class MoveToTargetAIAction : AIAction {
        public override void Initialize(Context context) {
            context.sensor.targetTags.Add(targetTag);
        }

        public override void Execute(Context context) {
            var target = context.sensor.GetClosestTarget(targetTag);
            if (target == null) return;

            context.agent.SetDestination(target.position);
        }
    }    
}

