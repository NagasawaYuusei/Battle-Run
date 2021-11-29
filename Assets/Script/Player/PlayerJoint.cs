using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoint : MonoBehaviour
{
    Rigidbody m_rb;
    PlayerController m_pc;
    bool m_isJump;
    float m_grappleSpeed = 15;
    float m_grappleUpSpeed;

    public Vector3 m_connectedAnchor;

    void Start()
    {
        SetUp();
        PlayerFirstMove();
    }

    void SetUp()
    {
        m_rb = GetComponent<Rigidbody>();
        m_pc = GetComponent<PlayerController>();
        if(m_rb.velocity.y > 0)
        {
            m_isJump = true;
        }
    }

    void PlayerFirstMove()
    {
        m_rb.velocity = transform.up * m_pc.JumpPower / 2;
    }

    void Update()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        Vector3 vec = m_connectedAnchor - transform.position;
        //if (!m_isJump)
        //{
            m_rb.velocity = (vec.normalized * m_grappleSpeed);
        //}
        //else
        //{

        //}
    }
}
