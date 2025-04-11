using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private PlayerController player;
    
    //It's better to just define a const string than writing the string over and over again. Can do a typo and compiler would never throw an error.
    private const string IS_WALKING ="IsWalking";
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Player has a method that returns a boolean to inform this script if isWalking is true or not.
        animator.SetBool(IS_WALKING,player.IsWalking());
    }
}
