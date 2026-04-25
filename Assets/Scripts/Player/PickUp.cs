using UnityEngine;

public class PickUp : MonoBehaviour {
    public Consumable Consumable;

    void OnTriggerEnter(Collider other) {
        var visitable = other.GetComponent<IVisitable>();
        if (visitable != null) {
            visitable.Accept(Consumable);
            Destroy(gameObject);
        }
    }
}