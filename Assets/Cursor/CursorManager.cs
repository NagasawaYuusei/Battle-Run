using UnityEngine;

/// <summary>
/// カーソルを制御するコンポーネント
/// </summary>
public class CursorManager : MonoBehaviour
{
    /// <summary>
    /// true = カーソルを表示する。
    /// false = カーソルを消し、中央に固定する。
    /// </summary>
    [SerializeField] bool m_cursor;
    void Start()
    {
        SetUp(m_cursor);
    }

    public void SetUp(bool on)
    {
        Cursor.visible = on;

        if (on)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
