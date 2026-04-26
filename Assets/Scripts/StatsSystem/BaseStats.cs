using UnityEngine;

namespace StatsSystem
{
    [CreateAssetMenu(fileName = "BaseStats", menuName = "Stats/BaseStats")]
    public class BaseStats : ScriptableObject {
        public int hunger = 100;
        public int thirst = 100;
    }
}
