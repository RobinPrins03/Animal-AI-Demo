using UnityEngine;

public enum StatType {Hunger, Thirst}

namespace StatsSystem
{
    public class Stats
    {
        readonly StatsMediator mediator;
        readonly BaseStats baseStats;
        
        public StatsMediator Mediator => mediator;

        public int Hunger {
            get {
                var q = new Query(StatType.Hunger,  baseStats.hunger);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }

        public int Thirst {
            get {
                var q = new Query(StatType.Thirst, baseStats.thirst);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }

        public Stats(StatsMediator mediator, BaseStats baseStats) {
            this.mediator = mediator;
            this.baseStats = baseStats;
        }

        public override string ToString() => $"Hunger: {Hunger}, Thirst: {Thirst}";
            
        
    }
}
