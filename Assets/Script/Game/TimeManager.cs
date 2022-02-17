using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    bool m_gameReady;
    float m_countDownTime;
    [SerializeField] float m_countDownValue = 3;
    [SerializeField] UIManager m_uim;
    [SerializeField] GameObject m_startObject;
    [SerializeField] AudioClip m_bgm;

    public bool GameReady
    {
        get
        {
            return m_gameReady;
        }
        set
        {
            m_gameReady = value;
        }
    }

    public float CountDownTime
    {
        get
        {
            return m_countDownTime;
        }
    }

    void Start()
    {
        m_countDownTime = m_countDownValue + 0.49f;
    }

    void Update()
    {
        CountDown();
        TimeCount();
    }

    void CountDown()
    {
        if(m_gameReady)
        {
            GameManager.Instance.m_isCountDown = true;
            m_countDownTime -= Time.deltaTime;
            if(m_countDownTime <= 0.49f)
            {
                GameManager.Instance.m_gameTime = 0;
                GameManager.Instance.m_inGame = true;
                GameManager.Instance.m_isGameStartScene = false;
                MusicManager.Instance.BGM(m_bgm);
                m_uim.TimeEnable(true);
                m_startObject.SetActive(false);
                m_gameReady = false;
            }
        }
    }

    void TimeCount()
    {
        if(GameManager.Instance.m_inGame)
        {
            GameManager.Instance.m_gameTime += Time.deltaTime;
        }
    }
}
