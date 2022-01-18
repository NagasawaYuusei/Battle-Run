using UnityEngine;
using UnityEngine.InputSystem;

public class SwordAttack : MonoBehaviour
{
    [Tooltip("�A�^�b�N�C���v�b�g�V�X�e��")] bool m_isAttack;
    [Tooltip("�O��̃A�^�b�N����o�߂��Ă��鎞��")] float m_isAttackTime;
    [Tooltip("�A�^�b�N�̃C���^�[�o��"), SerializeField] float m_isAttackInterval;

    void Update()
    {
        State();
    }

    /// <summary>���</summary>
    void State()
    {
        m_isAttackTime += Time.deltaTime;
    }

    /// <summary>�R���C�_�[�ڐG���̃A�b�v�f�[�g</summary>
    void OnTriggerStay(Collider other)
    {
        if(m_isAttack && m_isAttackTime >= m_isAttackInterval)
        {
            m_isAttackTime = 0;
            //null�`�F�b�N
            other.GetComponent<EnemyScript>()!.EnemyDamage = true;
        }
    }

    /// <summary>�A�^�b�N�C���v�b�g�V�X�e��</summary>
    public void PlayerSword(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_isAttack = true;
        }
    }
}
