using UnityEngine;
using UnityEngine.InputSystem;

public class GrapScript : MonoBehaviour
{
    [Tooltip("Grap判定距離"), SerializeField] float m_grapRange;
    [Tooltip("RayのGizmo表示"), SerializeField] bool m_onRay;
    [Tooltip("ポイントのレイヤー番号"), SerializeField] int m_pointLayerValue;
    [Tooltip("プレイヤーのRigidbody"), SerializeField] Rigidbody m_playerRb;
    [Tooltip("近くにいると判定する距離"), SerializeField] float m_pointNearRange = 5;
    [Tooltip("グラップリングのパワー"), SerializeField] float m_grapPower;
    [Tooltip("Y軸にかける特別な力"), SerializeField] float m_yPower;
    [Tooltip("当たったオブジェクトを判定するもの")] RaycastHit m_hit;
    [Tooltip("グラップリング中かどうか")] bool m_onGrapple = false;
    [Tooltip("当たったオブジェクトのトランスフォーム")] Transform m_hitTransform;

    [SerializeField] LineRenderer m_lr;
    [SerializeField] Transform m_gunTransform;
    public bool OnGrapple { get { return m_onGrapple; } }
    public Vector3 GetGrapplePoint { get { return m_hitTransform.position; } }


    void Update()
    {
        GrapCheck();
    }

    void FixedUpdate()
    {
        AddPowerReady();
    }

    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// GrapInputSystem処理　デフォルト　右クリック
    /// </summary>
    /// <param name="context"></param>
    public void PlayerGrap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PointHit();
        }

        if (context.canceled)
        {
            m_onGrapple = false;
        }
    }

    /// <summary>
    /// ポイントがあるかどうかの処理
    /// </summary>
    void PointHit()
    {
        bool hit = Physics.Raycast(transform.position, transform.forward, out m_hit, m_grapRange);
        Debug.DrawRay(transform.position, transform.forward * m_grapRange, Color.blue);
        if (hit && !m_onGrapple)
        {
            if (m_hit.collider.gameObject.layer == m_pointLayerValue)
            {
                m_hitTransform = m_hit.collider.gameObject.transform;
                m_lr.enabled = true;
                m_lr.positionCount = 2;
                m_onGrapple = true;
            }
        }
        return;
    }

    /// <summary>
    /// Grapplingできるかの処理
    /// </summary>
    void GrapCheck()
    {
        if (m_onRay)
        {
            PointHit();
        }
    }

    void AddPowerReady()
    {
        if (m_onGrapple)
        {
            AddPower();
        }
    }

    /// <summary>
    /// Grap処理
    /// </summary>
    void AddPower()
    {
        Vector3 dir = m_hitTransform.position - transform.position;
        dir = new Vector3(dir.x, dir.y * m_yPower, dir.z);
        m_playerRb.AddForce(dir * m_grapPower, ForceMode.Force);
    }

    /// <summary>
    /// GrapPointとの距離をBoolで返す
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="pointTransform"></param>
    /// <returns></returns>
    bool isNear(Transform transform, Transform pointTransform)
    {
        float dis = Vector3.Distance(transform.position, pointTransform.position);
        if (dis < m_pointNearRange)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 未完成
    /// </summary>
    void DrawRope()
    {
        if (!m_onGrapple)
        {
            m_lr.enabled = false;
            return;
        }
        m_lr.SetPosition(0, m_gunTransform.position);
        m_lr.SetPosition(1, m_hitTransform.position);
    }
}
