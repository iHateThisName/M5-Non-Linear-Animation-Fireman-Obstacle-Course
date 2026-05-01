using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 7f;
    [SerializeField] private InputActionReference moveAction;
    private InputAction moveInputAction;
    private bool isMoving;
    private Transform mainCameraTransform;

    private void OnEnable() {
        // TODO : Move this to a better place, GameManager
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //

        this.moveInputAction = this.moveAction.action;
        moveInputAction.Enable();
        moveInputAction.performed += OnMove;
        moveInputAction.canceled += OnMove;

    }
    private void OnDisable() {
        moveInputAction.Disable();
        moveInputAction.performed -= OnMove;
        moveInputAction.canceled -= OnMove;
    }

    private void Start() {
        this.mainCameraTransform = Camera.main.transform;
    }

    private void OnMove(InputAction.CallbackContext context) {

        if (context.performed) {
            if (!this.isMoving) {
                this.isMoving = true;
                StartCoroutine(PerformeMove());
            } else {
                StopAllCoroutines();
                StartCoroutine(PerformeMove());
            }
        } else if (context.canceled) {
            this.isMoving = false;
        }
    }

    private IEnumerator PerformeMove() {
        Vector2 moveInput = this.moveInputAction.ReadValue<Vector2>();
        Vector3 moveDirection = this.mainCameraTransform.right * moveInput.x + this.mainCameraTransform.forward * moveInput.y;
        moveDirection.y = 0; // Keep the movement on the horizontal plane
        while (this.isMoving) {
            Vector3 motion = this.speed * Time.fixedDeltaTime * moveDirection;
            this.controller.Move(motion);
            this.mainCameraTransform.position += motion;
            this.controller.gameObject.transform.rotation = Quaternion.Slerp(this.controller.gameObject.transform.rotation, Quaternion.LookRotation(moveDirection), Time.fixedDeltaTime * 5f);
            yield return null;
        }
    }
}
