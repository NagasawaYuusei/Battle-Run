using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class nav : MonoBehaviour
{
    NavMeshAgent m_Agent;
    [SerializeField] Transform target;     // �ڕW�n�_
    [SerializeField] LineRenderer line;
    NavMeshPath path;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    public void NavLine(bool on)
    {
        if (on)
        {
            line.enabled = true;
            // NavMeshAgent �ɖړI�n��ݒ肷��
            m_Agent.SetDestination(target.position);

            // �o�H�擾�p�̃C���X�^���X�쐬
            path = new NavMeshPath();
            // �����I�Ȍo�H�v�Z���s
            m_Agent.CalculatePath(target.position, path);

            // LineRenderer�Ōo�H�`��
            line.SetVertexCount(path.corners.Length);
            line.SetPositions(path.corners);
            m_Agent.isStopped = true;
        }
        else
        {
            line.enabled = false;
        }
    }
}
