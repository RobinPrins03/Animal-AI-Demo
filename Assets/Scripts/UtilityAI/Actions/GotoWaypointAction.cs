using HelperScripts;
using UnityEngine;
using UnityUtils;

namespace UtilityAI {
    [CreateAssetMenu(menuName = "UtilityAI/Actions/GotoWaypointAction")]
    public class GotoWaypointAction : AIAction {
        [SerializeField] private float idleTime = 2f;
        [SerializeField] private float arrivalThreshold = 1.5f;
        [SerializeField] private bool reverseAtEnd;

        const string WaypointIndexKey = "gotoWaypoint.currentIndex";
        const string DirectionKey = "gotoWaypoint.direction";
        const string RouteKey = "gotoWaypoint.route";
        const string WaitTimerKey = "gotoWaypoint.waitTimer";
        const string WaitingKey = "gotoWaypoint.isWaiting";

        public override void Initialize(Context context) {
            WaypointRoute route = context.brain.gameObject.GetOrAdd<WaypointRoute>();
            context.SetData(RouteKey, route);
        }

        public override void Execute(Context context) {
            if (!context.agent.isOnNavMesh) return;

            WaypointRoute route = GetRoute(context);
            if (route == null || route.Count == 0) return;

            CountdownTimer waitTimer = GetOrCreateWaitTimer(context);
            bool isWaiting = context.GetData<bool>(WaitingKey);

            if (isWaiting) {
                context.agent.ResetPath();
                waitTimer.Tick(Time.deltaTime);

                if (!waitTimer.IsRunning) {
                    context.SetData(WaitingKey, false);
                    AdvanceWaypoint(context, route);
                    MoveToCurrentWaypoint(context, route);
                }

                return;
            }

            MoveToCurrentWaypoint(context, route);

            if (HasArrived(context)) {
                StartWaiting(context, waitTimer);
            }
        }

        CountdownTimer GetOrCreateWaitTimer(Context context) {
            CountdownTimer timer = context.GetData<CountdownTimer>(WaitTimerKey);
            if (timer != null) return timer;

            timer = new CountdownTimer(idleTime);
            context.SetData(WaitTimerKey, timer);
            return timer;
        }

        void MoveToCurrentWaypoint(Context context, WaypointRoute route) {
            Transform waypoint = GetCurrentWaypoint(context, route);
            if (waypoint == null) return;

            context.agent.SetDestination(waypoint.position);
        }

        Transform GetCurrentWaypoint(Context context, WaypointRoute route) {
            int index = GetCurrentWaypointIndex(context, route);
            return route.GetWaypoint(index);
        }

        int GetCurrentWaypointIndex(Context context, WaypointRoute route) {
            int index = context.GetData<int>(WaypointIndexKey);
            if (index < 0 || index >= route.Count) {
                index = 0;
                context.SetData(WaypointIndexKey, index);
            }

            return index;
        }

        void AdvanceWaypoint(Context context, WaypointRoute route) {
            if (reverseAtEnd) {
                AdvanceWaypointReverseAtEnd(context, route);
                return;
            }

            AdvanceWaypointLoop(context, route);
        }

        void AdvanceWaypointLoop(Context context, WaypointRoute route) {
            int nextIndex = GetCurrentWaypointIndex(context, route) + 1;
            if (nextIndex >= route.Count) {
                nextIndex = 0;
            }

            context.SetData(WaypointIndexKey, nextIndex);
        }

        void AdvanceWaypointReverseAtEnd(Context context, WaypointRoute route) {
            if (route.Count <= 1) {
                context.SetData(WaypointIndexKey, 0);
                return;
            }

            int currentIndex = GetCurrentWaypointIndex(context, route);
            int direction = context.GetData<int>(DirectionKey);

            if (direction == 0) {
                direction = 1;
            }

            int nextIndex = currentIndex + direction;

            if (nextIndex >= route.Count) {
                direction = -1;
                nextIndex = currentIndex - 1;
            }
            else if (nextIndex < 0) {
                direction = 1;
                nextIndex = currentIndex + 1;
            }

            context.SetData(DirectionKey, direction);
            context.SetData(WaypointIndexKey, nextIndex);
        }

        bool HasArrived(Context context) {
            if (context.agent.pathPending) return false;
            if (!context.agent.hasPath) return false;
            return context.agent.remainingDistance <= arrivalThreshold;
        }

        void StartWaiting(Context context, CountdownTimer timer) {
            timer.Reset(idleTime);
            timer.Start();
            context.SetData(WaitingKey, true);
            context.agent.ResetPath();
        }

        WaypointRoute GetRoute(Context context) {
            WaypointRoute route = context.GetData<WaypointRoute>(RouteKey);
            if (route != null) return route;

            route = context.brain.gameObject.GetOrAdd<WaypointRoute>();
            context.SetData(RouteKey, route);
            return route;
        }
    }
}
