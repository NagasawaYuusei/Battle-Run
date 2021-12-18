using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public class ShoterBase : MonoBehaviour
{
    [Header("Input")]
    protected bool m_isFire; //Fire1

    [Header("Ray")]
    [SerializeField] Transform m_cameraTransform; //�J�����̃g�����X�t�H�[��
    [SerializeField] float m_distance = 200f; // �e�͈̔�
    [SerializeField] LayerMask m_enemyLayer; //�G�̃��C���[
    protected RaycastHit m_enemy; //���������G

    [Header("Damage")]
    [SerializeField] protected int m_damage; //�_���[�W�̒l
    [SerializeField] protected float m_damagePerSecond; //DPS
    
    [Header("Attack")]
    [SerializeField] protected float m_coolDownTime; //�A�^�b�N�̃N�[���^�C��
    protected int m_atkCount; //�N�[���^�C���O�Ɍ�������
    protected float m_atkNextTime;
    [SerializeField] protected float m_recoilValue; //���R�C���̒l
    [SerializeField] protected float m_recoilUpValue; //���R�C���̏㏸�l

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
