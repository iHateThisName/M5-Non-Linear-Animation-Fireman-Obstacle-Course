using Assets.Scripts.Singleton;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : Singleton<CameraManager> {
    [SerializeField] private List<CinemachineVirtualCameraBase> cinemachineCameras;
    [SerializeField] private InputActionReference aimRefrence;
    private InputAction aimAction;

    private bool isAiming = false;

    private void OnEnable() {
        this.aimAction = this.aimRefrence.action;
        this.aimAction.Enable();
        this.aimAction.performed += OnAim;
        this.aimAction.canceled += OnAim;
    }

    private void OnDisable() {
        this.aimAction.Disable();
        this.aimAction.performed -= OnAim;
        this.aimAction.canceled -= OnAim;
    }

    private void OnAim(InputAction.CallbackContext context) {
        if (context.performed) {
            this.isAiming = true;
        } else if (context.canceled) {
            this.isAiming = false;
        }
    }
}