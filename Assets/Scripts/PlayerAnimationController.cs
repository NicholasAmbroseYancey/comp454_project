using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private InputAction moveAction;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        moveAction = new InputAction(binding: "<Keyboard>/w");
        moveAction.AddBinding("<Keyboard>/a");
        moveAction.AddBinding("<Keyboard>/s");
        moveAction.AddBinding("<Keyboard>/d");
        moveAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
    }

    void Update()
    {
        if (animator == null) return;
        animator.SetBool("Walk", moveAction.IsPressed());
    }
}