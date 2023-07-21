using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class pMovement : MonoBehaviour
{
    private float
            horizontalMovement, verticalMovement,               //character movement variables
            horizontalMouseInput, verticalMouseInput,           //character and camera rotation variables
            verticalMouseOffset, horizontalMouseOffset;         //offset for vertical and horizontal camera's movement


    public float
            movementSpeed = 3.0f,                               //character movement speed value
            runningSpeed = 7.5f,                                //character running speed value
            cameraMovementSpeed = 3.0f,                         //character camera movement speed value
            cameraLookAroundSpeed = 2.0f,                       //character camera lookaround speed value
            minVerticalCameraAngle = -75.0f,                    //character camera minimal vertical rotation angle
            maxVerticalCameraAngle = 85.0f,                     //character camera maximal vertical rotation angle
            minHorizontalCameraAngle = -70.0f,                  //character camera minimal horizontal rotation angle
            maxHorizontalCameraAngle = 70.0f,                   //character camera maximal horizontal rotation angle
            jumpForce = 0.5f;                                   //character jump force multiplier value

    private bool
            isPlayerRunning = false,                            //character running state bool
            isJumpKeyPressed,                                   //character jump key state            
            isPlayerCameraRotate,                               //character ability to rotate camera only
            isPlayerLookAround;                                 //character lock state if camera look around

    public Camera characterCamera = null;                       //character camera object (MUST HAVE)
    private Rigidbody characterRB;                              //character rigidbody object (MUST HAVE)                
    private Ray raycastJumpRay;                                 //character raycast ground check ray
    private Quaternion originalRotation;                        //character Vector3 storage to store camera angle before look around


    void Start()
    {
        characterRB = gameObject.GetComponent<Rigidbody>();
        characterRB.constraints = RigidbodyConstraints.FreezeRotation; //freeze rigidbody's all rotation
        CursorOff();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPlayerOnTheGround())
        {
            isJumpKeyPressed = true;
        }

        isPlayerRunning = Input.GetKey(KeyCode.LeftShift) ? true : false;   //check leftShift key holding
        isPlayerCameraRotate = Input.GetKey(KeyCode.LeftAlt) ? true : false;   //check leftAlt key holding 

        Debug.DrawRay(characterCamera.transform.position, characterCamera.transform.forward, Color.red);
    }

    private void FixedUpdate()
    {    
        CharacterMovement();
        CharacterRotationMovement();
        CharacterCameraMovement();
        CharacterJump();
    }

    //=======================================================================================================================================================================================================

    void CharacterMovement()//character movement
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        float currentMovementSpeed = isPlayerRunning ? runningSpeed : movementSpeed; //switching between movement and running speed, assinging appropriate value to 'currentMovementSpeed'
        transform.position += Vector3.ClampMagnitude(transform.forward * verticalMovement * currentMovementSpeed * Time.deltaTime, 1.0f);
        transform.position += Vector3.ClampMagnitude(transform.right * horizontalMovement * currentMovementSpeed * Time.deltaTime, 1.0f);
    }

    void CharacterRotationMovement()//character rotation movement
    {
        horizontalMouseInput = Input.GetAxis("Mouse X");
        verticalMouseInput = Input.GetAxis("Mouse Y");
        if (!isPlayerLookAround)
        {
            Vector3 rotationVector = new Vector3(0, horizontalMouseInput * cameraMovementSpeed, 0);
            Quaternion rotationQuaternion = Quaternion.Euler(rotationVector);
            transform.rotation *= rotationQuaternion;
        }
    }

    void CharacterCameraMovement()//character camera horizontal rotation
    {
        verticalMouseOffset += verticalMouseInput * cameraMovementSpeed;
        verticalMouseOffset = Mathf.Clamp(verticalMouseOffset, minVerticalCameraAngle, maxVerticalCameraAngle);

        Vector3 cameraDefaultAngle = new Vector3(-verticalMouseOffset, 0, 0);
        characterCamera.transform.localRotation = Quaternion.Euler(cameraDefaultAngle);
    }
    
    void CharacterJump()
    {
        if (isJumpKeyPressed)
        {
            characterRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumpKeyPressed = false;
            Debug.Log("Player Jump");
        }
    }
       
    public bool isPlayerOnTheGround()
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
