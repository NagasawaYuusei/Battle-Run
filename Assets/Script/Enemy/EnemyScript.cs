using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Tooltip("�v���C���[���炭������U��")] bool m_enemyDamage;

    /// <summary>�J�v�Z�����@�v���C���[���炭������U��</summary>
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

    /// <summary>�G���S������</summary>
    void Death()
    {
        if(m_enemyDamage)
        {
            Destroy(gameObject);
        }
    }
}
