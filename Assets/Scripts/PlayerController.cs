using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float dashForce = 5.0f;
    [SerializeField] private float dashDuration = .5f;

    // Cached reference
    private bool isDashing = false;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        RotateTowardsMouse();
        DashForward();
    }

    private void Movement()
    {
        if (isDashing) return;
        InputAction moveAction = InputSystem.actions.FindAction("Move");
        Vector3 moveVector = moveAction.ReadValue<Vector3>();
        rb.linearVelocity = new Vector3(moveVector.x * moveSpeed, 0f, moveVector.z * moveSpeed);
    }

    private void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(lookAt);
        }
    }

    private void DashForward()
    {
        InputAction dashAction = InputSystem.actions.FindAction("Dash");
        if (dashAction.triggered)
        {
            StartCoroutine(PerformDash());
            
        }
    }

    private IEnumerator PerformDash()
    {
        float time = 0f;
        while(time <= dashDuration) {
            isDashing = true;
            rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);
            time += Time.deltaTime;
            yield return null;
        }
        isDashing = false;
    }
}