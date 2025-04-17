using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

   private const string PLAYER_PREFS_BINDINGS = "InputBindings";
   public static GameInput Instance { get; private set; }
   private PlayerInputActions playerInputActions;
   public event EventHandler onInteractPerformed;
   public event EventHandler OnInteractAlternateAction;
   public event EventHandler OnPauseAction;

   public event EventHandler OnInteractAction;
   public event EventHandler OnBindingRebind;

   public enum Binding
   {
      Move_Up,
      Move_Down,
      Move_Left,
      Move_Right,
      Interact,
      InteractAlternate,
      Pause
      
   }
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
      if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
      {
         playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
      }
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
      OnInteractAction?.Invoke(this, EventArgs.Empty);
   }

   public Vector3 GetMovementVectorNormalized()
   {
      //This line just reads the Vector2 values from the playerInputActions.
      Vector2 inputVector =playerInputActions.Player.Move.ReadValue<Vector2>(); 
      
      inputVector = inputVector.normalized;
      return inputVector;
   }

   public string GetBindingText(Binding binding)
   {
      switch (binding)
      {
         default:
         case Binding.Move_Up:
            return playerInputActions.Player.Move.bindings[1].ToDisplayString();
         case Binding.Move_Down:
            return playerInputActions.Player.Move.bindings[2].ToDisplayString();
         case Binding.Move_Left:
            return playerInputActions.Player.Move.bindings[3].ToDisplayString();
         case Binding.Move_Right:
            return playerInputActions.Player.Move.bindings[4].ToDisplayString();
         case Binding.Interact:
            return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
         case Binding.InteractAlternate:
            return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
         case Binding.Pause:
            return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
      }
   }

   public void RebindBinding(Binding binding, Action onActionRebound)
   {
      InputAction inputAction;
      int bindingIndex;
      switch (binding)
      {
         default:
         case Binding.Move_Up :
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 1;
            break;
         case Binding.Move_Down :
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 2;
            break;
         case Binding.Move_Left :
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 3;
            break;
         case Binding.Move_Right :
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 4;
            break;
         case Binding.Interact :
            inputAction = playerInputActions.Player.Interact;
            bindingIndex = 0;
            break;
         case Binding.InteractAlternate :
            inputAction = playerInputActions.Player.InteractAlternate;
            bindingIndex = 0;
            break;
         case Binding.Pause :
            inputAction = playerInputActions.Player.Pause;
            bindingIndex = 0;
            break;
      }
      playerInputActions.Player.Disable();
      inputAction.PerformInteractiveRebinding(bindingIndex)
         .OnComplete(callback =>
         {
            callback.Dispose();
            playerInputActions.Player.Enable();
            onActionRebound();


            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS,playerInputActions.SaveBindingOverridesAsJson() );
            PlayerPrefs.Save();
            OnBindingRebind?.Invoke(this, EventArgs.Empty);
         })
         .Start();
   }
}
