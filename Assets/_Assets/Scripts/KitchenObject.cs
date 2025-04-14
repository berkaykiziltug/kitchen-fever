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

  public void DestroySelf()
  {
    kitchenObjectParent.ClearKitchenObject();
    Destroy(gameObject);
  }

  public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
  {
    Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
    KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
    
    kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    return kitchenObject;
  }

  public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
  {
    if (this is PlateKitchenObject)
    {
      plateKitchenObject = this as PlateKitchenObject;
      return true;
    }
    else
    {
      plateKitchenObject = null;
      return false;
    }
  }
}
