using UnityEngine;
using UnityEngine.InputSystem;

public class SwordAttack : MonoBehaviour
{
    [Tooltip("�A�^�b�N�C���v�b�g�V�X�e��")] bool m_isAttack;
    [Tooltip("�O��̃A�^�b�N����o�߂��Ă��鎞��")] float m_attackTime;
    [Tooltip("�A�^�b�N�̃C���^�[�o��"), SerializeField] float m_attackInterval;
    [Tooltip("���������G")]RaycastHit m_inEnemyRay;
    [Tooltip("�A�^�b�N�̋���"), SerializeField] float m_attackLength;

    void Update()
    {
        State();
        Damage();
    }

    /// <summary>���</summary>
    void State()
    {
        m_attackTime += Time.deltaTime;
    }

    /// <summary>�A�^�b�N����֐�</summary>
    void Damage()
    {
        if (m_isAttack && m_attackTime >= m_attackInterval)
        {
            if (IsAttack())
            {
                m_inEnemyRay.collider.GetComponent<EnemyScript>().EnemyDamage = true;
            }
            m_attackTime = 0;
        }
        m_isAttack = false;
    }

    /// <summary>
    /// �A�^�b�N����
    /// </summary>
    /// <returns>
    /// �G������ true 
    /// �G�����Ȃ� false
    /// </returns>
    public bool IsAttack()
    {
        if (Physics.Raycast(transform.position, transform.forward, out m_inEnemyRay, m_attackLength))
        {
            if (m_inEnemyRay.collider.GetComponent<EnemyScript>())
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

    /// <summary>�A�^�b�N�C���v�b�g�V�X�e��</summary>
    public void PlayerSword(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_isAttack = true;
        }
    }
}
