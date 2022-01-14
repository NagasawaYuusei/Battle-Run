using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    [Header("Speed")]
    float m_moveSpeed = 4f; //�X�s�[�h
    float m_movementMultiplier = 10f; //�ʏ�搔
    [SerializeField] float m_airMultiplier = 0.4f; //�󒆏搔
    [SerializeField] float m_maxMoveSpeed = 4f; //�����X�s�[�h
    [SerializeField, Tooltip("���Ⴊ�݃X�s�[�h")] float m_downSpeed;

    [Header("Jump")]
    [SerializeField] float m_jumpPower = 5f; //�W�����v�p���[
    [SerializeField] LayerMask m_zimen; //�n�ʃ��C���[
    [SerializeField] bool m_isGizmo = true; //Gizmo�\��
    Vector3 m_centor; //�ݒu����̒��_
    Vector3 m_size; //�ݒu����̃T�C�Y
    [SerializeField] Vector3 m_collisionPoint; //���_����
    [SerializeField] Vector3 m_collisionSize; //�T�C�Y����

    [Header("Drag")]
    [SerializeField] float m_groundDrag = 6f; //�n�ʎ��̏d��
    [SerializeField] float m_airDrag = 2f; //�󒆎��̏d��

    [Header("Input")]  
    bool m_isJump; //�W�����v
    Vector3 m_moveDir; //�ړ�
    bool m_isDown;
    bool m_isMove;
    bool m_downAcceleration;

    //[Header("Ather")]
    Vector3 m_slopeMoveDir; //�X���[�v���̕���
    RaycastHit m_sloopeHit; //�X���[�v�̓����蔻��
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

    void Down()
    {
        Vector3 dir = Camera.main.transform.TransformDirection(Vector3.forward);
        if (m_isDown && IsGround())
        {
            CinemachineTransposer CT = UseCamera.CVC.GetCinemachineComponent<CinemachineTransposer>();
            CT.m_FollowOffset.y = -0.5f;
            //������܂�����
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

    /// <summary>�ړ�</summary>
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
            return true;
        }
        else
        {
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

    /// <summary>�W�����v�C���v�b�g�V�X�e��</summary>
    public void PlayerJump(InputAction.CallbackContext context)
    {
        if(context.started)
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
