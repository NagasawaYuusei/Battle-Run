using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallRun : MonoBehaviour
{
    [Header("UseScript")]
    [SerializeField, Tooltip("プレイヤーカメラのトランスフォーム")] Transform m_playerCamera;

    [Header("WallRunPlayerStates")]
    [SerializeField, Tooltip("ウォールラン中のジャンプパワー")] float m_wallRunJumpForce;

    [Header("Camera")]
    [SerializeField, Tooltip("通常時視野角")] float m_fov;
    [SerializeField, Tooltip("視野角変化までの偏移時間")] float m_wallRunfovTime;
    [SerializeField, Tooltip("ウォールラン時のカメラ角度")] float m_camTilt;
    [SerializeField, Tooltip("角度変化までの時間偏移")] float m_camTiltTime;
    [Tooltip("角度状態")]float m_tilt;

    //[Header("IsWall")]
    bool m_wallLeft = false; //左の壁かどうか
    bool m_wallRight = false; //右の壁かどうか
    RaycastHit m_leftWallHit; //あたった左の壁
    RaycastHit m_rightWallHit; //あたった右の壁
    [SerializeField] float m_wallDistance = 0.5f; //判定距離
    [SerializeField] float m_minimumJumpHeight = 1.5f; //ジャンプした際の判定距離
    [SerializeField] LayerMask m_wall; //ランウォールのレイヤー

    //[Header("Input")]
    bool m_isWallJump;

    //[Header("Ather")]
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
        m_wallLeft = Physics.Raycast(transform.position, -m_playerCamera.right, out m_leftWallHit, m_wallDistance, m_wall);
        m_wallRight = Physics.Raycast(transform.position, m_playerCamera.right, out m_rightWallHit, m_wallDistance, m_wall);
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
        UseCamera.CVC.m_Lens.FieldOfView = Mathf.Lerp(UseCamera.CVC.m_Lens.FieldOfView, m_fov + 20, m_wallRunfovTime * Time.deltaTime);

        if(m_wallLeft)
        {
            m_tilt = Mathf.Lerp(m_tilt, -m_camTilt, m_camTiltTime * Time.deltaTime);
        }
        else if(m_wallRight)
        {
            m_tilt = Mathf.Lerp(m_tilt, m_camTilt, m_camTiltTime * Time.deltaTime);
        }

        if (m_isWallJump)
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
            m_isWallJump = false;
        }
    }

    void StopWallRun()
    {
        m_rb.useGravity = true;
        UseCamera.CVC.m_Lens.FieldOfView = Mathf.Lerp(UseCamera.CVC.m_Lens.FieldOfView, m_fov , m_wallRunfovTime * Time.deltaTime);
        m_tilt = Mathf.Lerp(m_tilt, 0, m_camTiltTime * Time.deltaTime);
    }

    void Camera()
    {
        UseCamera.CVC.m_Lens.Dutch = m_tilt;
    }

    /// <summary>ジャンプインプットシステム</summary>
    public void PlayerWallJump(InputAction.CallbackContext context)
    {
        m_isWallJump = context.ReadValueAsButton();
    }
}
