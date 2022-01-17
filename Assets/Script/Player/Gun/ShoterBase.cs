using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public class ShoterBase : MonoBehaviour
{
    [Header("Input")]
    [Tooltip("���C���v�b�g�V�X�e��")]protected bool m_isFire; //Fire1

    [Header("Ray")]
    [SerializeField, Tooltip("�J�����̃g�����X�t�H�[��")] Transform m_cameraTransform;
    [SerializeField, Tooltip("�e�͈̔�")] float m_distance = 200f;
    [SerializeField, Tooltip("�G�̃��C���[")] LayerMask m_enemyLayer;
    [Tooltip("���������G")] protected RaycastHit m_enemy;

    [Header("Damage")]
    [SerializeField, Tooltip("�_���[�W�̒l")] protected int m_damage;
    [SerializeField, Tooltip("DPS")] protected float m_damagePerSecond;
    
    [Header("Attack")]
    [SerializeField, Tooltip("�A�^�b�N�̃N�[���^�C��")] protected float m_coolDownTime;
    protected bool m_isFireSecond;

    public bool IsFireSecond
    {
        get
        {
            return m_isFireSecond;
        }
    }

    /// <summary>�}�E�X���t�g�C���v�b�g�V�X�e��</summary>
    public void PlayerFire(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            m_isFire = true;
        }

        if(context.canceled)
        {
            m_isFire = false;
        }
    }

    /// <summary>�}�E�X���C�g�C���v�b�g�V�X�e��</summary>
    public void PlayerFireSecond(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            FireSecondMethod(true);
        }
        
        if(context.canceled)
        {
            FireSecondMethod(false);
        }
    }

    /// <summary>�U���̊֐�</summary>
    protected void Fire()
    {
        m_coolDownTime += Time.deltaTime;
        if (m_isFire)
        {
            FireMethod();
        }
    }

    /// <summary>���t�g�U�����\�b�h</summary>
    protected virtual void FireMethod(){}

    /// <summary>���C�g�U�����\�b�h</summary>
    protected virtual void FireSecondMethod(bool on){}

    /// <summary>���R�C�����\�b�h</summary>
    protected virtual void Recoil(CinemachinePOV pov) {}

    /// <summary>�U������</summary>
    protected bool IsCollision()
    {
        bool isCollision = Physics.Raycast(m_cameraTransform.position, m_cameraTransform.forward, out m_enemy, m_distance, m_enemyLayer);
        return isCollision;
    }
}
