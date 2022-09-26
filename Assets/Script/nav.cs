using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class nav : MonoBehaviour
{
    NavMeshAgent m_Agent;
    [SerializeField] Transform target;     // 目標地点
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
            // NavMeshAgent に目的地を設定する
            m_Agent.SetDestination(target.position);

            // 経路取得用のインスタンス作成
            path = new NavMeshPath();
            // 明示的な経路計算実行
            m_Agent.CalculatePath(target.position, path);

            // LineRendererで経路描画
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
