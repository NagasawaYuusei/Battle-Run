using UnityEngine;
using Cinemachine;

public class UseCamera
{
    [Tooltip("CinemachineVirtualCamera")]static CinemachineVirtualCamera m_cvc;

    //カプセル化
    /// <summary>現在使用しているCinemachineVirtualCamera</summary>
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
