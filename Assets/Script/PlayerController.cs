using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody m_rb; //Rigidbody
    float m_mouseX; //マウスの横入力の箱 
    float m_mouseY; // マウスの縦入力の箱

    [Header("設定項目")]
    [SerializeField] float m_moveSpeed; //プレイヤーのスピード
    [SerializeField] float m_dropSpeed;
    [SerializeField] float m_mouseSensitivity = 100f; //マウス感度
    [Space(1)]

    [Header("スクリプトで使うもの")]
    [SerializeField] GameObject m_camera; //カメラオブジェクト

    void Start()
    {
        SetUp();
    }

    void SetUp()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Mouse(); 
    }

    void Mouse()
    {
        float inputMouseX = Input.GetAxis("Mouse X");
        float inputMouseY = Input.GetAxis("Mouse Y");
        m_mouseX += inputMouseX * m_mouseSensitivity;
        m_mouseY += inputMouseY * m_mouseSensitivity;
        if(m_mouseY >= 90)
        {
            m_mouseY = 90;
        }
        if(m_mouseY <= -90)
        {
            m_mouseY = -90;
        }
        m_camera.transform.localRotation = Quaternion.Euler(-m_mouseY, 0, 0);
        transform.localRotation = Quaternion.Euler(0, m_mouseX, 0);
    }
    void FixedUpdate()
    {
        Status();
        Move();
    }

    void Status()
    {
        //m_rb.velocity.y -=

    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        m_rb.velocity = (transform.forward * v * m_moveSpeed) + (transform.right * h * m_moveSpeed);
    }
}
