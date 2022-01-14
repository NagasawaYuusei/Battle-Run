using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    [Header("Speed")]
    float m_moveSpeed = 4f; //スピード
    float m_movementMultiplier = 10f; //通常乗数
    [SerializeField] float m_airMultiplier = 0.4f; //空中乗数
    [SerializeField] float m_maxMoveSpeed = 4f; //歩くスピード
    [SerializeField, Tooltip("しゃがみスピード")] float m_downSpeed;

    [Header("Jump")]
    [SerializeField] float m_jumpPower = 5f; //ジャンプパワー
    [SerializeField] LayerMask m_zimen; //地面レイヤー
    [SerializeField] bool m_isGizmo = true; //Gizmo表示
    Vector3 m_centor; //設置判定の中点
    Vector3 m_size; //設置判定のサイズ
    [SerializeField] Vector3 m_collisionPoint; //中点差分
    [SerializeField] Vector3 m_collisionSize; //サイズ差分

    [Header("Drag")]
    [SerializeField] float m_groundDrag = 6f; //地面時の重力
    [SerializeField] float m_airDrag = 2f; //空中時の重力

    [Header("Input")]  
    bool m_isJump; //ジャンプ
    Vector3 m_moveDir; //移動
    bool m_isDown;
    bool m_isMove;
    bool m_downAcceleration;

    //[Header("Ather")]
    Vector3 m_slopeMoveDir; //スロープ時の方向
    RaycastHit m_sloopeHit; //スロープの当たり判定
    Rigidbody m_rb; //Rigidbody

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

    void Down()
    {
        Vector3 dir = Camera.main.transform.TransformDirection(Vector3.forward);
        if (m_isDown && IsGround())
        {
            CinemachineTransposer CT = UseCamera.CVC.GetCinemachineComponent<CinemachineTransposer>();
            CT.m_FollowOffset.y = -0.5f;
            //これをますえふ
        }
        else
        {
            CinemachineTransposer CT = UseCamera.CVC.GetCinemachineComponent<CinemachineTransposer>();
            CT.m_FollowOffset.y = 1;
        }

        if(m_downAcceleration && IsGround() && m_isMove)
        {    
            m_downAcceleration = false;
        }
        else if(m_downAcceleration)
        {
            m_downAcceleration = false;
        }
    }

    /// <summary>移動</summary>
    void Move()
    {
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
            return true;
        }
        else
        {
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
        if(context.started)
        {
            m_isMove = true;
        }
        m_moveDir = context.ReadValue<Vector3>();
        if(context.canceled)
        {
            m_isMove = false;
        }
    }

    /// <summary>ジャンプインプットシステム</summary>
    public void PlayerJump(InputAction.CallbackContext context)
    {
        if(context.started)
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

    public void PlayerDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_isDown = true;
            m_downAcceleration = true;
        }

        if (context.canceled)
        {
            m_isDown = false;
        }
    }
}
