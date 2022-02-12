using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSystem : MonoBehaviour
{
    public static InGameSystem Instance = default;
    [SerializeField] Transform m_respownPos;
    [SerializeField] Transform m_firstPos;
    [SerializeField] GameObject m_playerObject;
    void Awake()
    {
        Instance = this;
        if(!GameManager.Instance.m_isGameStart)
        {
            m_playerObject.transform.position = m_respownPos.position;
            m_playerObject.transform.rotation = m_respownPos.rotation;
        }
        else
        {
            m_playerObject.transform.position = m_firstPos.position;
            m_playerObject.transform.rotation = m_firstPos.rotation;
        }
    }
}
