using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private void Update()
    {
        //reset it every update to not add up.
        Vector2 inputVector = new Vector2(0, 0);
        if(Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }
        inputVector = inputVector.normalized;
        //We don't want the character to move upwards in the world. So I construct a new vector that is 0 on the Y.
        Vector3 moveDir = new Vector3(inputVector.x * moveSpeed, 0, inputVector.y * moveSpeed);
        //Multiply with Time.deltaTime so the movement is not frame dependant.
        transform.position += moveDir*Time.deltaTime;
    }
}
