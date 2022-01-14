using Cinemachine;

public class UseCamera
{
    static CinemachineVirtualCamera m_cvc;

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
