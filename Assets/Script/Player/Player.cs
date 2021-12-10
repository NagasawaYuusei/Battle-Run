using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Speed")]
    float m_moveSpeed = 6; //�X�s�[�h
    float m_movementMultiplier = 10; //�ʏ�搔
    [SerializeField] float m_airMultiplier = 0.4f; //�󒆏搔
    [SerializeField] float m_walkSpeed = 4; //�����X�s�[�h
    [SerializeField] float m_springSpeed = 6; //����X�s�[�h
    [SerializeField] float m_acceleration = 10; //����

    [Header("Jump")]
    [SerializeField] float m_jumpPower = 5; //�W�����v�p���[
    [SerializeField] LayerMask m_zimen; //�n�ʃ��C���[
    [SerializeField] bool m_isGizmo = true; //Gizmo�\��
    Vector3 m_centor; //�ݒu����̒��_
    Vector3 m_size; //�ݒu����̃T�C�Y
    [SerializeField] Vector3 m_collisionPoint; //���_����
    [SerializeField] Vector3 m_collisionSize; //�T�C�Y����

    [Header("Drag")]
    [SerializeField] float m_groundDrag = 6; //�n�ʎ��̏d��
    [SerializeField] float m_airDrag = 2; //�󒆎��̏d��

    //[Header("Input")]  
    bool m_isJump; //�W�����v
    bool m_isDash; //�_�b�V��
    Vector3 m_moveDir; //�ړ�

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
        SpeedControl();
        Jump();
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

�@�@/// <summary>�d�͑���</summary>
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

    /// <summary>�X�s�[�h����</summary>
    void SpeedControl()
    {
        if(m_isDash && IsGround())
        {
            m_moveSpeed = Mathf.Lerp(m_moveSpeed, m_springSpeed, m_acceleration * Time.deltaTime);
        }
        else
        {
            m_moveSpeed = Mathf.Lerp(m_moveSpeed, m_walkSpeed, m_acceleration * Time.deltaTime);
        }
    }

    /// <summary>�W�����v</summary>
    void Jump()
    {
        if (m_isJump && IsGround())
        {
            m_rb.velocity = new Vector3(m_rb.velocity.x, 0, m_rb.velocity.z);
            m_rb.AddForce(transform.up * m_jumpPower, ForceMode.Impulse);
        }
    }

    /// <summary>�ړ�</summary>
    void Move()
    {
        m_moveDir = Camera.main.transform.TransformDirection(m_moveDir);
        m_moveDir.y = 0;
        if (IsGround() && !OnSloope())
        {
            print("Ground");
            m_rb.AddForce((m_moveDir.normalized * m_moveSpeed * m_movementMultiplier) + m_rb.velocity.y * Vector3.up, ForceMode.Acceleration);
        }
        else if(IsGround() && OnSloope())
        {
            print("slope");
            m_rb.AddForce((m_slopeMoveDir.normalized * m_moveSpeed * m_movementMultiplier) + m_rb.velocity.y * Vector3.up, ForceMode.Acceleration);
        }
        else if(!IsGround())
        {
            print("air");
            m_rb.AddForce((m_moveDir.normalized * m_moveSpeed * m_movementMultiplier * m_airMultiplier) + (m_rb.velocity.y * Vector3.up), ForceMode.Acceleration);
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
            return false;
        }
    }

    /// <summary>
    /// �X���[�v����
    /// </summary>
    /// <returns>
    /// �X���[�v�ݒu true
    /// �n��         false
    /// </returns>
    bool OnSloope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out m_sloopeHit, 2.84f / 2 + 0.5f))
        {
            if(m_sloopeHit.normal != Vector3.up)
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
        m_isJump = context.ReadValueAsButton();
    }

    /// <summary>�_�b�V���C���v�b�g�V�X�e��</summary>
    public void PlayerDash(InputAction.CallbackContext context)
    {
        m_isDash = context.ReadValueAsButton();
    }
}
