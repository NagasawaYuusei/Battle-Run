using UnityEngine;
using Cinemachine;

public class AKScript : ShoterBase
{
    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera m_playerCamera;
    [SerializeField] CinemachineVirtualCamera m_akCamera;
    CinemachinePOV m_pPOV;
    CinemachinePOV m_aPOV;
    CinemachinePOV m_nowPOV;

    void Start()
    {
        m_pPOV = m_playerCamera.GetCinemachineComponent<CinemachinePOV>();
        m_aPOV = m_akCamera.GetCinemachineComponent<CinemachinePOV>();
        m_nowPOV = m_pPOV;
    }

    void Update()
    {
        State();
        Fire();
    }

    void State()
    {
        m_atkNextTime += Time.deltaTime;
    }

    protected override void FireMethod()
    {
        if(m_coolDownTime >=  m_damagePerSecond && m_isFire)
        {
            if(IsCollision())
            {
                EnemyScript m_es = m_enemy.collider.GetComponent<EnemyScript>();
                m_es.HP(m_damage);
            }
            Recoil(m_nowPOV);
            m_coolDownTime = 0;
        }
    }

    protected override void FireSecondMethod(bool on)
    {
        if(on)
        {
            m_aPOV.m_HorizontalAxis.Value = m_pPOV.m_HorizontalAxis.Value;
            m_aPOV.m_VerticalAxis.Value = m_pPOV.m_VerticalAxis.Value;
            m_akCamera.Priority = 10;
            m_playerCamera.Priority = 0;
            m_nowPOV = m_aPOV;
        }
        else
        {
            m_pPOV.m_HorizontalAxis.Value = m_aPOV.m_HorizontalAxis.Value;
            m_pPOV.m_VerticalAxis.Value = m_aPOV.m_VerticalAxis.Value;
            m_playerCamera.Priority = 10;
            m_akCamera.Priority = 0;
            m_nowPOV = m_pPOV;
        }
    }

    protected override void Recoil(CinemachinePOV pov)
    {
        if(m_atkNextTime > m_coolDownTime)
        {
            m_atkCount = 0;
            m_atkNextTime = 0;
        }
        else
        {
            m_atkCount++;
            m_atkNextTime = 0;
        }
        pov.m_VerticalAxis.Value -= (m_recoilValue + (m_recoilUpValue * m_atkCount)); 
    }
}
