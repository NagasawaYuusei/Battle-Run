using UnityEngine;
using UnityEngine.InputSystem;

public class SwordAttack : MonoBehaviour
{
    [Tooltip("アタックインプットシステム")] bool m_isAttack;
    [Tooltip("前回のアタックから経過している時間")] float m_attackTime;
    [Tooltip("アタックのインターバル"), SerializeField] float m_attackInterval;
    [Tooltip("当たった敵")]RaycastHit m_inEnemyRay;
    [Tooltip("アタックの距離"), SerializeField] float m_attackLength;

    void Update()
    {
        State();
        Damage();
    }

    /// <summary>状態</summary>
    void State()
    {
        m_attackTime += Time.deltaTime;
    }

    /// <summary>アタックする関数</summary>
    void Damage()
    {
        if (m_isAttack && m_attackTime >= m_attackInterval)
        {
            if (IsAttack())
            {
                m_inEnemyRay.collider.GetComponent<EnemyScript>().EnemyDamage = true;
            }
            m_attackTime = 0;
        }
        m_isAttack = false;
    }

    /// <summary>
    /// アタック判定
    /// </summary>
    /// <returns>
    /// 敵がいる true 
    /// 敵がいない false
    /// </returns>
    public bool IsAttack()
    {
        if (Physics.Raycast(transform.position, transform.forward, out m_inEnemyRay, m_attackLength))
        {
            if (m_inEnemyRay.collider.GetComponent<EnemyScript>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    /// <summary>アタックインプットシステム</summary>
    public void PlayerSword(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_isAttack = true;
        }
    }
}
