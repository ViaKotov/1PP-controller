using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class pMovement : MonoBehaviour
{
    private float
            horizontalMovement, verticalMovement,               //character movement variables
            horizontalMouseInput, verticalMouseInput,           //character and camera rotation variables
            verticalMouseOffset, horizontalMouseOffset,         //offset for vertical and horizontal camera's movement
            currentSpeedMultiplier,                             //variable to store ternary operation character's speed multiplier value
            horizontalRotation;                                 //variable to store horizontal rotation value

    public float
            movementSpeed = 3.0f,                               //character movement speed value
            runningSpeed = 7.5f,                                //character running speed value
            cameraMovementSpeed = 3.0f,                         //character camera movement speed value
            cameraLookAroundSpeed = 2.0f,                       //character camera lookaround speed value
            minVerticalCameraAngle = -75.0f,                    //character camera minimal vertical rotation angle
            maxVerticalCameraAngle = 85.0f,                     //character camera maximal vertical rotation angle
            minHorizontalCameraAngle = -70.0f,                  //character camera minimal horizontal rotation angle
            maxHorizontalCameraAngle = 70.0f,                   //character camera maximal horizontal rotation angle
            jumpForce = 0.5f,                                   //character jump force multiplier value
            cameraSensitivity = 0.2f;                           //character camera smoothing movement value
    public enum CharacterState                                  //character state enumerators to track
    {
        Idle,
        Walking,
        Running,
        Jumping,
        Crouching,
        ClimbingLadder,
        InTheAir
    }
    public CharacterState currentState;                         //character state variable to assign new states

    private bool
            isPlayerRunning = false,                            //character running state bool
            isJumpKeyPressed,                                   //character jump key state            
            isPlayerCrouching;                                 //character ability to crouching

    private float
            colliderHeightValue,                                //character collider's collider common height
            colliderCrouchValue;                                //character collider's crouching height

    public Camera characterCamera = null;                       //character's camera object (MUST HAVE)
    private Rigidbody characterRB;                              //character's rigidbody object (MUST HAVE)                
    private CapsuleCollider characterCollider;                  //character's collider component


    private bool enableDebugMSG = true;                         //ENABLE OR DISABLE PROPRIETARY DEBUG CONSTRUCTION

    void Start()
    {
        characterRB = gameObject.GetComponent<Rigidbody>();
        characterCollider = gameObject.GetComponent<CapsuleCollider>();

        colliderHeightValue = characterCollider.height;
        gameObject.tag = "Player";
        characterRB.constraints = RigidbodyConstraints.FreezeRotation; //freeze rigidbody's all rotation

        CursorOff();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isCharacterOnTheGround())
        {
            isJumpKeyPressed = true;
        }

        isPlayerRunning = Input.GetKey(KeyCode.LeftShift);   //check leftShift key holding
        isPlayerCrouching = Input.GetKey(KeyCode.LeftControl);   //check leftControl key holding 
        
        //Debug.DrawRay(characterCamera.transform.position, characterCamera.transform.forward, Color.red);
    }

    private void FixedUpdate()
    {
        CharacterMovement();
        CharacterJump();
        CharacterCrouching();
        CharacterRotationMovement();
        CharacterCameraMovement();
        PlayerSceneRestart();


        //DebugMSG($"Horizontal = {horizontalMovement}, Vertical = {verticalMovement}");
    }


    //=======================================================================================================================================================================================================

    void CharacterMovement()//character movement
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        currentSpeedMultiplier = isPlayerRunning ? runningSpeed : movementSpeed; //switching between movement and running speed, assinging appropriate value to 'currentMovementSpeed'
        transform.position += Vector3.ClampMagnitude(transform.right * horizontalMovement * currentSpeedMultiplier * Time.deltaTime, 1.0f);
        if (currentState == CharacterState.ClimbingLadder)
        {
            characterRB.useGravity = false;
            characterRB.isKinematic = true;
            transform.position += Vector3.ClampMagnitude(transform.up * verticalMovement * 1.2f * Time.deltaTime, 1.0f);
        }
        else if(currentState != CharacterState.ClimbingLadder && characterRB.useGravity == false)
        {
            characterRB.isKinematic = false;
            characterRB.useGravity = true;
        }
        else if(currentState == CharacterState.ClimbingLadder && characterRB.useGravity == false && isCharacterOnTheGround())
        {
            characterRB.useGravity = true;
            characterRB.isKinematic = false;
            transform.position += Vector3.ClampMagnitude(transform.forward * verticalMovement * currentSpeedMultiplier * Time.deltaTime, 1.0f);
        }
        else
        {
            transform.position += Vector3.ClampMagnitude(transform.forward * verticalMovement * currentSpeedMultiplier * Time.deltaTime, 1.0f);
        }
    }
    
    void CharacterRotationMovement()//character rotation movement
    {
        horizontalMouseInput = Input.GetAxis("Mouse X");
        horizontalRotation += horizontalMouseInput * cameraMovementSpeed;
        Vector3 rotationVector = new (0f, horizontalRotation, 0f);
        Quaternion rotationQuaternion = Quaternion.Euler(0f, horizontalRotation, 0f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotationQuaternion, cameraSensitivity);
    }

    void CharacterCameraMovement()//character camera vertical rotation
    {
        verticalMouseInput = Input.GetAxis("Mouse Y");
        verticalMouseOffset += -verticalMouseInput * cameraMovementSpeed;
        verticalMouseOffset = Mathf.Clamp(verticalMouseOffset, minVerticalCameraAngle, maxVerticalCameraAngle);
        Vector3 cameraDefaultAngle = new (verticalMouseOffset, 0, 0);
        characterCamera.transform.localRotation = Quaternion.Slerp(characterCamera.transform.localRotation, Quaternion.Euler(cameraDefaultAngle) , cameraSensitivity);
    }
    
    void CharacterJump()
    {
        if (isJumpKeyPressed)
        {
            characterRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumpKeyPressed = false;
        }
    }
       
    void CharacterCrouching()
    {
        if (isPlayerCrouching)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, 0.5f, gameObject.transform.localScale.z);
        }
        else
        {
            if (!Physics.Raycast(transform.position, transform.up, 1.5f, 1 << LayerMask.NameToLayer("Obstruction")))
            {
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, 1.0f, gameObject.transform.localScale.z);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, 0.5f, gameObject.transform.localScale.z);
                DebugMSG("Obstruction found. Couldn't stand up");
            }
            
        }
        
    }

    void DebugMSG(string message)
    {
        if (enableDebugMSG)
        {
            Debug.Log(message);
        }
    }

    private void PlayerSceneRestart()
    {
        if(Physics.Raycast(transform.position, -transform.up, 1.1f, 1 << LayerMask.NameToLayer("Water")))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public bool isCharacterOnTheGround()
    {
        bool result = Physics.Raycast(transform.position, -transform.up, 1.1f, 1 << LayerMask.NameToLayer("Ground")) ? true : false;
        return result;
    } 

    private void CursorOff()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
