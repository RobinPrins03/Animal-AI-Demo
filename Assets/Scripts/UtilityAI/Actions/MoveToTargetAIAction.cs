using UnityEngine;

namespace UtilityAI {
    [CreateAssetMenu(menuName = "UtilityAI/Actions/MoveToTargetAIAction")]
    public class MoveToTargetAIAction : AIAction {
        public override void Initialize(Context context) {
            Debug.Log($"Initialize called. TargetTag = {targetTag}");
            
            context.sensor.targetTags.Add(targetTag);
        }

        public override void Execute(Context context) {
            var target = context.sensor.GetClosestTarget(targetTag);
            if (target == null) return;

            context.agent.SetDestination(target.position);
            Debug.Log($"Moving to {target.name} at {target.position}");
        }
    }    
}

