using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;   // CRITICAL
    }

    // Called from PlayerInput (Update)
    public void Move(Vector2 input)
    {
        moveInput =
            transform.forward * input.y +
            transform.right * input.x;

        moveInput = Vector3.ClampMagnitude(moveInput, 1f);
    }

    // Physics movement
    private void FixedUpdate()
    {
        Vector3 velocity = moveInput * moveSpeed;
        velocity.y = rb.linearVelocity.y;   // preserve gravity

        rb.linearVelocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Escape"))
        {
            SceneManager.LoadScene(0);
            Debug.Log("You won");

        }
    }
}
