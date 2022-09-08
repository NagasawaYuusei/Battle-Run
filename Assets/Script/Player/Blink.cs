using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    Rigidbody m_rb;
    bool m_isBlink;
    Player _player;
    [SerializeField] float m_blinkSpeed = 36;
    [SerializeField] float m_slowMotionSpeed = 0.2f;
    [SerializeField] Image _slowUI;
    float m_uiA;

    void Start()
    {
        m_uiA = 0;
        m_rb = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
    }

    void Update()
    {
        if(m_isBlink)
        {
            
            if(m_uiA < 1)
            {
                m_uiA += 0.1f;
            }
        }
        else
        {
            if (m_uiA > 0)
            {
                m_uiA -= 0.1f;
            }
        }
        Color color = new Color(_slowUI.color.r, _slowUI.color.g, _slowUI.color.b, m_uiA);
        _slowUI.color = color;
    }

    public void PlayerBlink(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("‰Ÿ‚µ‚½");
            _player.ChangePlayerSpeed(m_blinkSpeed);
            m_isBlink = true;
            GameManager.Instance.TimerChange(true);
            Time.timeScale = m_slowMotionSpeed;
        }

        if (context.canceled)
        {
            Debug.Log("—£‚µ‚½");
            _player.DefaultSpeedReset();
            m_isBlink = false;
            GameManager.Instance.TimerChange(false);
            Time.timeScale = 1;
        }
    }
}
