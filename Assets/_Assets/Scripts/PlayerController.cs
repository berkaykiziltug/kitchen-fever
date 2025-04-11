using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private LayerMask countersLayerMask;
    
    private bool isWalking;
    private Vector3 lastInteractDir;

    private void Start()
    {
        //Just subscribing to the event that is fired from GameInput class.
        gameInput.onInteractPerformed += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x , 0, inputVector.y );

        //if the moveDir is not zero. Then lastInteractDir is last moveDir. This way player is not needed to keep pressing movement keys to interact. It will remember last interactDirection.
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hit, interactionDistance, countersLayerMask))
        {
            //Identify the hit object.
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                //Call the Interact method on the counter.
                clearCounter.Interact();
            }
        }
        
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x , 0, inputVector.y );

        //if the moveDir is not zero. Then lastInteractDir is last moveDir. This way player is not needed to keep pressing movement keys to interact. It will remember last interactDirection.
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hit, interactionDistance, countersLayerMask))
        {
            //Identify the hit object.
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                //Call the Interact method on the counter.
                // clearCounter.Interact();
            }
        }
        else
        {
            Debug.Log("------");
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleMovement()
    {
        //GetMovementVectorNormalized just reads the values from PlayerInputActions and returns a normalized vector2
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
       
        //We don't want the character to move upwards in the world. So I construct a new vector that is 0 on the Y.
        Vector3 moveDir = new Vector3(inputVector.x , 0, inputVector.y );
    
        
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        
        //if the capsuleCast does not hit anything player can move.
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,playerRadius, moveDir, moveDistance );
        if (!canMove)
        {
            //Cannot move towards the direction.
            //Attemt just the X movement.
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0 ).normalized; 
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,playerRadius, moveDirX, moveDistance );
            if (canMove)
            {
                //Can Move Only On the X
                moveDir = moveDirX;
            }
            else
            {
                //cannot move only on the X
                //Attemt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z ).normalized; 
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,playerRadius, moveDirZ, moveDistance );
                if (canMove)
                {
                    //can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move in any direction.
                }
            }
        }
        //Slerping the vector gives more natural rotation of character to move direction.
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);

        if (canMove)
        {
            //Multiply with Time.deltaTime so the movement is not frame dependant.
            transform.position += moveDir*moveDistance;
        }
        isWalking = moveDir != Vector3.zero;
    }
}
