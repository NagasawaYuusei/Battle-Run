using UnityEngine;

public class GrapllingHook : MonoBehaviour
{
    LineRenderer m_lr;
    Vector3 m_grapplePoint;
    [SerializeField] LayerMask m_whatIsGrappleable;
    [SerializeField] Transform m_hookTip;
    [SerializeField] Transform m_camera;
    [SerializeField] Transform m_player;
    [SerializeField] float m_maxDistance = 100;
    SpringJoint m_joint;

    void Start()
    {
        m_lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        DrawRope();
        PlayerInput();
    }

    void PlayerInput()
    {
        if(Input.GetButtonDown("Grap"))
        {
            StartGrapple();
        }
        else if(Input.GetButtonUp("Grap"))
        {
            StopGrapple();
        }
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(m_camera.position, m_camera.forward, out hit, m_maxDistance, m_whatIsGrappleable))
        {
            m_grapplePoint = hit.point;
            m_joint = m_player.gameObject.AddComponent<SpringJoint>();
            m_joint.autoConfigureConnectedAnchor = false;
            m_joint.connectedAnchor = m_grapplePoint;

            float distanceFromPoint = Vector3.Distance(m_player.position, m_grapplePoint);

            //マジックナンバー使用
            m_joint.maxDistance = distanceFromPoint * 0.8f;
            m_joint.minDistance = distanceFromPoint * 0.25f;

            m_joint.spring = 4.5f;
            m_joint.damper = 7f;
            m_joint.massScale = 4.5f;

            m_lr.positionCount = 2;
        }
    }

    void StopGrapple()
    {
        m_lr.positionCount = 0;
        Destroy(m_joint);
    }

    void LateUpdate()
    {
        DrawRope();
    }

    void DrawRope()
    {
        if (!m_joint) return;
        m_lr.SetPosition(0, m_hookTip.position);
        m_lr.SetPosition(1, m_grapplePoint);
    }
}
