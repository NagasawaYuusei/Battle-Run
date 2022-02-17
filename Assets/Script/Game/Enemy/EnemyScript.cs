using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Tooltip("プレイヤーからくらった攻撃")] bool m_enemyDamage;
    PhaseManager m_pm;
    [SerializeField] GameObject m_damage;

    /// <summary>カプセル化　プレイヤーからくらった攻撃</summary>
    public bool EnemyDamage
    {
        set
        {
            m_enemyDamage = value;
        }
    }

    void Awake()
    {
        m_pm = GameObject.FindObjectOfType<PhaseManager>();    
    }

    void Update()
    {
        Death();
    }

    /// <summary>敵死亡時処理</summary>
    void Death()
    {
        if(m_enemyDamage)
        {
            Instantiate(m_damage);
            m_pm.EnemyKnock(this.gameObject);
            m_enemyDamage = false;
        }
    }
}
