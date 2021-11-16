using UnityEngine;
using Cinemachine;
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
    Vector3 m_nowdir;
    CinemachineTransposer m_cinemaTransposer;
    bool m_isDown;
    float m_firstOffsetY;

    //Input
    float m_horizontal; //Horizontal
    float m_vertical; //Vertical

    [Header("Settings")]
    /// <summary>ジャンプの力</summary>
    [SerializeField] float m_jumpPower;
    /// <summary>設置判定のGizmoを表示するかどうか</summary>
    [SerializeField] bool m_isGizmo = true;
    /// <summary>設置判定オフセット</summary>
    [SerializeField] Vector3 m_collisionPoint;
    /// <summary>設置判定サイズ</summary>
    [SerializeField] Vector3 m_collisionSize;
    [SerializeField] float m_cameraMutation;

    //[SerializeField] float m_dropSpeed;
    //[SerializeField] float m_mouseSensitivity = 1f; //マウス感度

    [Space(1)]
    [Header("UseScript")]
    [SerializeField] LayerMask m_zimen;
    [SerializeField] CinemachineVirtualCamera m_cinema;
    //[SerializeField] CinemachineVirtualCamera m_chinemachineFPS;

    /// <summary>
    /// カプセル化開始
    /// </summary>
    
    public float NowSpeed
    {
        get
        {
            return m_nowSpeed;
        }
        set
        {
            m_nowSpeed = value;
        }
    }

    public float Horizontal
    {
        get
        {
            return m_horizontal;
        }
    }

    public float Vertical
    {
        get
        {
            return m_vertical;
        }
    }

    /// <summary>
    /// カプセル化終了
    /// </summary>

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
        m_cinemaTransposer = m_cinema.GetCinemachineComponent<CinemachineTransposer>();
        m_firstOffsetY = m_cinemaTransposer.m_FollowOffset.y;
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
        ///Gizmo
        m_centor = transform.position + m_collisionPoint;
        m_size = transform.localScale + m_collisionSize;

        if(!m_isDown)
        {
            m_cinemaTransposer.m_FollowOffset.y += m_cameraMutation;
            if (m_cinemaTransposer.m_FollowOffset.y >= m_firstOffsetY)
            {
                m_cinemaTransposer.m_FollowOffset.y = m_firstOffsetY;
            }
            transform.localScale = new Vector3(1, 1, 1);
        }
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

        if(Input.GetButton("Down"))
        {
            Down();
            m_isDown = true;
        }
        else
        {
            m_isDown = false;
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
        if(h != 0 || v != 0)
        {
            m_nowdir = dir;
        }
        m_rb.velocity = m_nowdir.normalized * m_nowSpeed + m_rb.velocity.y * Vector3.up;  // Y 軸方向の速度は変えない
    }

    /// <summary>
    /// ジャンプ処理(途中)　縦
    /// </summary>
    void Jump()
    {
        if(IsGround())
        {
            m_rb.velocity = transform.up * m_jumpPower; //+ m_rb.velocity.x * Vector3.right + m_rb.velocity.z * Vector3.forward;
        }
    }

    /// <summary>
    /// しゃがみ処理
    /// </summary>
    void Down()
    {
        m_cinemaTransposer.m_FollowOffset.y -= m_cameraMutation;
        if(m_cinemaTransposer.m_FollowOffset.y <= 1)
        {
            m_cinemaTransposer.m_FollowOffset.y = 1;
        }
        transform.localScale = new Vector3(1, 0.57f, 1);
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
