using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody m_rb;
    [SerializeField] float m_moveSpeed;
    [SerializeField] float m_mouseSensitivity = 100f;
    [SerializeField] GameObject m_camera;
    float m_mouseX;
    float m_mouseY;

    void Start()
    {
        SetUp();
    }

    void SetUp()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Mouse(); 
    }

    void Mouse()
    {
        float inputMouseX = Input.GetAxis("Mouse X");
        float inputMouseY = Input.GetAxis("Mouse Y");
        m_mouseX += inputMouseX;
        m_mouseY += inputMouseY;
        Vector3 mouse = new Vector3(-m_mouseY, m_mouseX,0);
        m_camera.transform.localRotation = Quaternion.Euler(mouse);
    }
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(h, m_rb.velocity.y, v);
        m_rb.velocity = dir.normalized * m_moveSpeed;
    }
}
