using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Animator Reference
    private Animator animator;

    //Can Move Flag
    private bool canMove;

    // Start is called before the first frame update
    private void Start()
    {
        //Get Animator Reference
        animator = GetComponent<Animator>();

        //Set Can Move
        canMove = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (canMove)
        {
            animator.SetFloat("MoveX", Input.GetAxis("Horizontal"), 0.5f, Time.deltaTime * 10f);
            animator.SetFloat("MoveZ", Input.GetAxis("Vertical"), 0.5f, Time.deltaTime * 10f);
        }
    }

    /// <summary>
    /// Controls Player's Ability to Move
    /// </summary>
    /// <param name="canMove">true - Allow Player to Move, false - Disallow Player to Move</param>
    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    /// <summary>
    /// Set Animation Parameters to 0
    /// </summary>
    public void StopPlayer()
    {
        animator.SetFloat("MoveX", 0);
        animator.SetFloat("MoveZ", 0);
    }
}
