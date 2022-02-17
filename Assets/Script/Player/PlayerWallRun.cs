using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerWallRun : MonoBehaviour
{
    [Header("UseScript")]
    [SerializeField, Tooltip("プレイヤーカメラのトランスフォーム")] Transform m_playerCamera;

    [Header("WallRunPlayerStates")]
    [SerializeField, Tooltip("ウォールラン中のジャンプパワー")] float m_wallRunJumpForce;
    [SerializeField] float m_wallRunSpeed;
    [SerializeField] float m_movementMultiplier;

    [Header("Camera")]
    [SerializeField, Tooltip("ウォールラン時のカメラ角度")] float m_camTilt;
    [SerializeField, Tooltip("角度変化までの時間偏移")] float m_camTiltTime;
    [Tooltip("角度状態")] float m_tilt;

    [Header("IsWall")]
    [Tooltip("左の壁か")]bool m_wallLeft = false;
    [Tooltip("右の壁か")]bool m_wallRight = false;
    [Tooltip("当たった左の壁")]RaycastHit m_leftWallHit = default;
    [Tooltip("当たった右の壁")]RaycastHit m_rightWallHit = default;
    [SerializeField, Tooltip("判定距離")] float m_wallDistance = 0.5f;
    [SerializeField, Tooltip("ジャンプした際の判定距離")] float m_minimumJumpHeight = 1.5f;
    [SerializeField, Tooltip("ランウォールのレイヤー")] LayerMask m_wall;
    bool m_isWallRun = false;
    bool m_wallRunStart = false;

    public bool IsWallRun => m_isWallRun;

    //[Header("Ather")]
    [Tooltip("Rigidbody")]Rigidbody m_rb;

    /// <summary>WallRunジャンプインプットシステム</summary>
    public void PlayerWallJump(InputAction.CallbackContext context)
    {
        if (context.started && m_isWallRun)
        {
            Vector3 wallRunJumpDirection = Vector3.zero;
            if (m_wallLeft)
            {
                wallRunJumpDirection =  m_leftWallHit.normal + m_leftWallHit.normal + transform.up;
            }
            else if (m_wallRight)
            {
                wallRunJumpDirection =  m_rightWallHit.normal + m_rightWallHit.normal + transform.up;
            }
            m_rb.AddForce(wallRunJumpDirection.normalized * m_wallRunJumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// WallRunできるかどうか
    /// </summary>
    /// <returns>
    /// 可能 true
    /// 不可能 false
    /// </returns>
    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, m_minimumJumpHeight);
    }

    void Start()
    {
        Setup();
    }

    /// <summary>最初のセットアップ</summary>
    void Setup()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    /// <summary>どっち方向の壁か　どの壁か</summary>
    void CheckWall()
    {
        m_wallLeft = Physics.Raycast(transform.position, -m_playerCamera.right, out m_leftWallHit, m_wallDistance, m_wall);
        m_wallRight = Physics.Raycast(transform.position, m_playerCamera.right, out m_rightWallHit, m_wallDistance, m_wall);
    }

    void Update()
    {
        CheckWall();
        CameraTilt();
    }

    void FixedUpdate()
    {
        WallRunReady();
    }

    /// <summary>WallRunの準備フェイズ</summary>
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

    /// <summary>WallRun処理</summary>
    void StartWallRun()
    {
        m_rb.useGravity = false;
        if(!m_wallRunStart)
            StartCoroutine(WallRun());
        Vector3 dir = Vector3.zero;
        if (m_wallLeft)
        {
            if(m_playerCamera.transform.forward.z >= 0)
            {
                dir = m_leftWallHit.collider.gameObject.transform.forward;
            }
            else if(m_playerCamera.transform.forward.z < 0)
            {
                dir = -m_leftWallHit.collider.gameObject.transform.forward;
            }
            if (m_playerCamera.transform.forward.z >= 0)
            {
                dir = m_leftWallHit.collider.gameObject.transform.forward;
            }
            else if (m_playerCamera.transform.forward.z < 0)
            {
                dir = -m_leftWallHit.collider.gameObject.transform.forward;
            }
            m_tilt = Mathf.Lerp(m_tilt, -m_camTilt, m_camTiltTime * Time.deltaTime);
            Debug.DrawRay(transform.position, transform.up + m_leftWallHit.normal + m_leftWallHit.normal * 10, Color.red);
        }
        else if (m_wallRight)
        {
            if (m_playerCamera.transform.forward.z >= 0)
            {
                dir = m_rightWallHit.collider.gameObject.transform.forward;
            }
            else if(m_playerCamera.transform.forward.z < 0)
            {
                dir = -m_rightWallHit.collider.gameObject.transform.forward;
            }
            m_tilt = Mathf.Lerp(m_tilt, m_camTilt, m_camTiltTime * Time.deltaTime);
            Debug.DrawRay(transform.position, transform.up + m_rightWallHit.normal + m_rightWallHit.normal * 10, Color.red);
        }
        dir.y = 0;
        m_rb.AddForce(dir.normalized * m_wallRunSpeed * m_movementMultiplier,ForceMode.Acceleration);
    }

    IEnumerator WallRun()
    {
        m_wallRunStart = true;
        m_rb.velocity = new Vector3(m_rb.velocity.x, 0, m_rb.velocity.z);
        yield return new WaitForSeconds(0.2f);
        m_isWallRun = true;
    }

    /// <summary>NotWallRun処理</summary>
    void StopWallRun()
    {
        m_rb.useGravity = true;
        m_tilt = Mathf.Lerp(m_tilt, 0, m_camTiltTime * Time.deltaTime);
        m_isWallRun = false;
        m_wallRunStart = false;
    }

    /// <summary>Cameraの角度</summary>
    void CameraTilt()
    {
        UseCamera.CVC.m_Lens.Dutch = m_tilt;
    }
}
