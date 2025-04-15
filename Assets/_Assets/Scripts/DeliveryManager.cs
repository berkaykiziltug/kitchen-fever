using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;


    private int waitingRecipesMax = 4;
    private float spawnRecipetimer;
    private float spawnRecipeTimerMax = 4f;

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
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipetimer -= Time.deltaTime;
        if (spawnRecipetimer <= 0)
        {
             spawnRecipetimer = spawnRecipeTimerMax;
             if (waitingRecipeSOList.Count < waitingRecipesMax)
             {
                 RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                 
                 waitingRecipeSOList.Add(waitingRecipeSO);
                 
                 OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
             }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
           RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

           if (waitingRecipeSO.kitchenObjectsSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
           {
               //Has the same number of ingredients.
               bool plateContentsMatchesRecipe = true;
               foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectsSOList)
               {
                   //Cycling through all ingredients in the recipe.
                   bool ingredientFound = false;
                   foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                   {
                       //Cycling through all ingredients in the Plate.

                       if (plateKitchenObjectSO == recipeKitchenObjectSO)
                       {
                           //Ingredient matches.
                           ingredientFound = true;
                           break;
                       }
                   }

                   if (!ingredientFound)
                   {
                       //This recipe ingredient was not found on the plate.
                       plateContentsMatchesRecipe = false;
                   }
               }

               if (plateContentsMatchesRecipe)
               {
                   //Player delivered the correct recipe
                   waitingRecipeSOList.RemoveAt(i);
                   
                   OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                   return;
               }
           }
        }
        
        //No matches found!
        //Player did not deliver a correct recipe.
        
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
}

