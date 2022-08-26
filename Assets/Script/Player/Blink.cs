using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blink : MonoBehaviour
{
    Rigidbody m_rb;
    bool m_isBlink;
    Player _player;
    [SerializeField] float m_blinkSpeed = 36;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
    }

    void Update()
    {
        if(m_isBlink)
        {
            
        }
    }

    public void PlayerBlink(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _player.ChangePlayerSpeed(m_blinkSpeed);
            m_isBlink = true;
        }

        if (context.canceled)
        {
            _player.DefaultSpeedReset();
            m_isBlink = false;
        }
    }
}
