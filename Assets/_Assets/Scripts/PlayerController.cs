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
        Vector3 moveDir = new Vector3(inputVector.x * moveSpeed, 0, inputVector.y * moveSpeed);
        //Slerping the vector gives more natural rotation of character to move direction.
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        //Multiply with Time.deltaTime so the movement is not frame dependant.
        transform.position += moveDir*Time.deltaTime;
        isWalking = moveDir != Vector3.zero;
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
