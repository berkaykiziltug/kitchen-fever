using UnityEngine;

public class CuttingCounter : BaseCounter
{
   [SerializeField]private CuttingRecipeSO[] cuttingRecipeSOArray;
   public override void Interact(PlayerController player)
   {

      if (!HasKitchenObject())
      {
         //There is no kitchen object here.
         if (player.HasKitchenObject())
         {
            //Player is carrying something
            if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
               //Player carryin something that can be cut
               player.GetKitchenObject().SetKitchenObjectParent(this);
            }
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
      if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
      {
         //There is a kitchen object here and it can be cut.
         KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
         GetKitchenObject().DestroySelf();
         KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
      }
   }

   private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
   {
      foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
      {
         if (cuttingRecipeSO.input == inputKitchenObjectSO)
         {
            return true;
         }
      }
      return false;
   }

   private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
   {
      foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
      {
         if (cuttingRecipeSO.input == inputKitchenObjectSO)
         {
            return cuttingRecipeSO.output;
         }
      }
      return null;
   }
}
