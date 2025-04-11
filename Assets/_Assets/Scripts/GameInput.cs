using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
   private PlayerInputActions playerInputActions;
   private void Awake()
   {
      playerInputActions = new PlayerInputActions();
      playerInputActions.Player.Enable();
   }

   public Vector3 GetMovementVectorNormalized()
   {
      //This line just reads the Vector2 values from the playerInputActions.
      Vector2 inputVector =playerInputActions.Player.Move.ReadValue<Vector2>(); 
      
      inputVector = inputVector.normalized;
      return inputVector;
   }
}
