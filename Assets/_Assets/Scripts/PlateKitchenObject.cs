using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
   public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

   public class OnIngredientAddedEventArgs : EventArgs
   {
      public KitchenObjectSO kitchenObjectSO;
   }
   private List<KitchenObjectSO> kitchenObjectSOList;
   [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

   private void Awake()
   {
      kitchenObjectSOList = new List<KitchenObjectSO>();
   }

   public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
   {
      if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
      {
         //not a valid ingredient.
         return false;
      }
      if (kitchenObjectSOList.Contains(kitchenObjectSO))
      {
         //Already has this type;
         return false;
      }
      else
      {
         
         kitchenObjectSOList.Add(kitchenObjectSO);
         OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
         {
            kitchenObjectSO = kitchenObjectSO
         });
         
         return true;
      }
   }
}
