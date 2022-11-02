using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// ブリンクを管理するクラス
/// </summary>
public class Blink : MonoBehaviour
{
    [Tooltip("ブリンクしているか")] bool m_isBlink;
    [Tooltip("プレイヤークラス")] Player m_player;
    [Tooltip("ブリンクのスピード"), SerializeField] float m_blinkSpeed = 36;
    [Tooltip("スローモーション中のスピード"), SerializeField] float m_slowMotionSpeed = 0.2f;
    [Tooltip("ノイズのImage"), SerializeField] Image m_slowUI;
    [Tooltip("ノイズUIのα値")] float m_uiA;

    void Start()
    {
        SetUp();
    }

    void Update()
    {
        BlinkMovement();
    }

    /// <summary>
    /// Startでのセットアップ
    /// </summary>
    void SetUp()
    {
        m_uiA = 0;
        m_player = GetComponent<Player>();
    }

    /// <summary>
    /// ブリンク処理
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
    /// ブリンクのインプット
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
