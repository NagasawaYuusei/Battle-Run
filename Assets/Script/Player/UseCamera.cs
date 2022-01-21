using UnityEngine;
using Cinemachine;

public class UseCamera
{
    [Tooltip("CinemachineVirtualCamera")]static CinemachineVirtualCamera m_cvc;

    //�J�v�Z����
    /// <summary>���ݎg�p���Ă���CinemachineVirtualCamera</summary>
    public static CinemachineVirtualCamera CVC
    {
        get
        {
            return m_cvc;
        }
        set
        {
            m_cvc = value;
        }
    }
}
