using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    [Header("Speed")]
    [Tooltip("�X�s�[�h")] float m_moveSpeed = 4f;
    [Tooltip("�ʏ�搔")] float m_movementMultiplier = 10f;
    [SerializeField, Tooltip("�󒆏搔")] float m_airMultiplier = 0.4f;
    [SerializeField, Tooltip("�ő�X�s�[�h")] float m_maxMoveSpeed = 4f;
    [Tooltip("���Ⴊ�݃X�s�[�h")] float m_downSpeed;
    [SerializeField, Tooltip("���Ⴊ�ݍŏ��X�s�[�h")] float m_downMinSpeed;
    [SerializeField, Tooltip("�X���C�f�B���O�ő�X�s�[�h")] float m_downAccelerationMaxSpeed;
    [SerializeField, Tooltip("�X���C�f�B���O�̃X�s�[�h")] float m_slidingTime;

    [Header("Jump")]
    [SerializeField, Tooltip("�W�����v�p���[")] float m_jumpPower = 5f;
    [SerializeField, Tooltip("�n�ʃ��C���[")] LayerMask m_zimen;
    [SerializeField, Tooltip("Gizmo�\��")] bool m_isGizmo = true;
    [Tooltip("�ݒu����̒��_")] Vector3 m_centor;
    [Tooltip("�ݒu����̃T�C�Y")] Vector3 m_size;
    [SerializeField, Tooltip("���_����")] Vector3 m_collisionPoint;
    [SerializeField, Tooltip("�T�C�Y����")] Vector3 m_collisionSize;

    [Header("Drag")]
    [SerializeField, Tooltip("�n�ʎ��̏d��")] float m_groundDrag = 6f;
    [SerializeField, Tooltip("�󒆎��̏d��")] float m_airDrag = 2f;

    [Header("Input")]
    [Tooltip("�C���v�b�g�V�X�e���W�����v")] bool m_isJump;
    [Tooltip("�C���v�b�g�V�X�e���ړ�")] Vector3 m_moveDir;
    [Tooltip("�C���v�b�g�V�X�e�����Ⴊ��")] bool m_isDown;

    [Header("Ather")]
    [Tooltip("�X���[�v���̕���")] Vector3 m_slopeMoveDir;
    [Tooltip("�X���[�v�̓����蔻��")] RaycastHit m_sloopeHit;
    [Tooltip("Rigidbody")] Rigidbody m_rb;
    [SerializeField, Tooltip("�ŏ��̃V�l�}�V��")] CinemachineVirtualCamera m_firstCamera;
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

    /// <summary>��ԍŏ��̃Z�b�g�A�b�v</summary>
    void AwakeSetUp()
    {
        m_pwr = GetComponent<PlayerWallRun>();
        UseCamera.CVC = m_firstCamera;
    }

    /// <summary>�ŏ��̃Z�b�g�A�b�v</summary>  
    void FirstSetUp()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.freezeRotation = true;
        m_moveSpeed = m_maxMoveSpeed;
    }

    /// <summary>�A�b�v�f�[�g���Ƃ̏��</summary>
    void State()
    {
        //Gizmo����
        m_centor = transform.position + m_collisionPoint;
        m_size = transform.localScale + m_collisionSize;

        //�X���[�v���̕���
        m_slopeMoveDir = Vector3.ProjectOnPlane(m_moveDir, m_sloopeHit.normal);
    }

    /// <summary>�d�͑���</summary>
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

    /// <summary>�W�����v</summary>
    void Jump()
    {
        if (m_isJump && IsGround())
        {
            m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
            m_isJump = false;
        }
    }

    /// <summary>���Ⴊ��</summary>
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

    /// <summary>�ړ�</summary>
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
    /// �ݒu����
    /// </summary>
    /// <returns>
    /// �ڒn true 
    /// �� false
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
    /// �X���[�v����
    /// </summary>
    /// <returns>
    /// �X���[�v�ݒu    true
    /// �n��           false
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
    /// �ݒu�����Gizmo�\��
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_isGizmo)
        {
            Gizmos.DrawCube(m_centor, m_size);
        }
    }

    /// <summary>�ړ��C���v�b�g�V�X�e��</summary>
    public void PlayerMove(InputAction.CallbackContext context)
    {
        m_moveDir = context.ReadValue<Vector3>();
    }

    /// <summary>�W�����v�C���v�b�g�V�X�e��</summary>
    public void PlayerJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_isJump = true;
        }
    }

    /// <summary>�_�b�V���C���v�b�g�V�X�e��</summary>
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

    /// <summary>���Ⴊ�݃C���v�b�g�V�X�e��</summary>
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
