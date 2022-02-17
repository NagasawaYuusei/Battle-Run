using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageCon : MonoBehaviour
{
    [SerializeField] SceneController _sc;
    [SerializeField] string _sceneName;
    void OnTriggerEnter(Collider other)
    {
        if(_sceneName == "Stage2")
        {
            GameManager.Instance.m_isCountDown = false;
        }
        if(other.tag == "Player")
        {
            _sc.ChangeScene(_sceneName);
        }
    }
}
