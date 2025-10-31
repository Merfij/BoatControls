using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class BoatScript_2 : MonoBehaviour
{
    BoatControls inputActions;
    public Rigidbody rb_boat;
    public Rigidbody rb;
    private Transform boat;
    public float force = 5;
    public float torque = 5;
    public float forceWall = 100;
    public float torqueWall = 100;
    public float angle = 50f;
    //public Transform spawnpoint;
    private Gamepad[] gamepads;
    public PlayerInput player;
    public bool jumpTwo = false;
    public float jumpForce = 100f;
    [SerializeField] bool hasJumped = false;

    private void Awake()
    {
        inputActions = new BoatControls();
        rb_boat = GameObject.FindGameObjectWithTag("Boat").GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        boat = GameObject.FindGameObjectWithTag("Boat").GetComponent<Transform>();
    }
    private void OnEnable()
    {
        inputActions.Boat.Player_2_Left.performed += RotateLeft;
        inputActions.Boat.Player_2_Right.performed += RotateRight;
        inputActions.Boat.Player_2_jump.performed += Jump;
        inputActions.Enable();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (hasJumped == false)
        {
            //rb_boat.position += boat.transform.up * 10;
            rb_boat.AddForce(transform.position.x, transform.position.y * jumpForce, transform.position.x);
            hasJumped = true;
            Debug.Log("JUMP JUMP JUMP");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall")){
            if (transform.rotation.y < 0)
            {
                rb_boat.AddTorque(transform.up * torqueWall * Time.deltaTime, ForceMode.VelocityChange);
                rb_boat.AddForce(transform.forward * -forceWall * Time.deltaTime, ForceMode.VelocityChange);
                if(transform.rotation.y > 0)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
                }
            }
            if (transform.rotation.y > 0)
            {
                rb_boat.AddTorque(transform.up * -torqueWall * Time.deltaTime, ForceMode.VelocityChange);
                rb_boat.AddForce(transform.forward * -forceWall * Time.deltaTime, ForceMode.VelocityChange);
            }
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log(boat.transform.eulerAngles.y.ToString());
        if (boat.transform.eulerAngles.y > angle && boat.transform.eulerAngles.y < 179)
        {
            rb_boat.angularVelocity = Vector3.zero;
            boat.transform.rotation = Quaternion.Euler(boat.transform.rotation.x, angle, boat.transform.rotation.z);
        }
        if (boat.transform.eulerAngles.y > 179 && boat.transform.eulerAngles.y < 360 - angle)
        {
            rb_boat.angularVelocity = Vector3.zero;
            boat.transform.rotation = Quaternion.Euler(boat.transform.rotation.x, -angle, boat.transform.rotation.z);
        }

        Debug.Log(jumpTwo.ToString());
        //transform.position = spawnpoint.position;
    }

    private void RotateLeft(InputAction.CallbackContext context)
    {
        rb.AddTorque(rb_boat.transform.up * torque * Time.deltaTime, ForceMode.VelocityChange);
        rb_boat.AddTorque(rb_boat.transform.up * torque * Time.deltaTime, ForceMode.VelocityChange);
        rb_boat.AddForce(rb_boat.transform.forward * force * Time.deltaTime, ForceMode.VelocityChange);
    }
    private void RotateRight(InputAction.CallbackContext context)
    {
        rb.AddTorque(rb_boat.transform.up * -torque * Time.deltaTime, ForceMode.VelocityChange);
        rb_boat.AddTorque(rb_boat.transform.up * torque * Time.deltaTime, ForceMode.VelocityChange);
        rb_boat.AddForce(rb_boat.transform.forward * force * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void OnDisable()
    {

        inputActions.Disable();
    }

    //private void Update()
    //{
    //    for (int i = 0; i < gamepads.Length; i++)
    //    {
    //        Debug.Log(gamepads[i]);
    //    }
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
