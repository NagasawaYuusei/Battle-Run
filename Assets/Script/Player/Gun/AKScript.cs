using UnityEngine;
using Cinemachine;

public class AKScript : ShoterBase
{
    [SerializeField] CinemachineVirtualCamera m_playerCamera;
    [SerializeField] CinemachineVirtualCamera m_akCamera;
    void FixedUpdate()
    {
        Fire();
    }

    protected override void FireMethod()
    {
        if(m_time >=  m_damagePerSecond && m_isFire)
        {
            if(IsCollision())
            {
                EnemyScript m_es = m_enemy.collider.GetComponent<EnemyScript>();
                m_es.HP(m_damage);
            }
            m_time = 0;
        }
    }

    protected override void FireSecondMethod(bool on)
    {
        if(on)
        {
            CinemachinePOV pPOV = m_playerCamera.GetCinemachineComponent<CinemachinePOV>();
            CinemachinePOV aPOV = m_akCamera.GetCinemachineComponent<CinemachinePOV>();
            aPOV.m_HorizontalAxis.Value = pPOV.m_HorizontalAxis.Value;
            aPOV.m_VerticalAxis.Value = pPOV.m_VerticalAxis.Value;
            m_akCamera.Priority = 10;
            m_playerCamera.Priority = 0;
        }
        else
        {
            CinemachinePOV pPOV = m_playerCamera.GetCinemachineComponent<CinemachinePOV>();
            CinemachinePOV aPOV = m_akCamera.GetCinemachineComponent<CinemachinePOV>();
            pPOV.m_HorizontalAxis.Value = aPOV.m_HorizontalAxis.Value;
            pPOV.m_VerticalAxis.Value = aPOV.m_VerticalAxis.Value;
            m_playerCamera.Priority = 10;
            m_akCamera.Priority = 0;
        }
    }
}
