using UnityEngine;

public class HungerComponent : MonoBehaviour, IVisitable {
    [SerializeField] int hunger = 100;

    public void Accept(IVisitor visitor) {
        visitor.Visit(this);
        Debug.Log("HungerComponent.Accept");
    }

    public void AddHunger(int hunger) {
        this.hunger += hunger;
    }
}