using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
   private PlayerInputActions playerInputActions;
   public event EventHandler onInteractPerformed;
   public event EventHandler OnInteractAlternateAction;
   private void Awake()
   {
      playerInputActions = new PlayerInputActions();
      playerInputActions.Player.Enable();
      //Get the event from playerInputActions then subscribe to it. Then inside that method. Fire another event that is to be answered from playerController? Its weird...
      playerInputActions.Player.Interact.performed += Interact_performed;
      playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
   }

   private void InteractAlternate_performed(InputAction.CallbackContext obj)
   {
      OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
   }

   private void Interact_performed(InputAction.CallbackContext obj)
   {
      onInteractPerformed?.Invoke(this, EventArgs.Empty);
   }

   public Vector3 GetMovementVectorNormalized()
   {
      //This line just reads the Vector2 values from the playerInputActions.
      Vector2 inputVector =playerInputActions.Player.Move.ReadValue<Vector2>(); 
      
      inputVector = inputVector.normalized;
      return inputVector;
   }
}
