using UnityEngine;
//using Cinemachine;

/// <summary>
/// プレイヤーを制御するコンポーネント
/// </summary>
public class PlayerController : MonoBehaviour
{
    Rigidbody m_rb; //Rigidbody
    Vector3 m_centor; //設置判定の中点
    Vector3 m_size; //設置判定のサイズ
    float m_nowSpeed;

    //Input
    float m_horizontal; //Horizontal
    float m_vertical; //Vertical

    [Header("Settings")]

    [SerializeField] float m_firstSpeed;
    /// <summary>プレイヤーの最大スピード</summary>
    [SerializeField] float m_maxMoveSpeed;
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

    /// <summary>
    /// 現状のプレイヤーステータス
    /// </summary>
    void States()
    {
        m_centor = transform.position + m_collisionPoint;
        m_size = transform.localScale + m_collisionSize;
        if(m_vertical == 0 && m_horizontal == 0)
        {
            m_nowSpeed = 0;
        }
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
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Move(m_horizontal, m_vertical);
    }

    /// <summary>
    /// プレイヤーの動き　前横
    /// </summary>
    void Move(float h,float v)
    {
        Vector3 dir = (Vector3.right * h) + (Vector3.forward * v);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        if(m_nowSpeed == 0)
        {
            m_nowSpeed = m_firstSpeed;
        }
        else if(m_maxMoveSpeed > m_nowSpeed)
        {
            m_nowSpeed += Time.deltaTime * 3;
        }
        m_rb.velocity = dir.normalized * m_nowSpeed + m_rb.velocity.y * Vector3.up;  // Y 軸方向の速度は変えない
    }

    /// <summary>
    /// ジャンプ処理(途中)　縦
    /// </summary>
    void Jump()
    {
        if(IsGround())
        {
            Debug.Log("Jump");
            m_rb.velocity = transform.up * m_jumpPower; //+ m_rb.velocity.x * Vector3.right + m_rb.velocity.z * Vector3.forward;
        }
    }

    /// <summary>
    /// 設置判定
    /// </summary>
    /// <returns>
    /// 接地 true 
    /// 空中 false
    /// </returns>
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
}
