using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] private bool debugMode;
    [SerializeField] private LayerMask ground;

    [Header("References")]
    [SerializeField] private Transform center;
    [SerializeField] private Rigidbody playerCharacter;
    [SerializeField] private CapsuleCollider characterCollider;

    [Header("Movement Values")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float inAirGravityMultiplier;
    public float runSpeed, walkSpeed, airSpeed, slopeSpeed;

    [Header("Slope Movement Values")]
    [SerializeField] private float height = 1.75f;
    [SerializeField] private float heightPadding = 0.05f;
    [SerializeField] private float maxGroundAngle = 120;
    [SerializeField] private float slopeForce;

    [Header("Wall Distance")]
    [SerializeField] private float maxDistanceCollision;
    [SerializeField] private float maxHitWallDistance;

    //  Animator state hash info
    AnimatorStateInfo stateInfo;

    //  movement
    Vector3 inputVector; 
    Vector3 forward;
    float ver, hor;
    [HideInInspector] public float speed;

    //  slope movement
    float angle;
    [ReadOnly] [SerializeField]float groundAngle;

    //  Raycast
    RaycastHit hitInfo;

    //  Boolean
    bool grounded;

    Camera mainCamera;
    AnalogStick analogStick;

    [Header("Debugger")]
    [ReadOnly] public Vector3 direction;
    [ReadOnly] public string TagGround;
    [ReadOnly] public Vector3 getCurrentVelocity;
    [ReadOnly] public Vector3 getWorkspace;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BodyPart")
        {
            GameManager.Instance.inventoryStageOne.OnTriggerEnterCharactrOnBodyPart =
                other.GetComponent<BodyPartController>().bodyPart;

            GameManager.Instance.inventoryStageOne.GetSetBodyPartObj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BodyPart")
        {
            GameManager.Instance.inventoryStageOne.OnTriggerEnterCharactrOnBodyPart = null;

            GameManager.Instance.inventoryStageOne.GetSetBodyPartObj = null;
        }
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        analogStick = GameManager.Instance.analogStick;
    }

    private void FixedUpdate()
    {
        if (debugMode)
            DrawDebugLines();

        //  On Slope
        CalculateFoward();
        CalculateGroundAngle();
        CheckGround();

        //  Player Movement
        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY && 
            GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.NONE &&
            GameManager.Instance.GetSetFindStartState == GameManager.FindStartState.NONE &&
            GameManager.Instance.sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.NONE)
        {
            //  StateInfo
            stateInfo = GameManager.Instance.genderSetter.GetSetGenderAnimator.GetCurrentAnimatorStateInfo(0);
            MovePlayer();
            SpeedMovementSetter();
        }
    }

    private void MovePlayer()
    {
        hor = analogStick.Horizontal();
        ver = analogStick.Vertical();

        if (stateInfo.fullPathHash != Animator.StringToHash("Base Layer.Non-Battle Locomotion.Idle Pickup") &&
            !GameManager.Instance.idlePickup)
        {
            direction = new Vector3(hor, 0f, ver);
            GameManager.Instance.direction = direction;
            //  Slope Gravity
            ApplySlopeGravity();

            inputVector = new Vector3(hor, 0f, ver).normalized;
            inputVector = mainCamera.transform.TransformDirection(inputVector);
            inputVector.y = 0f;

            if (inputVector.magnitude > 1.0f) inputVector = inputVector.normalized;


            if (hor != 0f || ver != 0f)
            {
                if (grounded)
                {
                    if (groundAngle < maxGroundAngle)
                        SetVelocityX(inputVector * speed);
                    //playerCharacter.MovePosition((Vector3) playerCharacter.position + inputVector * speed * Time.fixedDeltaTime);


                    playerCharacter.rotation = Quaternion.Slerp(playerCharacter.rotation,
                        Quaternion.LookRotation(inputVector.normalized), rotationSpeed *
                        Time.fixedDeltaTime);
                }
                else
                    SetVelocityX(inputVector * airSpeed);
            }
            else
            {
                SetVelocityZero();
            }

            //if (!grounded)
            //    SetVelocityY(new Vector3(GetCurrentVelocity.x, GetCurrentVelocity.y * inAirGravityMultiplier, GetCurrentVelocity.z));
        }
    }

    private void SpeedMovementSetter()
    {
        
            if (direction.magnitude == 0f)
                speed = 0f;

            if (direction.magnitude > 0f && direction.magnitude <= 0.5f)
                speed = walkSpeed;

            if (direction.magnitude > 0.5f && direction.magnitude <= 1f)
                speed = runSpeed;
    }

    public void SetVelocityZero()
    {
        playerCharacter.velocity = Vector3.zero;
        getCurrentVelocity = Vector3.zero;
    }

    public void SetVelocityX(Vector3 velocity)
    {
        getWorkspace.Set(velocity.x, getCurrentVelocity.y, velocity.z);
        playerCharacter.velocity = getWorkspace;
        getCurrentVelocity = getWorkspace;
    }

    #region SLOPE

    private void CalculateFoward()
    {
        if (!grounded)
        {
            forward = transform.forward;
            return;
        }

        forward = Vector3.Cross(hitInfo.normal, -transform.right);
    }

    private void CalculateGroundAngle()
    {
        if (!grounded)
        {
            groundAngle = 90;
            return;
        }

        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    private void CheckGround()
    {
        if (Physics.Raycast(center.position, Vector3.down, out hitInfo, height, ground))
        {
            grounded = true;
            TagGround = hitInfo.transform.gameObject.tag;
        }
        else
        {
            grounded = false;
            TagGround = "";
        }
    }

    private void ApplySlopeGravity()
    {
        if (groundAngle < 90 && (hor != 0 || ver != 0))
            playerCharacter.MovePosition((Vector3) playerCharacter.position +
                Vector3.down * characterCollider.height / 2 * slopeForce * Time.fixedDeltaTime);
    }

    private void DrawDebugLines()
    {
        Debug.Log("hello");
        Debug.DrawLine(center.position, center.position + forward * height * 2, Color.blue);
        Debug.DrawLine(center.position, center.position + Vector3.down * height, Color.yellow);
    }

    #endregion
}
