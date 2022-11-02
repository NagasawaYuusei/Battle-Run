using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// �u�����N���Ǘ�����N���X
/// </summary>
public class Blink : MonoBehaviour
{
    [Tooltip("�u�����N���Ă��邩")] bool m_isBlink;
    [Tooltip("�v���C���[�N���X")] Player m_player;
    [Tooltip("�u�����N�̃X�s�[�h"), SerializeField] float m_blinkSpeed = 36;
    [Tooltip("�X���[���[�V�������̃X�s�[�h"), SerializeField] float m_slowMotionSpeed = 0.2f;
    [Tooltip("�m�C�Y��Image"), SerializeField] Image m_slowUI;
    [Tooltip("�m�C�YUI�̃��l")] float m_uiA;

    void Start()
    {
        SetUp();
    }

    void Update()
    {
        BlinkMovement();
    }

    /// <summary>
    /// Start�ł̃Z�b�g�A�b�v
    /// </summary>
    void SetUp()
    {
        m_uiA = 0;
        m_player = GetComponent<Player>();
    }

    /// <summary>
    /// �u�����N����
    /// </summary>
    void BlinkMovement()
    {
        if (GameManager.Instance.IsChangeTime)
        {

            if (m_uiA < 1)
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
        Color color = new Color(m_slowUI.color.r, m_slowUI.color.g, m_slowUI.color.b, m_uiA);
        m_slowUI.color = color;
    }

    /// <summary>
    /// �u�����N�̃C���v�b�g
    /// </summary>
    /// <param name="context"></param>
    public void PlayerBlink(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_player.ChangePlayerSpeed(m_blinkSpeed);
            m_isBlink = true;
            GameManager.Instance.TimerChange(true);
            Time.timeScale = m_slowMotionSpeed;
        }

        if (context.canceled)
        {
            m_player.DefaultSpeedReset();
            m_isBlink = false;
            GameManager.Instance.TimerChange(false);
            Time.timeScale = 1;
            
        }
    }
}
