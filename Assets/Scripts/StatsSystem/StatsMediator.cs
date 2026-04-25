using System;
using System.Collections.Generic;
using System.Linq;


namespace StatsSystem {
    public class StatsMediator {
        readonly List<StatModifier> listModifiers = new();
        
        public void PerformQuery(object sender, Query query) {

            foreach (var modifier in listModifiers) {
                modifier.Handle(sender, query);
            }
        }

        public void AddModifier(StatModifier modifier) {
            listModifiers.Add(modifier); ;
            
            modifier.OnDispose += _ => listModifiers.Remove(modifier);
        }

        public void Update(float deltaTime) {
            foreach (var modifier in listModifiers) {
                modifier.Update(deltaTime);
            }

            foreach (var modifier in listModifiers.Where(modifier => modifier.MarkedForRemoval).ToList()) {
                modifier.Dispose();
            }
        }
    }
}    
public class Query {
    public readonly StatType StatType;
    public int Value;

    public Query(StatType statType, int value) {
        StatType = statType;
        Value = value;
    }
}