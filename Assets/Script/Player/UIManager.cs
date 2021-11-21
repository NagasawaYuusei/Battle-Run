using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text m_ui;
    [SerializeField] PlayerController m_pc;
    void Update()
    {
        m_ui.text = m_pc.NowSpeed.ToString("F2");
    }
}
