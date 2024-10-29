using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Moverment")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    private float runSpeed = 1;
    public float jumpPower;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    public Action inventory;
    private Rigidbody myRb;

    private void Awake()
    {
        myRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (!canLook)
            return;

        CameraLook();
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= (moveSpeed * runSpeed);
        dir.y = myRb.velocity.y;

        myRb.velocity = dir;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            runSpeed = 2f;
            StartCoroutine(nameof(CoEndBust));
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            StopCoroutine(nameof(CoEndBust));
            runSpeed = 1;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            myRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private IEnumerator CoEndBust()
    {
        yield return new WaitForSeconds(1);
        runSpeed = 1;
    }

    private bool IsGrounded()
    {
        Ray[] rayArr = new Ray[4]
        {
            new Ray(transform.position + (Vector3.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (Vector3.back * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (Vector3.left * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (Vector3.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rayArr.Length; i++)
        {
            if (Physics.Raycast(rayArr[0], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    private void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
