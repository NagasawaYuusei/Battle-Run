using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] int m_enemyHP;
    int nowHP;

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

    void Start()
    {
        nowHP = m_enemyHP + 1;
    }
    void Update()
    {
        HP();
        Death();
    }

    void HP()
    {
        if(m_enemyHP < nowHP)
        {
            print(m_enemyHP);
            nowHP--;
        }
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
