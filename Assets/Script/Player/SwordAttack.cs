using UnityEngine;
using UnityEngine.InputSystem;

public class SwordAttack : MonoBehaviour
{
    [Tooltip("アタックインプットシステム")] bool m_isAttack;
    [Tooltip("前回のアタックから経過している時間")] float m_isAttackTime;
    [Tooltip("アタックのインターバル"), SerializeField] float m_isAttackInterval;

    void Update()
    {
        State();
    }

    /// <summary>状態</summary>
    void State()
    {
        m_isAttackTime += Time.deltaTime;
    }

    /// <summary>コライダー接触中のアップデート</summary>
    void OnTriggerStay(Collider other)
    {
        if(m_isAttack && m_isAttackTime >= m_isAttackInterval)
        {
            m_isAttackTime = 0;
            //nullチェック
            other.GetComponent<EnemyScript>()!.EnemyDamage = true;
        }
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
