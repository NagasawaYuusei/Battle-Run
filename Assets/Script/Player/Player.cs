using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    [Header("Speed")]
    [Tooltip("スピード")] float m_moveSpeed = 4f;
    [Tooltip("通常乗数")] float m_movementMultiplier = 10f;
    [SerializeField, Tooltip("空中乗数")] float m_airMultiplier = 0.4f;
    [SerializeField, Tooltip("最大スピード")] float m_maxMoveSpeed = 4f;
    [Tooltip("しゃがみスピード")] float m_downSpeed;
    [SerializeField, Tooltip("しゃがみ最小スピード")] float m_downMinSpeed;
    [SerializeField, Tooltip("スライディング最大スピード")] float m_downAccelerationMaxSpeed;
    [SerializeField, Tooltip("スライディングのスピード")] float m_slidingTime;

    [Header("Jump")]
    [SerializeField, Tooltip("ジャンプパワー")] float m_jumpPower = 5f;
    [SerializeField, Tooltip("地面レイヤー")] LayerMask m_zimen;
    [SerializeField, Tooltip("Gizmo表示")] bool m_isGizmo = true;
    [Tooltip("設置判定の中点")] Vector3 m_centor;
    [Tooltip("設置判定のサイズ")] Vector3 m_size;
    [SerializeField, Tooltip("中点差分")] Vector3 m_collisionPoint;
    [SerializeField, Tooltip("サイズ差分")] Vector3 m_collisionSize;

    [Header("Drag")]
    [SerializeField, Tooltip("地面時の重力")] float m_groundDrag = 6f;
    [SerializeField, Tooltip("空中時の重力")] float m_airDrag = 2f;

    [Header("Input")]
    [Tooltip("インプットシステムジャンプ")] bool m_isJump;
    [Tooltip("インプットシステム移動")] Vector3 m_moveDir;
    [Tooltip("インプットシステムしゃがみ")] bool m_isDown;

    [Header("Ather")]
    [Tooltip("スロープ時の方向")] Vector3 m_slopeMoveDir;
    [Tooltip("スロープの当たり判定")] RaycastHit m_sloopeHit;
    [Tooltip("Rigidbody")] Rigidbody m_rb;
    [SerializeField, Tooltip("最初のシネマシン")] CinemachineVirtualCamera m_firstCamera;
    PlayerWallRun m_pwr;
    [SerializeField] AudioSource m_as;
    [SerializeField] AudioClip m_runAudio;
    [SerializeField] AudioClip m_jumpAudio;
    bool m_on;

    void Awake()
    {
        AwakeSetUp();
    }

    void Start()
    {
        FirstSetUp();
    }

    void Update()
    {
        State();
        ControlDrag();
        Jump();
        Down();
    }

    void FixedUpdate()
    {
        Move();
    }

    /// <summary>一番最初のセットアップ</summary>
    void AwakeSetUp()
    {
        m_pwr = GetComponent<PlayerWallRun>();
        UseCamera.CVC = m_firstCamera;
    }

    /// <summary>最初のセットアップ</summary>  
    void FirstSetUp()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.freezeRotation = true;
        m_moveSpeed = m_maxMoveSpeed;
    }

    /// <summary>アップデートごとの状態</summary>
    void State()
    {
        //Gizmo差分
        m_centor = transform.position + m_collisionPoint;
        m_size = transform.localScale + m_collisionSize;

        //スロープ時の方向
        m_slopeMoveDir = Vector3.ProjectOnPlane(m_moveDir, m_sloopeHit.normal);
    }

    /// <summary>重力操作</summary>
    void ControlDrag()
    {
        if (IsGround())
        {
            m_rb.drag = m_groundDrag;
        }
        else
        {
            m_rb.drag = m_airDrag;
        }
    }

    /// <summary>ジャンプ</summary>
    void Jump()
    {
        if (m_isJump && IsGround())
        {
            m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
            m_isJump = false;
        }
    }

    /// <summary>しゃがみ</summary>
    void Down()
    {
        Vector3 dir = Camera.main.transform.TransformDirection(m_moveDir);
        dir.y = 0;
        if (m_isDown && IsGround())
        {
            CinemachineTransposer CT = UseCamera.CVC.GetCinemachineComponent<CinemachineTransposer>();
            CT.m_FollowOffset.y = Mathf.Lerp(CT.m_FollowOffset.y, -0.5f, 20 * Time.deltaTime);
            m_downSpeed = Mathf.Lerp(m_downSpeed, m_downMinSpeed, m_slidingTime * Time.deltaTime);
        }
        else
        {
            CinemachineTransposer CT = UseCamera.CVC.GetCinemachineComponent<CinemachineTransposer>();
            CT.m_FollowOffset.y = Mathf.Lerp(CT.m_FollowOffset.y, 1, 20 * Time.deltaTime);
            m_downSpeed = Mathf.Lerp(m_downSpeed, m_downAccelerationMaxSpeed, m_slidingTime * Time.deltaTime);
        }
    }

    /// <summary>移動</summary>
    void Move()
    {
        if (m_pwr.IsWallRun) return;
        Vector3 dir = Camera.main.transform.TransformDirection(m_moveDir);
        dir.y = 0;
        if (IsGround() && !OnSloope() && !m_isDown)
        {
            m_rb.AddForce((dir.normalized * m_moveSpeed * m_movementMultiplier) + m_rb.velocity.y * Vector3.up, ForceMode.Acceleration);
        }
        else if (IsGround() && OnSloope() && !m_isDown)
        {
            m_rb.AddForce((dir.normalized * m_moveSpeed * m_movementMultiplier) + m_rb.velocity.y * Vector3.up, ForceMode.Acceleration);
        }
        else if (IsGround() && m_isDown)
        {
            m_rb.AddForce((dir.normalized * m_downSpeed * m_movementMultiplier) + m_rb.velocity.y * Vector3.up, ForceMode.Acceleration);
        }
        else if (!IsGround())
        {
            m_rb.AddForce((dir.normalized * m_moveSpeed * m_movementMultiplier * m_airMultiplier) + (m_rb.velocity.y * Vector3.up), ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// 設置判定
    /// </summary>
    /// <returns>
    /// 接地 true 
    /// 空中 false
    /// </returns>
    public bool IsGround()
    {
        Collider[] collision = Physics.OverlapBox(m_centor, m_size, Quaternion.identity, m_zimen);
        if (collision.Length != 0)
        {
            if (m_on)
            {
                m_as.PlayOneShot(m_jumpAudio);
                m_as.loop = false;
                m_on = false;
            }
            
            return true;
        }
        else
        {
            m_on = true;
            m_isJump = false;
            return false;
        }
    }

    /// <summary>
    /// スロープ判定
    /// </summary>
    /// <returns>
    /// スロープ設置    true
    /// 地面           false
    /// </returns>
    bool OnSloope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out m_sloopeHit, 2.84f / 2 + 0.5f))
        {
            if (m_sloopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// 設置判定のGizmo表示
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_isGizmo)
        {
            Gizmos.DrawCube(m_centor, m_size);
        }
    }

    /// <summary>移動インプットシステム</summary>
    public void PlayerMove(InputAction.CallbackContext context)
    {
        m_moveDir = context.ReadValue<Vector3>();
    }

    /// <summary>ジャンプインプットシステム</summary>
    public void PlayerJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_isJump = true;
        }
    }

    /// <summary>ダッシュインプットシステム</summary>
    //public void PlayerDash(InputAction.CallbackContext context)
    //{
    //    if(context.started)
    //    {
    //        m_isDashButton = true;
    //    }

    //    if(context.canceled)
    //    {
    //        m_isDashButton = true;
    //    }
    //}

    /// <summary>しゃがみインプットシステム</summary>
    public void PlayerDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_isDown = true;
        }

        if (context.canceled)
        {
            m_isDown = false;
        }
    }
}
