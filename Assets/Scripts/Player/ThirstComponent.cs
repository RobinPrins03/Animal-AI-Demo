using UnityEngine;

public class ThirstComponent :  MonoBehaviour ,IVisitable {
    public int thirst = 100;

    public void Accept(IVisitor visitor) {
        visitor.Visit(this);
        Debug.Log("ThirstComponent.Accept");
    }
}