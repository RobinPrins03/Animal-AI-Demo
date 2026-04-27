using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UtilityAI {
    [CreateAssetMenu(menuName = "UtilityAI/Actions/AvoidWithTagsAction")]
    public class AvoidTargetAIAction : AIAction {
        public List<string> avoidTags = new();
        public float avoidDistance = 8f;
        float sampleDistance = 4f;
        public float avoidCooldownSeconds = 1.5f;
        public string navMeshAreaName = "Walkable";

        const string AvoidUntilKey = "avoid.until";
        const string WanderHasDestinationKey = "wander.hasDestination";
        const string WanderWaitingKey = "wander.isWaiting";

        public override void Initialize(Context context) {
            foreach (string tag in avoidTags) {
                if (!context.sensor.targetTags.Contains(tag)) {
                    context.sensor.targetTags.Add(tag);
                }
            }
        }

        public override void Execute(Context context) {
            if (!context.agent.isOnNavMesh) return;

            Transform threat = GetClosestThreat(context);
            if (threat == null) return;

            Vector3 agentPosition = context.agent.transform.position;
            Vector3 awayDirection = (agentPosition - threat.position).normalized;

            if (awayDirection == Vector3.zero) {
                awayDirection = -context.agent.transform.forward;
            }

            Vector3 desiredPosition = agentPosition + awayDirection * avoidDistance;
            int areaMask = GetAreaMask();

            if (NavMesh.SamplePosition(desiredPosition, out NavMeshHit hit, sampleDistance, areaMask)) {
                context.agent.ResetPath();

                context.SetData(AvoidUntilKey, Time.time + avoidCooldownSeconds);
                context.SetData(WanderHasDestinationKey, false);
                context.SetData(WanderWaitingKey, false);

                context.agent.SetDestination(hit.position);
            }
        }

        Transform GetClosestThreat(Context context) {
            Transform closestThreat = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 agentPosition = context.agent.transform.position;

            foreach (string tag in avoidTags) {
                Transform candidate = context.sensor.GetClosestTarget(tag);
                if (candidate == null) continue;

                float distanceSqr = (candidate.position - agentPosition).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr) {
                    closestDistanceSqr = distanceSqr;
                    closestThreat = candidate;
                }
            }

            return closestThreat;
        }

        int GetAreaMask() {
            if (string.IsNullOrWhiteSpace(navMeshAreaName)) {
                return NavMesh.AllAreas;
            }

            int areaIndex = NavMesh.GetAreaFromName(navMeshAreaName);
            if (areaIndex < 0) {
                Debug.LogWarning($"NavMesh area '{navMeshAreaName}' was not found. Falling back to all areas.");
                return NavMesh.AllAreas;
            }

            return 1 << areaIndex;
        }
    }
}