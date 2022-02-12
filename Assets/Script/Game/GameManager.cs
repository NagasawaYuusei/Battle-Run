public class GameManager : SingletonMonoBehaviour<GameManager>
{
    protected override bool dontDestroyOnLoad { get { return true; } }
    public bool m_isGameStart { get; set; }

    public void GameStart(string str)
    {
        m_isGameStart = true;
        SceneChange.LoadScene(str);
    }
}
