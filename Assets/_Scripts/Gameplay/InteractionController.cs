using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour {

    [Header("Refrences")]
    [SerializeField] private InputActionReference interactionRefrence;
    [SerializeField] private GameObject model;

    [Header("Interaction Settings")]
    [SerializeField] private EnumInteractionType interactionType;

    private InputAction interactionInputAction;
    private bool isPickedUp = false;
    [field: SerializeField] public bool IsPlayerInRange { get; private set; } = false;

    [SerializeField] private Transform StartTeleport;
    [SerializeField] private Transform EndTeleport;

    private void Start() {
        if (interactionType == EnumInteractionType.None) {
            Debug.LogWarning("Interaction type is set to None, this component will not do anything.");
        }
        this.interactionInputAction = this.interactionRefrence.action;

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            this.IsPlayerInRange = true;
            this.interactionInputAction.Enable();
            this.interactionInputAction.performed += OnInteraction;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            this.IsPlayerInRange = false;
            this.interactionInputAction.performed -= OnInteraction;
            this.interactionInputAction.Disable();

            if (this.interactionType == EnumInteractionType.Ladder && isPickedUp) {
                PlayerAnimationController.Instance.DetachFromLadder();
                PlayerMovement.Instance.Teleport(EndTeleport.position, EndTeleport.rotation);
                isPickedUp = false;

            }
        }
    }

    private void OnInteraction(InputAction.CallbackContext context) {
        if (!context.performed) return;
        if (!this.IsPlayerInRange) return;

        switch (this.interactionType) {
            case EnumInteractionType.Axe:
                if (!isPickedUp) {

                    PlayerAnimationController.Instance.PickUpAxe();
                    isPickedUp = true;
                }
                break;
            case EnumInteractionType.WaterHose:
                if (!isPickedUp) {
                    PlayerAnimationController.Instance.PickUpWaterHose();
                    isPickedUp = true;
                }
                break;
            case EnumInteractionType.Ladder:
                if (!isPickedUp) {
                    PlayerMovement.Instance.Teleport(StartTeleport.position, StartTeleport.rotation);
                    PlayerAnimationController.Instance.AttacheToLadder();
                    isPickedUp = true;
                }
                break;
            default:
                Debug.LogWarning("Interaction type is set to None, this component will not do anything.");
                break;
        }

        if (this.interactionType != EnumInteractionType.Ladder) {
            this.model.SetActive(false);
            PlayerAnimationController.Instance.currentInteraction = this;
        }

    }

    public void Drop() {
        if (!isPickedUp) return;
        this.model.SetActive(true);
        isPickedUp = false;
    }

    public enum EnumInteractionType { None, Axe, WaterHose, Ladder, }
}