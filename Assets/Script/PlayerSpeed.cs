using UnityEngine;

public class PlayerSpeed : MonoBehaviour
{
    Rigidbody m_rb;
    PlayerController m_pc;
    float m_speed = 0;

    [Header("Velocity")]
    /// <summary>プレイヤーの歩き時最大スピード</summary>
    [SerializeField] float m_maxWalkSpeed = 8;
    /// <summary>プレイヤーの走り時最大スピード</summary>
    [SerializeField] float m_maxRunSpeed = 12;
    /// <summary>プレイヤーの１フレームごとの歩くスピード</summary>
    [SerializeField] float m_walkSpeed = 0.5f;
    /// <summary>プレイヤーの１フレームごとの走るスピード</summary>
    [SerializeField] float m_runSpeed = 0.13f;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_pc = GetComponent<PlayerController>();
    }
    void FixedUpdate()//60f
    {
        MoveSpeed();
    }

    void MoveSpeed()
    {
        if ( m_pc.Vertical > 0)
        {
            if (Input.GetButton("Fire3") && m_maxWalkSpeed > m_speed)
            {
                m_speed += m_walkSpeed;
            }
            else if (Input.GetButton("Fire3") && m_maxWalkSpeed <= m_speed)
            {
                m_speed += m_runSpeed;
                if (m_maxRunSpeed < m_speed)
                {
                    m_speed = m_maxRunSpeed;
                }
            }
            else if (!Input.GetButton("Fire3") && m_maxWalkSpeed > m_speed)
            {
                m_speed += m_walkSpeed;
                if (m_maxWalkSpeed < m_speed)
                {
                    m_speed = m_maxWalkSpeed;
                }
            }
        }
        else if(m_pc.Horizontal > 0 || m_pc.Horizontal < 0 || m_pc.Vertical < 0)
        {
            m_speed += m_walkSpeed;
            if (m_maxWalkSpeed < m_speed)
            {
                m_speed = m_maxWalkSpeed;
            }
        }
        else
        {
            m_speed -= 0.8f;
            if(m_speed <= 0)
            {
                m_speed = 0;
            }
        }

        m_pc.NowSpeed = m_speed;
    }
}
