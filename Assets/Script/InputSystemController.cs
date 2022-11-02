using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputSystemController
{
    readonly static InputSystemController Instance = new InputSystemController();
    Vector3 PlayerMoveDir;
    private Action<bool> ScanEvent = default;
    private Action<bool> BlinkEvent = default;
    private Action<bool> WallJumpEvent = default;
    private Action<bool> DownEvent = default;
    private Action<bool> JumpEvent = default;
    private Action<bool> GrappleEvent = default;
    private Action<bool> AttackEvent = default;

    public void SubScanEvent(Action<bool> action)
    {
        ScanEvent += action;
    }

    public void PlayerScan(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ScanEvent.Invoke(true);
        }
    }

    public void PlayerWallJump(InputAction.CallbackContext context)
    {
        //if (context.started && m_isWallRun)
        //{
        //    Vector3 wallRunJumpDirection = Vector3.zero;
        //    if (m_wallLeft)
        //    {
        //        wallRunJumpDirection = m_leftWallHit.normal + m_leftWallHit.normal + transform.up;
        //    }
        //    else if (m_wallRight)
        //    {
        //        wallRunJumpDirection = m_rightWallHit.normal + m_rightWallHit.normal + transform.up;
        //    }
        //    m_rb.AddForce(wallRunJumpDirection.normalized * m_wallRunJumpForce, ForceMode.Impulse);
        //}
    }

    public void PlayerDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DownEvent.Invoke(true);
        }

        if (context.canceled)
        {
            DownEvent.Invoke(false);
        }
    }

    public void PlayerJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpEvent.Invoke(true);
        }
    }

    public void PlayerMove(InputAction.CallbackContext context)
    {
        PlayerMoveDir = context.ReadValue<Vector3>();
    }

    public void PlayerGrap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //PointHit();
        }

        if (context.canceled)
        {
            //m_onGrapple = false;
            GrappleEvent.Invoke(false);
        }
    }

    public void PlayerBlink(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Debug.Log("‰Ÿ‚µ‚½");
            //_player.ChangePlayerSpeed(m_blinkSpeed);
            //m_isBlink = true;
            //GameManager.Instance.TimerChange(true);
            //Time.timeScale = m_slowMotionSpeed;
        }

        if (context.canceled)
        {
            //Debug.Log("—£‚µ‚½");
            //_player.DefaultSpeedReset();
            //m_isBlink = false;
            //GameManager.Instance.TimerChange(false);
            //Time.timeScale = 1;

        }
    }

    public void PlayerSword(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            
        }
    }
}
