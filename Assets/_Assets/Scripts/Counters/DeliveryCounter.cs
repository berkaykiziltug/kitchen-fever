using System;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
  public static DeliveryCounter Instance { get; private set; }

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
    }
    else
    {
      Instance = this;
    }
  }

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
