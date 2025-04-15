using System;
using UnityEngine;

public class TrashCounter : BaseCounter
{
   public static event EventHandler OnAnyObjectTrashed;
   public override void Interact(PlayerController player)
   {
      if (player.HasKitchenObject())
      {
         player.GetKitchenObject().DestroySelf();
         OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
      }
   }
}
