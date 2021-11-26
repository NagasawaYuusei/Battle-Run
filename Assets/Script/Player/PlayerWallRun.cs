using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerWallRun : MonoBehaviour
{
    [SerializeField] Transform m_player;

    [SerializeField] float m_wallDistance = 0.5f;
    [SerializeField] float m_minimumJumpHeight = 1.5f;

    [SerializeField] float m_wallRunGravity;
    [SerializeField] float m_wallRunJumpForce;

    [SerializeField] LayerMask m_wall;

    [SerializeField] CinemachineVirtualCamera m_cam;
    [SerializeField] float m_fov;
    [SerializeField] float m_wallRunfovTime;
    [SerializeField] float m_camTilt;
    [SerializeField] float m_camTiltTime;
    float m_tilt;

    bool m_wallLeft = false;
    bool m_wallRight = false;

    RaycastHit m_leftWallHit;
    RaycastHit m_rightWallHit;


    Rigidbody m_rb;

    public float Tilt
    {
        get
        {
            return m_tilt;
        }
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, m_minimumJumpHeight);
    }

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void CheckWall()
    {
        m_wallLeft = Physics.Raycast(transform.position, -m_player.right, out m_leftWallHit, m_wallDistance, m_wall);
        m_wallRight = Physics.Raycast(transform.position, m_player.right, out m_rightWallHit, m_wallDistance, m_wall);
    }

    void Update()
    {
        CheckWall();
        WallRunReady();
        Camera();
    }

    void WallRunReady()
    {
        if (CanWallRun())
        {
            if (m_wallLeft)
            {
                StartWallRun();
            }
            else if (m_wallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }

    void StartWallRun()
    {
        m_rb.useGravity = false;
        m_rb.AddForce(Vector3.down * m_wallRunGravity, ForceMode.Force);
        m_cam.m_Lens.FieldOfView = Mathf.Lerp(m_cam.m_Lens.FieldOfView, m_fov + 20, m_wallRunfovTime * Time.deltaTime);

        if(m_wallLeft)
        {
            m_tilt = Mathf.Lerp(m_tilt, -m_camTilt, m_camTiltTime * Time.deltaTime);
        }
        else if(m_wallRight)
        {
            m_tilt = Mathf.Lerp(m_tilt, m_camTilt, m_camTiltTime * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if(m_wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + m_leftWallHit.normal;
                m_rb.velocity = new Vector3(m_rb.velocity.x, 0, m_rb.velocity.z);
                m_rb.AddForce(wallRunJumpDirection * m_wallRunJumpForce * 100, ForceMode.Force);
            }
            else if(m_wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + m_rightWallHit.normal;
                m_rb.velocity = new Vector3(m_rb.velocity.x, 0, m_rb.velocity.z);
                m_rb.AddForce(wallRunJumpDirection * m_wallRunJumpForce * 100, ForceMode.Force);
            }

        }
    }

    void StopWallRun()
    {
        m_rb.useGravity = true;
        m_cam.m_Lens.FieldOfView = Mathf.Lerp(m_cam.m_Lens.FieldOfView, m_fov , m_wallRunfovTime * Time.deltaTime);
        m_tilt = Mathf.Lerp(m_tilt, 0, m_camTiltTime * Time.deltaTime);
    }

    void Camera()
    {
        m_cam.m_Lens.Dutch = m_tilt;
    }
}
