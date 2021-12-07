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
    [SerializeField] float m_grappleSpeed;

    SpringJoint m_joint;
    float m_distanceFromPoint;

    void Start()
    {
        m_lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        PlayerInput();
        State();
    }

    void PlayerInput()
    {
        if(Input.GetButtonDown("Grap"))
        {
            StartGrapple();
            Debug.Log("Push");
        }
        else if(Input.GetButtonUp("Grap"))
        {
            StopGrapple();
        }
    }

    void State()
    {
        //if ()
        //{
        //    StopGrapple();
        //}
        if(m_joint)
        {
            m_joint.maxDistance -= m_grappleSpeed;
            m_joint.minDistance -= m_grappleSpeed;
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

            m_distanceFromPoint = Vector3.Distance(m_player.position, m_grapplePoint);

            m_joint.maxDistance = m_distanceFromPoint;
            m_joint.minDistance = m_distanceFromPoint;

            //m_joint.spring = 4.5f;
            //m_joint.damper = 7f;
            //m_joint.massScale = 4.5f;

            //m_joint = m_player.gameObject.AddComponent<PlayerJoint>();
            //m_joint.m_connectedAnchor = m_grapplePoint;

            m_lr.positionCount = 2;

            //m_player.GetComponent<PlayerController>().enabled = false;
        }
        Debug.Log("Start");
    }

    void StopGrapple()
    {
        m_lr.positionCount = 0;
        Destroy(m_joint);
        //m_player.GetComponent<PlayerController>().enabled = true;
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
