/*
using UnityEngine;
using UnityEngine.AI;

namespace UtilityAI {
    public class NpcAnimation : MonoBehaviour
    {
        public NavMeshAgent agent;
        Animator animator;

        void Start() {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        void Update() {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }
}
*/
using UnityEngine;
using UnityEngine.AI;

namespace UtilityAI {
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
    public class NpcAnimation : MonoBehaviour {
        [SerializeField] float speedDampTime = 0.1f;

        NavMeshAgent agent;
        Animator animator;

        static readonly int SpeedHash = Animator.StringToHash("Speed");

        void Awake() {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        void Update() {
            float speed = agent.velocity.magnitude;
            animator.SetFloat(SpeedHash, speed, speedDampTime, Time.deltaTime);
        }
    }
}