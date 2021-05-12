using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlatformerCharacterController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    private Vector3 moveDirection;
    public int ringCount;
    [SerializeField] private Transform cameraObject;   
    [SerializeField] private Rigidbody rb; 
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Animator animator;
    [SerializeField] private float maxVel;
    [SerializeField] private GameObject aura;
    [SerializeField] private GameObject trail;
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource attackSFX;
    [SerializeField] private AudioSource springSFX;
    [SerializeField] private AudioSource checkpointSFX;
    [SerializeField] private AudioSource levelClear;
    [SerializeField] private AudioSource ringSFX;
    [SerializeField] private AudioSource ringLossSFX;
    [SerializeField] private AudioSource mainTrack;
    [SerializeField] private LayerMask enemy;
    private bool grounded;
    private bool canHomingAttack;
    public LayerMask ground;
    public bool isLooping;
    private float checkpointX;
    private float checkpointY;
    private float checkpointZ;

    private void Start() {
        checkpointX=transform.position.x;
        checkpointY=transform.position.y;
        checkpointZ=transform.position.z;
    }
    
    private void HandleMovement(){
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        if(isLooping) moveDirection = new Vector3(transform.forward.x,transform.forward.y,transform.forward.z)*inputManager.verticalInput;
        if(!isLooping)moveDirection = moveDirection+cameraObject.right*inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection = moveDirection * movementSpeed;
        if(isLooping) moveDirection=moveDirection*1.5f;

        Vector3 movementVelocity = new Vector3(moveDirection.x,0,moveDirection.z);
        if(isLooping) {
            movementVelocity=moveDirection+-transform.up*10;
        }
        if(!isLooping){
            movementVelocity+=-transform.up;
            rb.AddForce(movementVelocity);
        }
        else{rb.velocity=movementVelocity/2f;}
        if(inputManager.movementInput==Vector2.zero && !isLooping && grounded) {
            rb.drag=8;
        }
        else if((Vector2.Dot(new Vector2(moveDirection.x,moveDirection.z).normalized,new Vector2(rb.velocity.x,rb.velocity.z).normalized)<-0.1f) && grounded && !isLooping){
            rb.drag=8;
        }
        else if(grounded){
            rb.drag=1.25f;
        }
        else{
            rb.drag=0.5f;
        }
    }

    private void HandleRotation(){
        Vector3 targetDirection;
        targetDirection=cameraObject.forward*inputManager.verticalInput;
        targetDirection=targetDirection+cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y=0;
        if(targetDirection==Vector3.zero){
            targetDirection=transform.forward;
        }

        Quaternion targetRotation=Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation=Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed*Time.deltaTime);
        transform.rotation=playerRotation;
    }

    public void HandlePlayer(){
        HandleMovement();
        HandleRotation();
    }
    private void Update() {
        grounded=Physics.Raycast(transform.position+new Vector3(0,0.5f,0),Vector3.down,1f,ground);
        RaycastHit hit;
        if(Physics.Raycast(transform.position+transform.up.normalized*0.5f,-transform.up,out hit,1f,ground)){
            transform.rotation=Quaternion.FromToRotation(transform.up,hit.normal)*transform.rotation;
        }
        //if(isLooping) rb.velocity+=transform.up*-2f;
        animator.SetBool("Grounded",grounded);
        if(grounded){
            canHomingAttack=true;
            aura.SetActive(false);
            trail.SetActive(false);
            if(!isLooping)Physics.gravity=new Vector3(0,-5,0);
        }
        if(!grounded){
            aura.SetActive(true);
            trail.SetActive(true);
            if(!isLooping)Physics.gravity=new Vector3(0,-20,0);
        }

        if(rb.velocity.magnitude > maxVel)
         {
                rb.velocity = GetComponent<Rigidbody>().velocity.normalized * maxVel;
         }
    }

    public void Jump(){
        if(grounded){
            rb.velocity=new Vector3(rb.velocity.x,jumpForce,rb.velocity.z);
            jumpSFX.Play();
        }
        else if(canHomingAttack){
            Collider[] hits= Physics.OverlapSphere(transform.position,10,enemy);
            if(hits.Length>=1){
                transform.LookAt(hits[0].gameObject.transform);
                rb.velocity=transform.forward*50;
            }
            else{
                rb.velocity=transform.forward*50;
                canHomingAttack=false;
            }
            
            attackSFX.Play();
        }
    }
    public void Dive(){
        if(!grounded){
            rb.velocity=new Vector3(0,-15,0);
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(other.transform.tag=="Loop"){
            //transform.parent=other.transform;
            isLooping=true;
            Physics.gravity=Vector3.zero;
        }
    }
    private void OnCollisionExit(Collision other) {
        if(other.transform.tag=="Loop"){
            //transform.parent=null;
            isLooping=false;
            Physics.gravity=new Vector3(0,-20,0);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.tag=="Water"){
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            transform.position=new Vector3(checkpointX,checkpointY,checkpointZ);
            rb.velocity=Vector3.zero;
        }
        if(other.transform.tag=="Spring"){
            rb.velocity=other.transform.up*50;
            springSFX.Play();
        }
        if(other.transform.tag=="Level")SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        if(other.tag=="Checkpoint") {
            checkpointSFX.Play();
            checkpointX=transform.position.x;
            checkpointY=transform.position.y;
            checkpointZ=transform.position.z;
        }
        if(other.tag=="Stage"){
            levelClear.Play();
            mainTrack.Stop();
            movementSpeed=0;
            jumpForce=0;
            rb.velocity=Vector3.zero;
            
        }
        if(other.tag=="Ring"){
            ringCount++;
            Destroy(other.gameObject);
            ringSFX.Play();
        }
        if(other.tag=="Enemy"){
            if(grounded){
                rb.velocity=new Vector3(-transform.forward.x*50,0,-transform.forward.z*50);
                animator.SetTrigger("Hurt");
                if(ringCount!=0){
                    ringCount=0;
                    ringLossSFX.Play();
                }
                else{
                    transform.position=new Vector3(checkpointX,checkpointY,checkpointZ);
                    rb.velocity=Vector3.zero;
                }
            }
            else{
                rb.velocity=new Vector3(0,10,0);
                Destroy(other.gameObject);
            }
            
        }
    }


}
