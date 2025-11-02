using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    BoatControls inputActions;
    InputAction InputAction;
    public Transform targetLeft;
    public Transform targetRight;
    public Rigidbody rb;
    public float force;
    public float torque;
    public float forceWall = 100;
    public float torqueWall = 100;
    float moveY;
    public float angle = 50f;
    public float jumpForce;
    public float jumpForceDown;
    public float forceDownSlope;
    public float jumpBoost;

    public float rowLeftTimer = 0.5f;
    public float rowRightTimer = 0.5f;

    [SerializeField] private GameObject checkpoint;
    [SerializeField] private GameObject checkpoint_2;
    public Vector3 currentCheckPoint;

    public AudioSource paddle;
    public AudioClip paddleSound;
    public AudioClip woosh;
    public AudioClip impactObstacle;
    public AudioClip jump;
    public AudioClip checkpointSound;
    public AudioClip boatLanding;
    public AudioSource soundtrack;
    public AudioSource wind;

    public Transform slope;
    public Transform boatTransform;

    private bool player1Readytojump = false;
    private bool player2Readytojump = false;
    private bool isBoatInAir = false;

    private float jumpCancelTimer = 0.2f;

    private void Awake()
    {
        force = 3000;
        inputActions = new BoatControls();
        rb = GetComponent<Rigidbody>();
        slope = GetComponent<Transform>();
        checkpoint = GameObject.FindGameObjectWithTag("CheckPoint");
        checkpoint_2 = GameObject.FindGameObjectWithTag("Checkpoint_2");
        inputActions.Enable();
        paddle = GetComponent<AudioSource>();
        soundtrack = GetComponent<AudioSource>();
        soundtrack.Play();
        wind = GetComponent<AudioSource>();
        wind.Play();
    }

    private void Start()
    {
        rb.inertiaTensor = rb.inertiaTensor;
        rb.inertiaTensorRotation = rb.inertiaTensorRotation;
    }
    private void OnEnable()
    {
        inputActions.Boat.Left.performed += RotateLeft;
        inputActions.Boat.Right.performed += RotateRight;
        inputActions.Boat.Jump.performed += Jump;
        inputActions.Boat.Jump_2.performed += Jump_2;
        inputActions.Enable();
    }

    private void Update()
    {
        rowLeftTimer -= Time.deltaTime;
        rowRightTimer -= Time.deltaTime;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        player1Readytojump = true;
    }

    private void Jump_2(InputAction.CallbackContext context)
    {
        player2Readytojump = true;
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (transform.rotation.y < 0)
    //    {
    //        rb.AddTorque(transform.up * torqueWall * Time.deltaTime, ForceMode.VelocityChange);
    //        rb.AddForce(transform.forward * -forceWall * Time.deltaTime, ForceMode.VelocityChange);
    //        if (transform.rotation.y > 0)
    //        {
    //            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
    //        }
    //    }
    //    if (transform.rotation.y > 0)
    //    {
    //        rb.AddTorque(transform.up * -torqueWall * Time.deltaTime, ForceMode.VelocityChange);
    //        rb.AddForce(transform.forward * -forceWall * Time.deltaTime, ForceMode.VelocityChange);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Wall"))
        //{
        //    if (transform.rotation.y < 0)
        //    {
        //        rb.AddForce(transform.forward * forceWall);
        //        if (transform.rotation.y < 0)
        //        {
        //           rb.AddTorque(transform.up * torqueWall * Time.deltaTime, ForceMode.Force);
        //        }
        //    }
        //    if (transform.rotation.y > 0)
        //    {
        //        //rb.AddTorque(transform.up * torqueWall);
        //        rb.AddForce(transform.forward * forceWall);
        //        if (transform.rotation.y > 0)
        //        {
        //            rb.AddTorque(transform.up * -torqueWall * Time.deltaTime, ForceMode.Force);
        //        }
        //    }
        //}

        if (other.CompareTag("SpeedBoost"))
        {
            paddle.PlayOneShot(clip: woosh, volumeScale: 1f);
        }

        if (other.CompareTag("CheckPoint"))
        {
            currentCheckPoint = checkpoint.transform.position;
            paddle.PlayOneShot(clip: checkpointSound, volumeScale: 0.5f);
            Debug.Log(currentCheckPoint.ToString());
        }

        if (other.CompareTag("Checkpoint_2"))
        {
            currentCheckPoint = checkpoint_2.transform.position;
            paddle.PlayOneShot(clip: checkpointSound, volumeScale: 0.5f);
            Debug.Log(currentCheckPoint.ToString());
        }

        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("HIT!");
            paddle.PlayOneShot(clip: impactObstacle, volumeScale: 1f);
            rb.linearVelocity = Vector3.zero;
            transform.position = currentCheckPoint;
            //transform.position = new Vector3(transform.position.x, 2, transform.position.z);
        }

        if (other.CompareTag("GravityOn"))
        {
            //rb.freezeRotation = false;
            rb.angularDamping = 15;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        }

        if (other.CompareTag("GravityOff"))
        {
            //rb.freezeRotation = true;
            //rb.constraints = RigidbodyConstraints.None;
            rb.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            torque = 250;
            rb.angularDamping = 0.2f;
            rb.mass = 1;
            Debug.Log("Rotation on");
            rb.AddForce(transform.forward * force);
        }

        //if (other.CompareTag("SpeedBoost"))
        //{
        //    rb.AddForce(transform.forward * jumpBoost);
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Wall"))
        //{
        //    paddle.PlayOneShot(clip: impactObstacle, volumeScale: 0.4f);
        //    if (transform.rotation.y < 0)
        //    {
        //        rb.AddTorque(transform.up * torqueWall * Time.deltaTime, ForceMode.VelocityChange);
        //        rb.AddForce(transform.forward * -forceWall * Time.deltaTime, ForceMode.VelocityChange);
        //        if (transform.rotation.y > 0)
        //        {
        //            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        //        }
        //    }
        //    if (transform.rotation.y > 0)
        //    {
        //        rb.AddTorque(transform.up * -torqueWall * Time.deltaTime, ForceMode.VelocityChange);
        //        rb.AddForce(transform.forward * -forceWall * Time.deltaTime, ForceMode.VelocityChange);
        //    }
        //}

        if (other.CompareTag("GravityOn"))
        {
            //rb.AddForce(transform.forward * forceDownSlope * Time.deltaTime);
            //rb.AddForce(transform.up * -jumpForceDown * Time.deltaTime);
            //rb.angularDamping = 2;
            Debug.Log("On Slope Now");
        }

        if (other.CompareTag("SpeedBoost"))
        {
            rb.AddForce(transform.forward * jumpBoost);
        }
    }

    private void FixedUpdate()
    {
        //if (transform.eulerAngles.y > angle && transform.eulerAngles.y < 179)
        //{
        //    rb.angularVelocity = Vector3.zero;
        //    transform.rotation = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);
        //}
        //if (transform.eulerAngles.y > 179 && transform.eulerAngles.y < 360 - angle)
        //{
        //    rb.angularVelocity = Vector3.zero;
        //    transform.rotation = Quaternion.Euler(transform.rotation.x, -angle, transform.rotation.z);
        //}

        bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, 0.5f);
        }

        if (player1Readytojump)
        {
            jumpCancelTimer -= Time.deltaTime;
            if(jumpCancelTimer < 0)
            {
                player1Readytojump = false;
                jumpCancelTimer = 0.2f;
            } 
        }

        if (player2Readytojump)
        {
            jumpCancelTimer -= Time.deltaTime;
            if (jumpCancelTimer < 0)
            {
                player2Readytojump = false;
                jumpCancelTimer = 0.2f;
            }
        }

        if (player1Readytojump && player2Readytojump && IsGrounded())
        {
            rb.AddForce(transform.up * jumpForce);
            rb.angularVelocity = Vector3.forward * 1000;
            paddle.PlayOneShot(clip: jump, volumeScale: 0.7f);
            player1Readytojump = false;
            player2Readytojump = false;
            isBoatInAir = true;
        }

        if (IsGrounded() && isBoatInAir == true)
        {
            StartCoroutine(BoatLandSound());
        }

        if (!IsGrounded())
        {
            rb.AddForce(transform.up * -jumpForceDown * Time.deltaTime);
            Debug.Log("Force down active");
        }
    }

    IEnumerator BoatLandSound()
    {
        yield return new WaitForSeconds(1.4f);
        paddle.PlayOneShot(clip: boatLanding, volumeScale: 1f);
        isBoatInAir = false;
    }

    private void RotateLeft(InputAction.CallbackContext context)
    {
        if (rowLeftTimer <= 0)
        {
            rb.AddTorque(transform.up * torque * Time.deltaTime, ForceMode.VelocityChange);
            rb.AddForce(transform.forward * force * Time.deltaTime, ForceMode.VelocityChange);
            paddle.PlayOneShot(clip: paddleSound, volumeScale: 0.4f);
            rowLeftTimer = 0.5f;
        }
    }
    private void RotateRight(InputAction.CallbackContext context)
    {
        if (rowRightTimer <= 0)
        {
            rb.AddTorque(transform.up * -torque * Time.deltaTime, ForceMode.VelocityChange);
            rb.AddForce(transform.forward * force * Time.deltaTime, ForceMode.VelocityChange);
            paddle.PlayOneShot(clip: paddleSound, volumeScale: 0.4f);
            rowRightTimer = 0.5f;
        }
    }

    private void OnDisable()
    {
        inputActions.Boat.Left.performed -= RotateLeft;
        inputActions.Boat.Right.performed -= RotateRight;
        inputActions.Boat.Jump.performed -= Jump;
        inputActions.Boat.Jump_2.performed -= Jump_2;
        inputActions.Disable();
    }

    //public override float ReadValue(ref InputAction.CallbackContext context)
    //{
    //    var firstPArt = context.ReadValue<float>(firstPArt);
    //}

    //if (Input.GetKey(KeyCode.Joystick1Button1))
    //{
    //    rb.AddTorque(transform.up * torque *  Time.deltaTime, ForceMode.VelocityChange);
    //    rb.AddForce(transform.forward * force * Time.deltaTime, ForceMode.VelocityChange);
    //}
    //if (Input.GetKey(KeyCode.Joystick1Button0))
    //{
    //    rb.AddTorque(transform.up * -torque * Time.deltaTime, ForceMode.VelocityChange);
    //    rb.AddForce(transform.forward * force * Time.deltaTime, ForceMode.VelocityChange);
    //}
}
