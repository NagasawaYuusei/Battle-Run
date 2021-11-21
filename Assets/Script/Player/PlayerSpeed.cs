using UnityEngine;

public class PlayerSpeed : MonoBehaviour
{
    PlayerController m_pc;
    float m_speed = 0;
    float m_firstWalk;
    float m_firstRun;
    bool m_isFaster;
    bool m_slide;

    [Header("Velocity")]
    /// <summary>プレイヤーの歩き時最大スピード</summary>
    [SerializeField] float m_maxWalkSpeed = 8;
    /// <summary>プレイヤーの走り時最大スピード</summary>
    [SerializeField] float m_maxRunSpeed = 12;
    /// <summary>プレイヤーの１フレームごとの歩くスピード</summary>
    [SerializeField] float m_walkSpeed = 0.5f;
    /// <summary>プレイヤーの１フレームごとの走るスピード</summary>
    [SerializeField] float m_runSpeed = 0.13f;

    public bool Slide
    {
        get
        {
            return m_slide;
        }
        set
        {
            m_slide = value;
        }
    }

    void Start()
    {
        SetUp();
    }

    void SetUp()
    {
        m_pc = GetComponent<PlayerController>();
        m_firstWalk = m_maxWalkSpeed;
        m_firstRun = m_maxRunSpeed;
    }
    void FixedUpdate()//60f
    {
        MoveSpeed();
        DownSpeed();
        m_pc.NowSpeed = m_speed;
    }

    void MoveSpeed()
    {
        if ( m_pc.Vertical > 0 && !m_isFaster)
        {
            if (m_pc.Dash && m_maxWalkSpeed > m_speed)
            {
                m_speed += m_walkSpeed;
            }
            else if (m_pc.Dash && m_maxWalkSpeed <= m_speed)
            {
                m_speed += m_runSpeed;
                if (m_maxRunSpeed < m_speed)
                {
                    m_speed = m_maxRunSpeed;
                }
            }
            else if (!m_pc.Dash && m_maxWalkSpeed > m_speed)
            {
                m_speed += m_walkSpeed;
                if (m_maxWalkSpeed < m_speed)
                {
                    m_speed = m_maxWalkSpeed;
                }
            }
        }
        else if(m_pc.Horizontal > 0 || m_pc.Horizontal < 0 || m_pc.Vertical < 0 && !m_isFaster)
        {
            m_speed += m_walkSpeed;
            if (m_maxWalkSpeed < m_speed)
            {
                m_speed = m_maxWalkSpeed;
            }
        }
        else if (!m_isFaster)
        {
            m_speed -= 0.8f;
            if(m_speed <= 0)
            {
                m_speed = 0;
            }
        }
    }

    void DownSpeed()
    {
        if(m_pc.IsDown)
        {
            if(m_speed > m_maxWalkSpeed)
            {
                m_isFaster = true;
                if (m_slide)
                {
                    m_speed = m_speed * 1.5f;
                    m_slide = false;
                }
                m_speed -= 0.2f;
            }
            else
            {
                m_speed = 4;
                m_maxWalkSpeed = 4;
                m_maxRunSpeed = 4;
                m_isFaster = false;
            }
        }
        else
        {
            m_maxWalkSpeed = m_firstWalk;
            m_maxRunSpeed = m_firstRun;
            m_isFaster = false;
        }
    }
}
