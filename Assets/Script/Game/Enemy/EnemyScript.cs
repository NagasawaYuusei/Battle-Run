using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Tooltip("�v���C���[���炭������U��")] bool m_enemyDamage;
    PhaseManager m_pm;
    [SerializeField] GameObject m_damage;

    /// <summary>�J�v�Z�����@�v���C���[���炭������U��</summary>
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

    /// <summary>�G���S������</summary>
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
