using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    static GameManager _instance = new GameManager();
    static public GameManager Instance => _instance;

    static public GameObject _player;

    int _currentStage;
    int _currentPhase;
    [SerializeField] int _stageNum;
    //[SerializeField] int[] _phaseNums = new int[_stageNum];
    GameManager() { }

    bool _isChangeTime;

    public bool IsChangeTime => _isChangeTime;

    public void TimerChange(bool flg)
    {
        _isChangeTime = flg;
    }

    public void SetPlayer(GameObject go)
    {
        _player = go;
    }

    public void SetStageNum()
    {
        _currentStage = int.Parse(SceneManager.GetActiveScene().name.Split('_')[1]);
    }

    public void SetPhaseNums()
    {
        //_phaseNums = new int[_currentStage - 1];
    }

    public void SetRespownPos(int phaseNum)
    {
    }
}
