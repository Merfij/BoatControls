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
    public float force = 5;
    public float torque = 5;
    public float forceWall = 100;
    public float torqueWall = 100;
    float moveY;
    public float angle = 50f;
    public float jumpForce;
    public float jumpForceDown;
    public float forceDownSlope;

    public Transform slope;
    public Transform boatTransform;

    private bool player1Readytojump = false;
    private bool player2Readytojump = false;

    private float jumpCancelTimer = 0.2f;

    private void Awake()
    {
        inputActions = new BoatControls();
        rb = GetComponent<Rigidbody>();
        slope = GetComponent<Transform>();
        inputActions.Enable();
    }
    private void OnEnable()
    {
        inputActions.Boat.Left.performed += RotateLeft;
        inputActions.Boat.Right.performed += RotateRight;
        inputActions.Boat.Jump.performed += Jump;
        inputActions.Boat.Jump_2.performed += Jump_2;
        inputActions.Enable();
    }

    private void Jump(InputAction.CallbackContext context)
    {

        player1Readytojump = true;
        Debug.Log("Player 1 ready to jump.");
        //rb.AddForce(transform.up * jumpForce * Time.deltaTime, ForceMode.VelocityChange);
        //StartCoroutine(FakeGravity());
    }

    private void Jump_2(InputAction.CallbackContext context)
    {
        player2Readytojump = true;
        Debug.Log("Player 2 ready to jump.");
        //rb.AddForce(transform.up * jumpForce * Time.deltaTime, ForceMode.VelocityChange);
        //StartCoroutine(FakeGravity());
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
        if (other.CompareTag("Wall"))
        {
            if (transform.rotation.y < 0)
            {
                rb.AddTorque(transform.up * torqueWall * Time.deltaTime, ForceMode.VelocityChange);
                rb.AddForce(transform.forward * -forceWall * Time.deltaTime, ForceMode.VelocityChange);
                if (transform.rotation.y > 0)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
                }
            }
            if (transform.rotation.y > 0)
            {
                rb.AddTorque(transform.up * -torqueWall * Time.deltaTime, ForceMode.VelocityChange);
                rb.AddForce(transform.forward * -forceWall * Time.deltaTime, ForceMode.VelocityChange);
            }
        }

        if (other.CompareTag("GravityOn"))
        {
            rb.freezeRotation = false;
            //rb.AddForce(transform.forward * force);
        }

        if (other.CompareTag("SpeedBoost"))
        {
            rb.AddForce(transform.forward * force);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("GravityOn"))
        {
            rb.freezeRotation = false;
            rb.AddForce(transform.forward * forceDownSlope * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (transform.eulerAngles.y > angle && transform.eulerAngles.y < 179)
        {
            rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);
        }
        if (transform.eulerAngles.y > 179 && transform.eulerAngles.y < 360 - angle)
        {
            rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(transform.rotation.x, -angle, transform.rotation.z);
        }

        bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, 0.2f);
        }

        bool IsGrounded2()
        {
            return Physics.Raycast(transform.position, -Vector3.up, 0.2f);
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
            Debug.Log("boat should jump");
            player1Readytojump = false;
            player2Readytojump = false;
        }

        //if (!IsGrounded())
        //{
        //    rb.AddForce(transform.up * -jumpForceDown * Time.deltaTime);
        //}

        Debug.Log(Vector3.Distance(transform.position, slope.transform.position).ToString());

        if(Vector3.Distance(transform.position, slope.transform.position) > 15)
        {
            rb.AddForce(transform.up * -jumpForceDown * Time.deltaTime);
        }
        
    }

    private void RotateLeft(InputAction.CallbackContext context)
    {
        rb.AddTorque(transform.up * torque * Time.deltaTime, ForceMode.VelocityChange);
        rb.AddForce(transform.forward * force * Time.deltaTime, ForceMode.VelocityChange);
    }
    private void RotateRight(InputAction.CallbackContext context)
    {
        rb.AddTorque(transform.up * -torque * Time.deltaTime, ForceMode.VelocityChange);
        rb.AddForce(transform.forward * force * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    //IEnumerator FakeGravity()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    rb.AddForce(transform.up * -jumpForceDown * Time.deltaTime, ForceMode.VelocityChange);
    //}

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
