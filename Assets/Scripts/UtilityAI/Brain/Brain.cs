using UnityEngine;
using UnityEngine.AI;
using UtilityAI.Actions;
using System.Collections.Generic;

namespace UtilityAI.Brain {
    [RequireComponent(typeof(NavMeshAgent), typeof(Sensor))]
    public class Brain : MonoBehaviour {
        public List<AIAction> actions;
        public Context context;

        void Awake() {
            context = new Context(this);

            foreach (var action in actions) {
                action.Initialize(context);
            }
        }
        //Change this if performance is bad
        private void Update() {
            UpdateContext();
            
            AIAction bestAction = null;
            float highestUtility = float.MinValue;

            foreach (var action in actions) {
                float utility = action.CalculateUtility(context);
                if (utility > highestUtility) {
                    highestUtility = utility;
                    bestAction = action;
                }
            }

            if (bestAction != null) {
                bestAction.Execute(context);
            }
        }
        private void UpdateContext() {}
    }

}
