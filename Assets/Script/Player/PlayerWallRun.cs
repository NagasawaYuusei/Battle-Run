using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallRun : MonoBehaviour
{
    [Header("UseScript")]
    [SerializeField, Tooltip("プレイヤーカメラのトランスフォーム")] Transform m_playerCamera;

    [Header("WallRunPlayerStates")]
    [SerializeField, Tooltip("ウォールラン中のジャンプパワー")] float m_wallRunJumpForce;

    [Header("Camera")]
    [Tooltip("FOV初期値")] float m_firstFOV;
    [SerializeField, Tooltip("視野角変化までの偏移時間")] float m_wallRunfovTime;
    [SerializeField, Tooltip("ウォールラン時のカメラ角度")] float m_camTilt;
    [SerializeField, Tooltip("角度変化までの時間偏移")] float m_camTiltTime;
    [Tooltip("角度状態")] float m_tilt;

    [Header("IsWall")]
    [Tooltip("左の壁か")]bool m_wallLeft = false;
    [Tooltip("右の壁か")]bool m_wallRight = false;
    [Tooltip("当たった左の壁")]RaycastHit m_leftWallHit;
    [Tooltip("当たった右の壁")]RaycastHit m_rightWallHit;
    [SerializeField, Tooltip("判定距離")] float m_wallDistance = 0.5f;
    [SerializeField, Tooltip("ジャンプした際の判定距離")] float m_minimumJumpHeight = 1.5f;
    [SerializeField, Tooltip("ランウォールのレイヤー")] LayerMask m_wall;

    //[Header("Input")]
    [Tooltip("ウォールジャンプインプットシステム")]bool m_isWallJump;

    //[Header("Ather")]
    [Tooltip("Rigidbody")]Rigidbody m_rb;

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
        m_firstFOV = UseCamera.CVC.m_Lens.FieldOfView;
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
        WallRunReady();
        Camera();
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
        UseCamera.CVC.m_Lens.FieldOfView = Mathf.Lerp(UseCamera.CVC.m_Lens.FieldOfView, m_firstFOV + 20, m_wallRunfovTime * Time.deltaTime);

        if (m_wallLeft)
        {
            m_tilt = Mathf.Lerp(m_tilt, -m_camTilt, m_camTiltTime * Time.deltaTime);
        }
        else if (m_wallRight)
        {
            m_tilt = Mathf.Lerp(m_tilt, m_camTilt, m_camTiltTime * Time.deltaTime);
        }

        if (m_isWallJump)
        {
            if (m_wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + m_leftWallHit.normal;
                m_rb.velocity = new Vector3(m_rb.velocity.x, 0, m_rb.velocity.z);
                m_rb.AddForce(wallRunJumpDirection * m_wallRunJumpForce * 100, ForceMode.Force);
            }
            else if (m_wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + m_rightWallHit.normal;
                m_rb.velocity = new Vector3(m_rb.velocity.x, 0, m_rb.velocity.z);
                m_rb.AddForce(wallRunJumpDirection * m_wallRunJumpForce * 100, ForceMode.Force);
            }
            m_isWallJump = false;
        }
    }

    /// <summary>NotWallRun処理</summary>
    void StopWallRun()
    {
        UseCamera.CVC.m_Lens.FieldOfView = Mathf.Lerp(UseCamera.CVC.m_Lens.FieldOfView, m_firstFOV, m_wallRunfovTime * Time.deltaTime);
        m_rb.useGravity = true;
        m_tilt = Mathf.Lerp(m_tilt, 0, m_camTiltTime * Time.deltaTime);
    }

    /// <summary>Cameraの角度</summary>
    void Camera()
    {
        UseCamera.CVC.m_Lens.Dutch = m_tilt;
    }

    /// <summary>WallRunジャンプインプットシステム</summary>
    public void PlayerWallJump(InputAction.CallbackContext context)
    {
        m_isWallJump = context.ReadValueAsButton();
    }
}
