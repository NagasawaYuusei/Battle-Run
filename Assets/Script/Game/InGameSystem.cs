using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSystem : MonoBehaviour
{
    public static InGameSystem Instance = default;
    [SerializeField] Transform m_respownPos;
    [SerializeField] Transform m_firstPos;
    [SerializeField] GameObject m_playerObject;
    [SerializeField] bool _isGameStartDebug;
    void Awake()
    {
        Instance = this;
        if(!GameManager.Instance.m_isGameStartScene)
        {
            m_playerObject.transform.position = m_respownPos.position;
            m_playerObject.transform.rotation = m_respownPos.rotation;
        }
        else if(_isGameStartDebug)
        {
            m_playerObject.transform.position = m_firstPos.position;
            m_playerObject.transform.rotation = m_firstPos.rotation;
            GameManager.Instance.m_isGameStartScene = false;
            _isGameStartDebug = false;
        }
        else
        {
            m_playerObject.transform.position = m_firstPos.position;
            m_playerObject.transform.rotation = m_firstPos.rotation;
            GameManager.Instance.m_isGameStartScene = false;
            _isGameStartDebug = false;
        }
    }
}
