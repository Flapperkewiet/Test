using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Looking")]
    public float LookSensitivity = 5f;
    public float UpDownLookRange = 90f;

    float VerticalRotation = 0;

    [Header("Movement")]
    public float ForwardSpeedMultiplier = 5f;
    public float SideSpeedMultiplier = 3.5f;
    public float SprintIncrease = 2.5f;
    public float SprintFOVChange;
    [Range(0.01f,0.5f)]
    public float SprintFOVChangeSpeed;

    float f;
    float s;

    [Header("Faling and Jumping")]
    public float GravityStrength = 5;
    public float JumpHeight = 7.5f;
    float DefaultVerticalVelocity = -5;
    bool DoubleJumped;

    [Range(30, 120)]
    public float FieldOfVision = 60;


    //refrences
    Camera mainCamera;
    CharacterController cc;
    //local variables
    float VerticalVelocity = 0;
    int IsSprinting = 0;    //(0 = walking, 1 = sprinting)
    float ForwardSpeed = 0;
    float SideSpeed = 0;

    void Awake()
    {
        Settings.Player = gameObject;
        cc = gameObject.GetComponent<CharacterController>();
        mainCamera = Camera.main;
        if (Settings.LockCursur)
        {
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
    }
    void Update()
    {
        #region Looking
        transform.Rotate(0, Input.GetAxis("Mouse X") * LookSensitivity, 0);

        //Up Down Looking       
        VerticalRotation -= Input.GetAxis("Mouse Y") * LookSensitivity;
        VerticalRotation = Mathf.Clamp(VerticalRotation, -UpDownLookRange, UpDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(VerticalRotation, 0, 0);
        #endregion

        #region Gravity
        if (cc.isGrounded)
            VerticalVelocity = DefaultVerticalVelocity;
        else
            VerticalVelocity += Physics.gravity.y * Time.deltaTime * GravityStrength;
        #endregion

        #region Movement
        f = Input.GetAxisRaw("Vertical");
        s = Input.GetAxisRaw("Horizontal");
        #endregion

        #region Sprinting

        if (Input.GetKey(KeyCode.LeftShift))
            IsSprinting = 1;
        else
            IsSprinting = 0;

        if (IsSprinting == 1 && ForwardSpeed != 0)
        {
            if (mainCamera.fieldOfView < FieldOfVision + SprintFOVChange)
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, FieldOfVision + SprintFOVChange, SprintFOVChangeSpeed);
        }
        else
        {
            if (mainCamera.fieldOfView > FieldOfVision)
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, FieldOfVision, SprintFOVChangeSpeed);
        }
        #endregion

        #region Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (cc.isGrounded)
            {
                DoubleJumped = false;
                VerticalVelocity = JumpHeight;
            }
            else if (DoubleJumped == false)
            {
                DoubleJumped = true;
                VerticalVelocity = JumpHeight;
            }
        }

        #endregion

        ForwardSpeed = f * (ForwardSpeedMultiplier + (IsSprinting * SprintIncrease));
        SideSpeed = s * (SideSpeedMultiplier + (IsSprinting * SprintIncrease * 0.8f));

        //Sending to CharacterController
        Vector3 speed = new Vector3(SideSpeed, VerticalVelocity, ForwardSpeed);
        speed = transform.rotation * speed;
        if (SideSpeed != 0 || ForwardSpeed != 0 || !cc.isGrounded || VerticalVelocity != DefaultVerticalVelocity)
            cc.Move(speed * Time.deltaTime);
    }

    void OnValidate()
    {
        Camera.main.fieldOfView = FieldOfVision;
    }
}
