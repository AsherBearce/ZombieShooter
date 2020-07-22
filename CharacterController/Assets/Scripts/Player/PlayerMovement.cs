using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Range(6, 15)]
    public float speed = 6;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public GameObject groundCheck;
    public LayerMask groundMask;
    public float jumpHeight = 2f;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public Vector3 getMovementVelocity()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        return transform.right * x + transform.forward * z;
    }

    public bool grounded()
    {
        return isGrounded;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = getMovementVelocity();

        controller.Move(move * Time.deltaTime * speed);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
