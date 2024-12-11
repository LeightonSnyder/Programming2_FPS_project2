using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Controller : MonoBehaviour
{
    [SerializeField] private ParticleSystem splashParticles;
    [SerializeField] private ParticleSystem dirtParticles;
    //Public floats
    public float WalkSpeed = 5f;
    public float SprintMultiplier = 2f;
    public float SprintSpeed = 10f;
    public float JumpForce = 5f;
    public float GroundCheckDist = 1.5f; //distance for raycast
    public float LookSensitivityX = 1f;
    public float LookSensitivityY = 1f;
    public float MaxYLookAngle = 90f;
    public float MinYLookAngle = -90f;
    public float Gravity = -9.8f;
    public float Acceleration = 5f;
    public float currentSpeed;


    public float footSize = 0.6f; //size for ground detector
    public float footMaxDist = 0.75f; //max distance for ground detector
    public LayerMask footMask;
    public bool isSprinting;

    private float verticalRotation = 0f;

    private Vector3 velocity;
    public Transform PlayerCamera;
    public CharacterController characterController;

    //Attacking
    public float mopRange = 3f;
    public float mopDelay = 0.1f;
    public float mopSpeed = 1f;
    public int mopDamage = 1;
    public float mopForce = 100f;
    public bool attacking = false;
    public bool readyToAttack = true;
    public LayerMask physicsMask;
    public LayerMask dirtMask;
    public GameObject targetDirt;
    public Animator mopAnim;

    //Gamemanager
    public GameManager gameManager;
    public SceneChanger sceneChanger;

    //Particles
    private ParticleSystem splashParticlesInstance;
    private ParticleSystem dirtParticlesInstance;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //Movement
        //Get direction
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
        moveDirection.Normalize();

        //Get sprint input
        if (Input.GetAxis("Sprint") > 0)
        {
            isSprinting = true;
        }
        else { isSprinting = false; }

        //Calculate speed and acceleration
        if (moveDirection != Vector3.zero) //if recieving movement input
        {
            currentSpeed += Acceleration * Time.deltaTime; //add acceleration to speed over time

            if ((currentSpeed > SprintSpeed) && (isSprinting)) //if sprinting, cap speed at sprinting max
            {
                currentSpeed = SprintSpeed;
            }
            else if ((currentSpeed < WalkSpeed) && (isSprinting)) //jump speed to walking max when sprint is pressed
            {
                currentSpeed = WalkSpeed;
            }
            else if ((currentSpeed > WalkSpeed) && (!isSprinting)) //if not sprinting, cap speed at walking max
            {
                currentSpeed = WalkSpeed;
            }
        }
        else { currentSpeed = 0f; } //otherwise, speed is zero
        
        //Apply speed to direction character is moving
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
        
        //Jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity.y = JumpForce;
            Debug.Log("Jumped");
        }
        else if (!IsGrounded())
        {
            velocity.y += Gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);

        //Mopping
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        //Camera
        if (PlayerCamera != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * LookSensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * LookSensitivityY;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, MinYLookAngle, MaxYLookAngle);

            PlayerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            transform.Rotate(Vector2.up * mouseX);
        }

        //Debug.Log(IsGrounded());
        Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.forward * mopRange, Color.red);

        Debug.Log(IsGrounded());
    }

    private void FixedUpdate()
    {
        //all your velocity change stuff goes here
        //calculate your direction
        //based off states/bools change vars (like with sprint)
        //add velocity
        //Jump();
    }

    //void Jump()
    //{
    //    //jump
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - transform.up * footMaxDist, footSize);
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        //Create spherecast to detect ground
        if (Physics.SphereCast(transform.position, footSize, -transform.up, out hit, footMaxDist, footMask)) 
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void Attack()
    {
        if (!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), mopSpeed);
        Invoke(nameof(MopCast), mopDelay);
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    private void MopCast()
    {
        RaycastHit hit;

        var mopDirection = transform.TransformDirection(Vector3.forward);


        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, mopRange, physicsMask))
        {
            //hit.rigidbody.AddForceAtPosition(mopDirection * mopForce, hit.point);
            hit.rigidbody.AddExplosionForce(mopForce, hit.point, 20f);
            //HitTarget(hit.point);
        }
        
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, mopRange, dirtMask))
        {
            targetDirt = hit.collider.gameObject;
            var dirtScript = targetDirt.GetComponent<MessScript>();
            SpawnDirtParticles(hit.point);
            //Destroy(targetDirt);


            //gameManager.MessCleaned();
            if (dirtScript.health > 0) { dirtScript.MessDamaged(); }
            else
            {
                Destroy(targetDirt);
                gameManager.MessCleaned();
            }
        }
        
        else if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, mopRange))
        {
            //spawn in particles
            SpawnSplashParticles(hit.point);
        }

        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, mopRange, dirtMask))
        {
            targetDirt = hit.collider.gameObject;
            var dirtScript = targetDirt.GetComponent<MessScript>();
            //Destroy(targetDirt);

            
            //gameManager.MessCleaned();
            if (dirtScript.health > 0) { dirtScript.MessDamaged(); }
            else
            {
                Destroy(targetDirt);
                gameManager.MessCleaned();
            }
        }
        mopAnim.SetTrigger("Attack");
    }

    private void SpawnSplashParticles(Vector3 hitPoint)
    {
        splashParticlesInstance = Instantiate(splashParticles, hitPoint, Quaternion.identity);
    }

    private void SpawnDirtParticles(Vector3 hitPoint)
    {
        dirtParticlesInstance = Instantiate(dirtParticles, hitPoint, Quaternion.identity);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Door")
        {
            Door cs = other.GetComponent<Door>();

            int sceneNum = cs.doorToSceneNum;
            Debug.Log(sceneNum);

            sceneChanger.MoveToScene(sceneNum);
        }
    }


}
