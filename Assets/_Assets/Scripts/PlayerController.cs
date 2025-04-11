using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;
    private void Update()
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

    public bool IsWalking()
    {
        return isWalking;
    }
}
