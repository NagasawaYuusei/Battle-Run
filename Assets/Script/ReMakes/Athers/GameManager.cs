using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    protected override bool dontDestroyOnLoad { get { return true; } }
    public bool m_isGameStartScene { get; set; }
    public bool m_inGame { get; set; }
    public float m_gameTime { get; set; }

    public float m_lastScene { get; set; }

    private static bool roadNow = false;

    public bool m_isCountDown { get; set; }

    //public float m_first { get; set; }

    //public float m_second { get; set; }

    //public float m_third { get; set; }

    //public string m_firstName { get; set; }

    //public string m_secondName { get; set; }

    //public string m_thirdName { get; set; }

    public void GameStart(string str)
    {
        m_isGameStartScene = true;
        SceneChange.LoadScene(str);
    }

    public void MusicFade()
    {
        if (roadNow)
        {
            return;
        }
        roadNow = true;
        MusicManager.StartFadeOut(() => Fade());
    }
    private static void Fade()
    {
        roadNow = false;
        GameObject.Find("MusicManager").GetComponent<AudioSource>().Stop();
    }
}
