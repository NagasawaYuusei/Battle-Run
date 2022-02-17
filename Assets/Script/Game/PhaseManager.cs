using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PhaseManager : MonoBehaviour
{
    int m_nowPhase;
    [SerializeField] Transform[] m_deathPos;
    [SerializeField] Transform[] m_restartPos;
    [SerializeField] int[] m_enemyTotal;
    int m_remainingEnemy;
    [SerializeField] GameObject[] m_fields;
    [SerializeField] GameObject m_player;
    [SerializeField] Text _restartText;
    bool m_isDeath;
    List<GameObject> m_enemyList = new List<GameObject>();

    void Update()
    {
        FallDeath();
    }

    void FallDeath()
    {
        if(m_player.transform.position.y < m_deathPos[m_nowPhase].position.y)
        {
            Death();
        }
    }

    public void EnemyKnock(GameObject go)
    {
        m_enemyList.Add(go);
        m_remainingEnemy++;
        go.SetActive(false);
        if (m_remainingEnemy == m_enemyTotal[m_nowPhase] && m_nowPhase == m_fields.Length - 1)
        {
            m_fields[m_nowPhase].SetActive(true);
            m_enemyList.Clear();
            m_remainingEnemy = 0;
        }
        else if(m_remainingEnemy == m_enemyTotal[m_nowPhase])
        {
            m_fields[m_nowPhase].SetActive(true);
            m_enemyList.Clear();
            m_remainingEnemy = 0;
            m_nowPhase++;
        }
    }

    public void Death()
    {
        _restartText.text = "Restart Push R";
        _restartText.gameObject.SetActive(true);
        m_isDeath = true;
        m_remainingEnemy = 0;
    }

    public void Restart(InputAction.CallbackContext context)
    {
        if(context.started && m_isDeath)
        {
            m_player.transform.position = m_restartPos[m_nowPhase].transform.position;
            m_player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _restartText.gameObject.SetActive(false);
            foreach (GameObject go in m_enemyList)
            {
                go.SetActive(true);
            }
            m_enemyList.Clear();
            m_isDeath = false;
        }
    }
}
