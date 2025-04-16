using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
   public static GameInput Instance { get; private set; }
   private PlayerInputActions playerInputActions;
   public event EventHandler onInteractPerformed;
   public event EventHandler OnInteractAlternateAction;
   public event EventHandler OnPauseAction;
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
      playerInputActions = new PlayerInputActions();
      playerInputActions.Player.Enable();
      //Get the event from playerInputActions then subscribe to it. Then inside that method. Fire another event that is to be answered from playerController? Its weird...
      playerInputActions.Player.Interact.performed += Interact_performed;
      playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
      playerInputActions.Player.Pause.performed += Pause_Performed;
   }

   private void OnDestroy()
   {
      playerInputActions.Player.Interact.performed -= Interact_performed;
      playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
      playerInputActions.Player.Pause.performed -= Pause_Performed;
      
      playerInputActions.Dispose();
   }

   private void Pause_Performed(InputAction.CallbackContext obj)
   {
      OnPauseAction?.Invoke(this, EventArgs.Empty);
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
