using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] int m_enemyHP;

    public int EnemyHP
    {
        get
        {
            return m_enemyHP;
        }
        set
        {
            m_enemyHP = value;
        }
    }

    void Update()
    {
        HP();
        Death();
    }

    void HP()
    {
        print(m_enemyHP);
    }

    void Death()
    {
        if(m_enemyHP <= 0)
        {
            print("Death");
            Destroy(gameObject);
        }
    }
}
