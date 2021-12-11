using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKScript : ShoterBase
{
    [SerializeField] float m_damagePerSecond;
    
    void FixedUpdate()
    {
        Fire();
    }

    protected override void FireMethod()
    {
        m_time += Time.deltaTime;
        if(m_time >=  m_damagePerSecond && m_isFire)
        {
            if(IsCollision())
            {
                EnemyScript m_es = m_enemy.collider.GetComponent<EnemyScript>();
                m_es.EnemyHP -= m_damage;
            }
        }
    }

    protected override void FireSecondMethod()
    {
        
    }
}
