using Assets.Scripts.Singleton;
using UnityEngine;

public class PlayerAnimationController : Singleton<PlayerAnimationController> {
    [SerializeField] private Animator animator;

    public bool IsArmsOverwritte { get; private set; } = false;

    protected override void Awake() {
        base.Awake();
        if (this.animator == null) {
            this.animator = transform.root.GetComponentInChildren<Animator>();
        }
    }

    public void UpdateMovementInput(float newVelocity) {
        if (!this.IsArmsOverwritte) {
            Debug.Log("Updating movement velocity to " + newVelocity);
            this.animator.SetFloat(AnimationState.MovementVelocity, newVelocity);
        }
    }

    public class AnimationState {
        // Main States
        public static readonly int LiftTrigger = Animator.StringToHash("Lift Trigger");
        public static readonly int LaderClimbTrigger = Animator.StringToHash("Lader Climb Trigger");
        public static readonly int AxeSwingTrigger = Animator.StringToHash("Axe Swing Trigger");

        // Movement Blend Tree
        public static readonly int MovementVelocity = Animator.StringToHash("Movement Blend"); // 3 running, 1 walking, 0 Idle, -1 Backwards.

        // Arms Mask
        public static readonly int ArmsOverwritte = Animator.StringToHash("Apply Arms Overwrite"); // Enter/Exit condition, Boolean value to apply the arms overwrite layer. Needs to be true to play any of the arm animations. When false will stop looping arms animation.
        public static readonly int AimWaterHoseTrigger = Animator.StringToHash("Spray Hose Aim Trigger"); // Trigger to start aiming the water hose, starting the aiming blend tree.
        public static readonly int CarryWaterHoseTrigger = Animator.StringToHash("Carry Hose Trigger"); // Trigger to start carrying the water hose.
        public static readonly int CarryAxeTrigger = Animator.StringToHash("Axe Carry Trigger"); // Trigger to start carrying the axe.

        // Arms Mask, Water Hose Aiming Blend Tree
        public static readonly int AimWaterHoseHorizontal = Animator.StringToHash("Spray X"); // Horizontal input for aiming the water hose
        public static readonly int AimWaterHoseVertical = Animator.StringToHash("Spray Y"); // Vertical input for aiming the water hose

    }
}
