using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text m_coutDownText;
    [SerializeField] Text m_timeText;
    [SerializeField] TimeManager m_tm;
    bool m_gameFinish;

    public bool GameFinish
    {
        set
        {
            m_gameFinish = value;
        }
    }
    void Update()
    {
        TimeText();
        CountDownText();
    }

    void TimeText()
    { 
        if(GameManager.Instance.m_inGame)
        {
            m_timeText.text = "Time:" + ((int)GameManager.Instance.m_gameTime).ToString("0000") +
                (GameManager.Instance.m_gameTime - ((int)GameManager.Instance.m_gameTime)).ToString("F2").TrimStart('0');
        }
    }

    void CountDownText()
    {
        if(m_tm.GameReady)
        {
            m_coutDownText.text = m_tm.CountDownTime.ToString("F0");
        }
        else if(!m_tm.GameReady && GameManager.Instance.m_inGame && GameManager.Instance.m_isCountDown)
        {
            if (!m_coutDownText)
                return;
            m_coutDownText.text = "Go!";
        }
        else
        {
            m_coutDownText.text = "Ready? Push R";
        }

        if(m_gameFinish)
        {
            m_coutDownText.text = "Goal! Time:" + GameManager.Instance.m_gameTime;
        }
    }

    public void TimeEnable(bool on)
    {
        m_timeText.gameObject.SetActive(on);
    }
}
