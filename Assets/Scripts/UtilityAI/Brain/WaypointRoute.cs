using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI {
    public class WaypointRoute : MonoBehaviour {
        [SerializeField] private List<GameObject> waypoints = new();

        public int Count => waypoints.Count;

        public Transform GetWaypoint(int index) {
            if (index < 0 || index >= waypoints.Count) return null;

            GameObject waypoint = waypoints[index];
            return waypoint != null ? waypoint.transform : null;
        }
    }
}
