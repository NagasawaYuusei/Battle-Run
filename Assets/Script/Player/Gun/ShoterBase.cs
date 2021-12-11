using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShoterBase : MonoBehaviour
{
    protected bool m_isFire;
    protected bool m_isSingleFire;
    protected bool m_isFireSecond;
    [SerializeField] bool m_singleType;
    [SerializeField] Transform m_cameraTransform;
    [SerializeField] float m_distance = 200f;
    [SerializeField] LayerMask m_enemyLayer;
    [SerializeField] protected int m_damage;
    protected RaycastHit m_enemy;
    protected float m_time;

    public void PlayerFire(InputAction.CallbackContext context)
    {
        m_isFire = context.ReadValueAsButton();
    }

    public void PlayerSingleFire(InputAction.CallbackContext context)
    {
        m_isSingleFire = context.ReadValueAsButton();
    }

    public void PlayerFireSecond(InputAction.CallbackContext context)
    {
        m_isFireSecond = context.ReadValueAsButton();
    }

    protected void Fire()
    {
        if(m_isFire && !m_singleType)
        {
            FireMethod();
        }

        if(m_isFireSecond)
        {
            FireSecondMethod();
        }

        if(m_isSingleFire && m_singleType)
        {
            SingleFireMethod();
        }
    }

    protected virtual void FireMethod()
    {

    }

    protected virtual void FireSecondMethod()
    {

    }

    protected virtual void SingleFireMethod()
    {

    }

    protected bool IsCollision()
    {
        bool isCollision = Physics.Raycast(m_cameraTransform.position, m_cameraTransform.forward, out m_enemy, m_distance, m_enemyLayer);
        return isCollision;
    }
}
