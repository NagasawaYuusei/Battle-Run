using UnityEngine.SceneManagement;

public class SceneChange
{
    private static bool loadNow = false;
    /// <summary>
    /// 指定シーンに移行する
    /// </summary>
    public static void LoadScene(string sceneName)
    {
        if (loadNow)
        {
            return;
        }
        loadNow = true;
        FadeController.StartFadeOut(() => Load(sceneName));
    }  
    private static void Load(string sceneName)
    {
        loadNow = false;
        SceneManager.LoadScene(sceneName);
    }
}
