using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Tooltip("プレイヤーからくらった攻撃")] bool m_enemyDamage;

    /// <summary>カプセル化　プレイヤーからくらった攻撃</summary>
    public bool EnemyDamage
    {
        set
        {
            m_enemyDamage = value;
        }
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
            Destroy(gameObject);
        }
    }
}
