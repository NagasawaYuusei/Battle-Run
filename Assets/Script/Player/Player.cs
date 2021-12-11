using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Speed")]
    float m_moveSpeed = 4f; //スピード
    float m_movementMultiplier = 10f; //通常乗数
    [SerializeField] float m_airMultiplier = 0.4f; //空中乗数
    [SerializeField] float m_walkSpeed = 4f; //歩くスピード
    [SerializeField] float m_springSpeed = 6f; //走るスピード
    [SerializeField] float m_acceleration = 10f; //加速

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

    //[Header("Input")]  
    bool m_isJump; //ジャンプ
    bool m_isDash; //ダッシュ
    bool m_isDown;
    Vector3 m_moveDir; //移動

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
        SpeedControl();
        Jump();
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

    /// <summary>スピード操作</summary>
    void SpeedControl()
    {
        //・地面
        //ダッシュ
        //歩行
        //スライディング
        //しゃがみ

        //・空中
        //しゃがみジャンプ
        //通常ジャンプ
        //スライディングジャンプ

        //・坂
        //スライディング
        //しゃがみ

        //スピードが10

        if (m_isDash && IsGround())
        {
            m_moveSpeed = Mathf.Lerp(m_moveSpeed, m_springSpeed, m_acceleration * Time.deltaTime);
        }
        else if (!m_isDash)
        {
            m_moveSpeed = Mathf.Lerp(m_moveSpeed, m_walkSpeed, m_acceleration * Time.deltaTime);
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

    /// <summary>移動</summary>
    void Move()
    {
        Vector3 dir = Camera.main.transform.TransformDirection(m_moveDir);
        dir.y = 0;
        if (IsGround() && !OnSloope())
        {
            m_rb.AddForce((dir.normalized * m_moveSpeed * m_movementMultiplier) + m_rb.velocity.y * Vector3.up, ForceMode.Acceleration);
        }
        else if (IsGround() && OnSloope())
        {
            m_rb.AddForce((dir.normalized * m_moveSpeed * m_movementMultiplier) + m_rb.velocity.y * Vector3.up, ForceMode.Acceleration);
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
    /// スロープ設置 true
    /// 地面         false
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
        m_isJump = context.ReadValueAsButton();
    }

    /// <summary>ダッシュインプットシステム</summary>
    public void PlayerDash(InputAction.CallbackContext context)
    {
        m_isDash = context.ReadValueAsButton();
    }

    public void PlayerDown(InputAction.CallbackContext context)
    {
        m_isDown = context.ReadValueAsButton();
    }
}
