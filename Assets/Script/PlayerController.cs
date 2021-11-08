using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    Rigidbody m_rb; //Rigidbody

    [Header("設定項目")]
    [SerializeField] float m_moveSpeed; //プレイヤーのスピード
    [SerializeField] float m_jumpPower; //ジャンプの力
    [SerializeField] float m_dropSpeed;
    [SerializeField] float m_mouseSensitivity = 100f; //マウス感度
    [Space(1)]

    [Header("スクリプトで使うもの")]
    [SerializeField] GameObject m_camera; //カメラオブジェクト

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
        
    }

    void FixedUpdate()
    {
        Status();
        Move();
        Jump();
    }

    void Status()
    {
       
    }

    /// <summary>
    /// プレイヤーの動き　前横
    /// </summary>
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = (Vector3.right * h) + (Vector3.forward * v);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        m_rb.velocity = dir.normalized * m_moveSpeed + m_rb.velocity.y * Vector3.up;  // Y 軸方向の速度は変えない
    }

    /// <summary>
    /// ジャンプ処理(途中)
    /// </summary>
    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            m_rb.velocity = transform.up * m_jumpPower;
        }
    }
}
