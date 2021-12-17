using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShoterBase : MonoBehaviour
{
    protected bool m_isFire;
    protected bool m_isSingleFire;
    [SerializeField] Transform m_cameraTransform;
    [SerializeField] float m_distance = 200f;
    [SerializeField] LayerMask m_enemyLayer;
    [SerializeField] protected int m_damage;
    [SerializeField] protected float m_damagePerSecond;
    protected RaycastHit m_enemy;
    protected float m_time;

    public void PlayerFire(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            m_isFire = true;
        }

        if(context.canceled)
        {
            m_isFire = false;
        }
    }

    public void PlayerSingleFire(InputAction.CallbackContext context)
    {
        m_isSingleFire = context.ReadValueAsButton();
    }

    public void PlayerFireSecond(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            FireSecondMethod(true);
        }
        
        if(context.canceled)
        {
            FireSecondMethod(false);
        }
    }

    protected void Fire()
    {
        m_time += Time.deltaTime;
        if (m_isFire)
        {
            FireMethod();
        }
    }

    protected virtual void FireMethod()
    {

    }

    protected virtual void FireSecondMethod(bool on)
    {

    }

    protected bool IsCollision()
    {
        bool isCollision = Physics.Raycast(m_cameraTransform.position, m_cameraTransform.forward, out m_enemy, m_distance, m_enemyLayer);
        return isCollision;
    }
}
