using UnityEngine;

public class KitchenObject : MonoBehaviour
{
  [SerializeField] private KitchenObjectSO kitchenObjectSO;
  private IKitchenObjectParent kitchenObjectParent;

  public KitchenObjectSO GetKitchenObjectSO()
  {
    return kitchenObjectSO;
  }

  public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
  {
    if (this.kitchenObjectParent != null)
    {
      //Clear the object on previous counter
      this.kitchenObjectParent.ClearKitchenObject();
    }
    //Set the kitchen object to current counter
    this.kitchenObjectParent = kitchenObjectParent;
    if (this.kitchenObjectParent.HasKitchenObject())
    {
      Debug.LogError("KitchenObjectParent already has a kitchen object.");
    }
    kitchenObjectParent.SetKitchenObject(this);
    
    //Move kitchen object to new position.
    transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
    transform.localPosition = Vector3.zero;
  }

  public IKitchenObjectParent GetKitchenObjectParent()
  {
    return kitchenObjectParent;
  }
}
