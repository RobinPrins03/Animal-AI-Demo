using UnityEngine;

public class ThirstComponent : MonoBehaviour ,IVisitable {
    [SerializeField] int thirst = 100;

    public void Accept(IVisitor visitor) {
        visitor.Visit(this);
        Debug.Log("ThirstComponent.Accept");
    }
    
    public void AddThirst(int thirst) {
        this.thirst += thirst;
    }
}