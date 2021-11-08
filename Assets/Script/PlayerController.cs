using UnityEngine;
//using Cinemachine;

/// <summary>
/// プレイヤーを制御するコンポーネント
/// </summary>
public class PlayerController : MonoBehaviour
{
    Rigidbody m_rb; //Rigidbody
    Vector3 m_centor;
    Vector3 m_size;

    //Input
    float m_horizontal; //Horizontal
    float m_vertical; //Vertical
    bool m_isJump; //Jump

    [Header("Settings")]

    /// <summary>プレイヤーのスピード</summary>
    [SerializeField] float m_moveSpeed;
    /// <summary>ジャンプの力</summary>
    [SerializeField] float m_jumpPower;
    /// <summary>設置判定のGizmoを表示するかどうか</summary>
    [SerializeField] bool m_isGizmo = true;
    /// <summary>設置判定オフセット</summary>
    [SerializeField] Vector3 m_collisionPoint;
    /// <summary>設置判定サイズ</summary>
    [SerializeField] Vector3 m_collisionSize;
    //[SerializeField] float m_dropSpeed;
    //[SerializeField] float m_mouseSensitivity = 1f; //マウス感度
    [Space(1)]

    [Header("UseScript")]
    [SerializeField] LayerMask m_zimen;
    //[SerializeField] CinemachineVirtualCamera m_chinemachineFPS;


    void Start()
    {
        SetUp();
    }

    /// <summary>
    /// GetComponent等のセットアップ
    /// </summary>
    void SetUp()
    {
        m_rb = this.GetComponent<Rigidbody>();
   
    }

    void Update()
    {
        States();
        PlayerInput();
    }

    void States()
    {
        m_centor = transform.position + m_collisionPoint;
        m_size = transform.localScale + m_collisionSize;
        //m_chinemachineFPS.m_HorizontalAxis.m_MaxSpeed = m_chinemachineFPS.m_HorizontalAxis.m_MaxSpeed * m_mouseSensitivity;
        //m_chinemachineFPS.m_VerticalAxis.m_MaxSpeed = m_chinemachineFPS.m_VerticalAxis.m_MaxSpeed * m_mouseSensitivity;
        //m_chinemachineFPS.m_XAxis.m_MaxSpeed = m_chinemachineFPS.m_XAxis.m_MaxSpeed * m_mouseSensitivity;
        //m_chinemachineFPS.m_YAxis.m_MaxSpeed = m_chinemachineFPS.m_YAxis.m_MaxSpeed * m_mouseSensitivity;
    }

    /// <summary>
    /// Input情報
    /// </summary>
    void PlayerInput()
    {
        m_horizontal = Input.GetAxisRaw("Horizontal");
        m_vertical = Input.GetAxisRaw("Vertical");
        m_isJump = Input.GetButtonDown("Jump");
    }

    void FixedUpdate()
    {
        FixedStatus();
        Move(m_horizontal,m_vertical);
        Jump(m_isJump);
    }

    void FixedStatus()
    {
       
    }

    /// <summary>
    /// プレイヤーの動き　前横
    /// </summary>
    void Move(float h,float v)
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        Vector3 dir = (Vector3.right * h) + (Vector3.forward * v);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        m_rb.velocity = dir.normalized * m_moveSpeed + m_rb.velocity.y * Vector3.up;  // Y 軸方向の速度は変えない
    }

    /// <summary>
    /// ジャンプ処理(途中)
    /// </summary>
    void Jump(bool j)
    {
        if(j && IsGround())
        {
            Debug.Log("Jump");
            m_rb.velocity = transform.up * m_jumpPower;
        }
    }

    bool IsGround()
    {
        Collider[] collision = Physics.OverlapBox(m_centor, m_size,Quaternion.identity, m_zimen);
        if (collision.Length != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_isGizmo)
        {
            Gizmos.DrawCube(m_centor, m_size);
        }
    }
}
