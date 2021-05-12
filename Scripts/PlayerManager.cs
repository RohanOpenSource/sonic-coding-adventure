using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlatformerCharacterController characterController;
    [SerializeField] private Animator animator;
    private void Awake() {
        inputManager=GetComponent<InputManager>();
        characterController=GetComponent<PlatformerCharacterController>();
    }

    private void Update() {
        inputManager.HandleAllInputs();
        if(inputManager.movementInput!=Vector2.zero){
            animator.SetBool("Running",true);
        }
        else{
            animator.SetBool("Running",false);
        }
    }
    private void FixedUpdate() {
        characterController.HandlePlayer();
    }
}
