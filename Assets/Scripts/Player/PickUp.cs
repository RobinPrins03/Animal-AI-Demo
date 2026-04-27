using DG.Tweening;
using UnityEngine;
//OLD
/*public class PickUp : MonoBehaviour {
    public Consumable Consumable;

    void OnTriggerEnter(Collider other) {
        var visitable = other.GetComponent<IVisitable>();
        if (visitable != null) {
            visitable.Accept(Consumable);
            
            DOTween.Kill(transform);
            
            Destroy(gameObject);
        }
    }
}*/

public class PickUp : MonoBehaviour
{
    public Consumable Consumable;

    void OnTriggerEnter(Collider other)
    {
        var pickupTrigger = other.GetComponent<PickupTrigger>();
        if (pickupTrigger == null || pickupTrigger.owner == null) return;

        pickupTrigger.owner.Accept(Consumable);

        DOTween.Kill(transform);
        Destroy(gameObject);
    }
}