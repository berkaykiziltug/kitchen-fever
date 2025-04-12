using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private void Update()
    {
       
    }

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
}
