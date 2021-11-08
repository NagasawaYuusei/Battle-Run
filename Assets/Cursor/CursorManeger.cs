using UnityEngine;

/// <summary>
/// カーソルを制御するコンポーネント
/// </summary>
public class CursorManeger : MonoBehaviour
{
    /// <summary>
    /// true = カーソルを表示する。
    /// false = カーソルを消し、中央に固定する。
    /// </summary>
    [SerializeField] bool m_cursor;
    void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        Cursor.visible = m_cursor;

        if (m_cursor)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
