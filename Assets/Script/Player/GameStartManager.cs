using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] Text m_gameStartUI;
    bool m_isReady;
    [SerializeField] string m_gameStartPosName;
    [SerializeField] TimeManager m_tm;
    [SerializeField] GameObject m_startObject;
    [SerializeField] GameObject m_startWall;

    public void PlayerGameStart(InputAction.CallbackContext context)
    {
        if(m_isReady && context.started)
        {
            m_tm.GameReady = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == m_gameStartPosName)
        {
            m_isReady = true;
            m_gameStartUI.gameObject.SetActive(true);
        }

        if (other.tag == m_gameStartPosName && GameManager.Instance.m_inGame)
        {
            m_startWall.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == m_gameStartPosName && !m_tm.GameReady)
        {
            m_isReady = false;
            m_gameStartUI.gameObject.SetActive(false);
        }

        if(other.tag == m_gameStartPosName && GameManager.Instance.m_inGame)
        {
            m_startObject.SetActive(true);
            m_gameStartUI.gameObject.SetActive(false);
        }
    }
}
