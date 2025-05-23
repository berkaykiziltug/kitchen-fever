using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{

   public static event EventHandler OnAnyCut;
   public event EventHandler OnCut;
   public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
   
   [SerializeField]private CuttingRecipeSO[] cuttingRecipeSOArray;
   
   private int cuttingProgress;

   new public static void ResetStaticData()
   {
      OnAnyCut = null;
   }
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
               cuttingProgress = 0;
               CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
               OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
               {
                  progressNormalized = (float)cuttingProgress /cuttingRecipeSO.cuttingProgressMax
               });
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
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
               if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
               {
                  GetKitchenObject().DestroySelf();
               }
            }
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
         cuttingProgress++;
         
         OnCut?.Invoke(this, EventArgs.Empty);
         OnAnyCut?.Invoke(this, EventArgs.Empty);
         
         CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
         OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
         {
            progressNormalized = (float)cuttingProgress /cuttingRecipeSO.cuttingProgressMax
         });
         if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
         {
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
         }
         
      }
   }

   private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
   {
      CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
      return cuttingRecipeSO != null;
   }

   private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
   {
      CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
      if (cuttingRecipeSO != null)
      {
         return cuttingRecipeSO.output;
      }
      else
      {
         return null;
      }
   }

   private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
   {
      foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
      {
         if (cuttingRecipeSO.input == inputKitchenObjectSO)
         {
            return cuttingRecipeSO;
         }
      }
      return null;
   }
}
