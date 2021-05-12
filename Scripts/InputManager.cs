using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    public Vector2 movementInput;
    public float verticalInput;
    public float cameraInput;   
    public float horizontalInput;
    private void OnEnable()
    {
        if(playerControls == null){
            playerControls=new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput =i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed+= m => cameraInput =m.ReadValue<float>();
        }
        playerControls.Enable();
    }
    private void Update() {
        playerControls.PlayerMovement.Jump.performed += i => GetComponent<PlatformerCharacterController>().Jump();
        playerControls.PlayerMovement.Dive.performed += i => GetComponent<PlatformerCharacterController>().Dive();
    }

    private void OnDisable(){
        playerControls.Disable();
    }

    public void HandleAllInputs(){
        HandleMovementInput();
    }
    private void HandleMovementInput(){
        verticalInput=movementInput.y;
        horizontalInput=movementInput.x;
    }
}
