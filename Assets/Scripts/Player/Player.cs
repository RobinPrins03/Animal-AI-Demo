using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class Player : MonoBehaviour, IVisitable {
    List<IVisitable> visitableComponents = new List<IVisitable>();

    void Start() {
        visitableComponents.Add(gameObject.GetOrAdd<HungerComponent>());
        visitableComponents.Add(gameObject.GetOrAdd<ThirstComponent>());
    }
    public void Accept(IVisitor visitor) {
        foreach (var component in visitableComponents) {
            component.Accept(visitor);
        }
    }
}