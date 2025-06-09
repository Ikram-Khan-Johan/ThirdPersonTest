using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 0.1f; // Degrees per second
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(x, 0, z).normalized;
        if (move.magnitude >= 0.1f)
        {
            // Calculate the target angle based on the input direction
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            // Rotate the player to face the direction of movement
            transform.rotation = Quaternion.Euler(0, angle, 0);
            // Move the character controller in the specified direction
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
           characterController.Move(moveDirection * speed * Time.deltaTime);
        }
    }
}
