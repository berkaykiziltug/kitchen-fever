using UnityEngine;

public class KitchenObject : MonoBehaviour
{
  [SerializeField] private KitchenObjectSO kitchenObjectSO;
  private ClearCounter clearCounter;

  public KitchenObjectSO GetKitchenObjectSO()
  {
    return kitchenObjectSO;
  }

  public void SetClearCounter(ClearCounter clearCounter)
  {
    if (this.clearCounter != null)
    {
      //Clear the object on previous counter
      this.clearCounter.ClearKitchenObject();
    }
    //Set the kitchen object to current counter
    this.clearCounter = clearCounter;
    if (this.clearCounter.HasKitchenObject())
    {
      Debug.LogError("Counter already has a kitchen object.");
    }
    clearCounter.SetKitchenObject(this);
    
    //Move kitchen object to new position.
    transform.parent = clearCounter.GetKitchenObjectFollowTransform();
    transform.localPosition = Vector3.zero;
  }

  public ClearCounter GetClearCounter()
  {
    return clearCounter;
  }
}
