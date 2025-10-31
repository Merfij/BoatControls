using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputAction movement;
    private Vector2 moveDirection;
    public Rigidbody Rigidbody;
    public float moveSpeed;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        moveSpeed = 5f;
    }

    private void Update()
    {
        moveDirection = movement.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Rigidbody.linearVelocity = new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.y * moveSpeed) * Time.fixedDeltaTime;
    }

    private void OnEnable()
    {
        movement.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
    }
}
