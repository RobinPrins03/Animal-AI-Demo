using UnityEngine;

public class HungerComponent : MonoBehaviour, IVisitable {
    public int hunger = 100;

    public void Accept(IVisitor visitor) {
        visitor.Visit(this);
        Debug.Log("HungerComponent.Accept");
    }
}