using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interLadder : MonoBehaviour
{
    public pMovement characterScript;

    void Start()
    {
        characterScript = FindObjectOfType<pMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterScript.currentState = pMovement.CharacterState.ClimbingLadder;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && characterScript.currentState == pMovement.CharacterState.ClimbingLadder)
        {
            characterScript.currentState = pMovement.CharacterState.Idle;
        }
    }
}
