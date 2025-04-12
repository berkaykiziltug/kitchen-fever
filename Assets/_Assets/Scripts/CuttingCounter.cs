using UnityEngine;

public class CuttingCounter : BaseCounter
{
   [SerializeField]private KitchenObjectSO cutKitchenObjectSO;
   public override void Interact(PlayerController player)
   {

      if (!HasKitchenObject())
      {
         //There is no kitchen object here.
         if (player.HasKitchenObject())
         {
            //Player is carrying something
            player.GetKitchenObject().SetKitchenObjectParent(this);
         }
         else
         {
            //Player not carrying anything.
         }
      }
      else
      {
         //There is a kitchen object here.
         if (player.HasKitchenObject())
         {
            //Player is carrying something.
         }
         else
         {
            //player is not carrying anything.
            GetKitchenObject().SetKitchenObjectParent(player);
         }
      }
   }

   public override void InteractAlternate(PlayerController player)
   {
      if (HasKitchenObject())
      {
         //There is a kitchen object. So cut it.
         GetKitchenObject().DestroySelf();
         KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
         
      }
   }
}
