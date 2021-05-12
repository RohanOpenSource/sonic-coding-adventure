using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlatformerCharacterController ch;
    public float rotateSpeed = 5;
    Vector3 offset;
     
    void Start() {
        offset = target.transform.position - transform.position;
    }
     
    void LateUpdate() {
            float horizontal = inputManager.cameraInput * rotateSpeed;
            transform.Rotate(0, horizontal, 0);
            Quaternion rotation = Quaternion.Euler(0,transform.localEulerAngles.y, 0);
            transform.position = target.transform.position - (rotation* offset); 
            transform.LookAt(target.transform);
    

        
    }
}
