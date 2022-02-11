using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArm : MonoBehaviour
{
    [SerializeField] GrapScript m_gs;
    Quaternion m_desiredRotation;
    float m_rotationSpeed = 5f;
    void Update()
    {
        if (!m_gs.OnGrapple)
        {
            m_desiredRotation = transform.parent.rotation;
        }
        else
        {
            m_desiredRotation = Quaternion.LookRotation(m_gs.GetGrapplePoint - transform.position);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, m_desiredRotation, Time.deltaTime * m_rotationSpeed);
    }
}
