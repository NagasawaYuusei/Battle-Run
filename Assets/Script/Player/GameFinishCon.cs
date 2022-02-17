using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinishCon : MonoBehaviour
{
    [SerializeField] string m_gameFinishPosName;
    [SerializeField] GameObject[] _finishWall;
    [SerializeField] UIManager _uim;

    void Awake()
    {
        if (GameManager.Instance.m_inGame)
        {
            _finishWall[0].SetActive(false);
            _finishWall[1].SetActive(false);
        }
        else
        {
            _finishWall[0].SetActive(true);
            _finishWall[1].SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == m_gameFinishPosName && GameManager.Instance.m_inGame)
        {
            StartCoroutine(GameFinish());
            GameManager.Instance.m_inGame = false;
            _finishWall[0].SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == m_gameFinishPosName)
        {
            _finishWall[1].SetActive(true);
        }
    }

    IEnumerator GameFinish()
    {
        _uim.GameFinish = true;
        GameManager.Instance.MusicFade();
        yield return new WaitForSeconds(3);
        _uim.TimeEnable(false);
        _uim.GameFinish = false;
    }
}
