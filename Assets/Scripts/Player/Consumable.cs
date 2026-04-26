using System;
using System.Reflection;
using UnityEngine;
[CreateAssetMenu(fileName = "Consumable", menuName = "Consumable")]
public class Consumable : ScriptableObject, IVisitor {
    public int HungerBonus = 10;
    public int ThirstBonus = 10;

    public void Visit(object o) {
        MethodInfo visitMethod = GetType().GetMethod("Visit", new Type[] {o.GetType()});
        if (visitMethod != null && visitMethod != GetType().GetMethod("Visit", new Type[] {typeof(object)})){
            visitMethod.Invoke(this, new object[] { o });
        } else {
            DefaultVisit(o);
        }
    }

    private void DefaultVisit(object o) {
        // Non implemented Components
        Debug.Log("Consumable.DefaultVisit");
    }

    public void Visit(HungerComponent hungerComponent) {
        hungerComponent.AddHunger(HungerBonus);
        Debug.Log("Consumable.Visit(HungerComponent):");
    }

    public void Visit(ThirstComponent thirstComponent) {
        thirstComponent.AddThirst(ThirstBonus);
        Debug.Log("Consumable.Visit(ThirstComponent):");
    }
}