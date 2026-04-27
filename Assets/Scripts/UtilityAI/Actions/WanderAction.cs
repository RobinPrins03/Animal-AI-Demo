using UnityEngine;
using UnityEngine.AI;

namespace UtilityAI {
    [CreateAssetMenu(menuName = "UtilityAI/Actions/WanderAction")]
    public class WanderAIAction : AIAction {
        public float wanderRadius = 10f;
        float sampleDistance = 4f;
        float arrivalThreshold = 1.5f;
        public string navMeshAreaName = "Walkable";

        Vector3 currentDestination;
        bool hasDestination;

        public override void Execute(Context context) {
            if (!context.agent.isOnNavMesh) return;

            if (NeedsNewDestination(context)) {
                if (TryGetRandomNavMeshPoint(context.agent.transform.position, out Vector3 point)) {
                    currentDestination = point;
                    hasDestination = true;
                    context.agent.SetDestination(currentDestination);
                }
            }
        }

        bool NeedsNewDestination(Context context) {
            if (!hasDestination) return true;
            if (context.agent.pathPending) return false;
            if (context.agent.remainingDistance <= arrivalThreshold) return true;
            if (!context.agent.hasPath) return true;
            return false;
        }

        bool TryGetRandomNavMeshPoint(Vector3 origin, out Vector3 result) {
            int areaMask = GetAreaMask();

            for (int i = 0; i < 10; i++) {
                Vector3 randomOffset = Random.insideUnitSphere * wanderRadius;
                randomOffset.y = 0f;

                Vector3 candidate = origin + randomOffset;

                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, sampleDistance, areaMask)) {
                    result = hit.position;
                    return true;
                }
            }

            result = origin;
            return false;
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
