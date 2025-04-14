using UnityEngine;

public class DeliveryCounter : BaseCounter
{
  
  public override void Interact(PlayerController player)
  {
    if (player.HasKitchenObject())
    {
      if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
      {
        //Only accepts plates.
        DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
        player.GetKitchenObject().DestroySelf();
      }
    }
  }
}
