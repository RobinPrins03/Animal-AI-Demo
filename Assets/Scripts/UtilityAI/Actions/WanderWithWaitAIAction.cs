using HelperScripts;
using UnityEngine;
using UnityEngine.AI;

namespace UtilityAI {
    [CreateAssetMenu(menuName = "UtilityAI/Actions/WanderWithWaitAction")]
    public class WanderWithWaitAIAction : AIAction {
        public float wanderRadius = 10f;
        float sampleDistance = 4f;
        public float arrivalThreshold = 1.5f;
        public float waitSeconds = 2f;
        public string navMeshAreaName = "Walkable";

        const string DestinationKey = "wander.destination";
        const string HasDestinationKey = "wander.hasDestination";
        const string WaitTimerKey = "wander.waitTimer";
        const string WaitingKey = "wander.isWaiting";
        const string AvoidUntilKey = "avoid.until";

        public override void Execute(Context context) {
            if (!context.agent.isOnNavMesh) return;

            float avoidUntil = context.GetData<float>(AvoidUntilKey);
            if (Time.time < avoidUntil) {
                return;
            }

            CountdownTimer waitTimer = GetOrCreateWaitTimer(context);
            bool isWaiting = context.GetData<bool>(WaitingKey);

            if (isWaiting) {
                context.agent.ResetPath();
                waitTimer.Tick(Time.deltaTime);

                if (!waitTimer.IsRunning) {
                    context.SetData(WaitingKey, false);
                    context.SetData(HasDestinationKey, false);
                }

                return;
            }

            if (NeedsNewDestination(context)) {
                if (TryGetRandomNavMeshPoint(context.agent.transform.position, out Vector3 point)) {
                    context.SetData(DestinationKey, point);
                    context.SetData(HasDestinationKey, true);
                    context.agent.SetDestination(point);
                }

                return;
            }

            if (HasArrived(context)) {
                StartWaiting(context, waitTimer);
                context.agent.ResetPath();
                return;
            }

            Vector3 currentDestination = context.GetData<Vector3>(DestinationKey);
            context.agent.SetDestination(currentDestination);
        }

        bool NeedsNewDestination(Context context) {
            bool hasDestination = context.GetData<bool>(HasDestinationKey);
            if (!hasDestination) return true;
            if (!context.agent.hasPath && !HasArrived(context)) return true;
            return false;
        }

        bool HasArrived(Context context) {
            if (context.agent.pathPending) return false;
            if (!context.agent.hasPath) return false;
            return context.agent.remainingDistance <= arrivalThreshold;
        }

        CountdownTimer GetOrCreateWaitTimer(Context context) {
            CountdownTimer timer = context.GetData<CountdownTimer>(WaitTimerKey);
            if (timer != null) return timer;

            timer = new CountdownTimer(waitSeconds);
            context.SetData(WaitTimerKey, timer);
            return timer;
        }

        void StartWaiting(Context context, CountdownTimer timer) {
            timer.Reset(waitSeconds);
            timer.Start();
            context.SetData(WaitingKey, true);
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