using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveDir {  get; private set; } = Vector2.zero;
    public bool IsJumping { get; private set; } = false;
    public bool IsAttacking { get; private set; } = false;

    public event Action OnJumpStarted;
    public event Action OnAttackStarted;
    void Start()
    {
        
    }


    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            var dir = ctx.ReadValue<Vector2>();
            MoveDir = new Vector2(dir.x, 0f);
        }
        else if (ctx.canceled)
        {
            MoveDir = Vector3.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            IsJumping = true;
            OnJumpStarted?.Invoke();
        }
        else if (ctx.canceled)
        {
            IsJumping = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            IsAttacking = true;
            OnAttackStarted?.Invoke();
        }
        else if (ctx.canceled)
        {
            IsAttacking = false;
        }
    }
}
