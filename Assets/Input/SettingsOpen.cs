using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsOpen : MonoBehaviour
{
    [SerializeField] GameObject m_ui;
    [SerializeField] string m_settingsButtonName = "Settings";
    bool m_stop = false;

    public bool Stop
    {
        get
        {
            return m_stop;
        }
    }

    void Update()
    {
        InputPlayer();
    }

    void InputPlayer()
    {
        if (Input.GetButtonDown(m_settingsButtonName))
        {
            m_ui.SetActive(!m_ui.activeSelf);
            m_stop = m_ui.activeSelf;
        }
    }
}
