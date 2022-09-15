using UnityEngine;

public class GameManager
{
    static GameManager _instance = new GameManager();
    static public GameManager Instance => _instance;
    GameManager() { }

    bool _isChangeTime;

    public bool IsChangeTime => _isChangeTime;

    public void TimerChange(bool flg)
    {
        _isChangeTime = flg;
    }
}
