using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
   public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
   public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
   public class OnStateChangedEventArgs : EventArgs
   {
      public State state;
   }
   public enum State
   {
      Idle,
      Frying,
      Fried,
      Burned
   }
   [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
   [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

   private State state = State.Idle;
   private float fryingTimer;
   private float burnedTimer;
   private FryingRecipeSO fryingRecipeSO;
   private BurningRecipeSO burningRecipeSO;


   private void Start()
   {
      state = State.Idle;
   }

   private void Update()
   {
      if (HasKitchenObject())
      {
         switch (state)
         {
            case State.Idle:
               break;
            case State.Frying:
            
               fryingTimer += Time.deltaTime;
               OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
               {
                  progressNormalized = fryingTimer/fryingRecipeSO.fryingTimerMax
               });

               if (fryingTimer > fryingRecipeSO.fryingTimerMax)
               {
                  //fried.
                  GetKitchenObject().DestroySelf();
                  KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                  
                  state = State.Fried;
                  burnedTimer = 0f;
                  burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                  OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                  {
                     state = state
                  });
                  
               }
               break;
            case State.Fried:
               
               burnedTimer += Time.deltaTime;
               OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
               {
                  progressNormalized = burnedTimer/burningRecipeSO.burnedTimerMax
               });

               if (burnedTimer > burningRecipeSO.burnedTimerMax)
               {
                  //fried.
                  GetKitchenObject().DestroySelf();
                  KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                  Debug.Log("Object burned");
                  state = State.Burned;
                  OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                  {
                     state = state
                  });
                  OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                  {
                     progressNormalized =0f
                  });
               }
               break;
            case State.Burned:
               break;
         }

         Debug.Log(state);
      }
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
               //Player carryin something that can be fried
               player.GetKitchenObject().SetKitchenObjectParent(this);
               fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
               state = State.Frying;
               fryingTimer = 0f;
               OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
               {
                  state = state
               });
               OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
               {
                  progressNormalized = fryingTimer/fryingRecipeSO.fryingTimerMax
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
                  state = State.Idle;
                  OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                  {
                     state = state
                  });
                  OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                  {
                     progressNormalized = 0f
                  });
               }
            }
         }
         else
         {
            //player is not carrying anything.
            GetKitchenObject().SetKitchenObjectParent(player);
            state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
               state = state
            });
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
               progressNormalized = 0f
            });
         }
      }
   }
   private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
   {
      FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
      return fryingRecipeSO != null;
   }

   private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
   {
      FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
      if (fryingRecipeSO != null)
      {
         return fryingRecipeSO.output;
      }
      else
      {
         return null;
      }
   }

   private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
   {
      foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
      {
         if (fryingRecipeSO.input == inputKitchenObjectSO)
         {
            return fryingRecipeSO;
         }
      }
      return null;
   }

   private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
   {
      foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
      {
         if (burningRecipeSO.input == inputKitchenObjectSO)
         {
            return burningRecipeSO;
         }
      }

      return null;
   }
}
