using UnityEngine;

namespace UtilityAI {
    public abstract class AIAction : ScriptableObject {
        public string targetTag;
        public Consideration consideration;

        public virtual void Initialize(Context context) {
            //Useful later if functions need to be run before other logic.
        }
        
        public float CalculateUtility(Context context) => consideration.Evaluate(context);
        
        public abstract void Execute(Context context);
    }    
}

