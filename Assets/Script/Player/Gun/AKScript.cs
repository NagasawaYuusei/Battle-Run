using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class AKScript : ShoterBase
{
    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera m_playerCamera;
    [SerializeField] CinemachineVirtualCamera m_akCamera;
    [SerializeField] float m_shotTime;
    CinemachinePOV m_pPOV;
    CinemachinePOV m_aPOV;
    CinemachineVirtualCamera m_nowCinema;

    void Start()
    {
        Setup();
    }

    void Update()
    {
        State();
        Fire();
    }

    void Setup()
    {
        m_pPOV = m_playerCamera.GetCinemachineComponent<CinemachinePOV>();
        m_aPOV = m_akCamera.GetCinemachineComponent<CinemachinePOV>();
        m_nowCinema = m_playerCamera;
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
            m_nowCinema = m_akCamera;
        }
        else
        {
            m_pPOV.m_HorizontalAxis.Value = m_aPOV.m_HorizontalAxis.Value;
            m_pPOV.m_VerticalAxis.Value = m_aPOV.m_VerticalAxis.Value;
            m_playerCamera.Priority = 10;
            m_akCamera.Priority = 0;
            m_nowCinema = m_playerCamera;
        }
    }

   
}
