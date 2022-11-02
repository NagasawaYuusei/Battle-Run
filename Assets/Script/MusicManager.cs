using System;
using System.Collections;
using UnityEngine;

public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    protected override bool dontDestroyOnLoad { get { return true; } }

    public void BGM(AudioClip audioClip)
    {
        Instance.GetComponent<AudioSource>().clip = audioClip;
        Instance.GetComponent<AudioSource>().Play();
    }

    public static void StartFadeOut(Action action = null)
    {
        if (Instance == null)
        {
            action?.Invoke();
            return;
        }
        Instance.StartCoroutine(Instance.FadeOut(action));
    }

    IEnumerator FadeOut(Action action)
    {
        yield return FadeOut();
        action?.Invoke();
    }

    IEnumerator FadeOut()
    {
        float firstvolume = 0.05f;
        while (firstvolume > 0f)
        {
            firstvolume -= 0.1f * Time.deltaTime;
            if (firstvolume <= 0f)
            {
                firstvolume = 0f;
            }
            GetComponent<AudioSource>().volume = firstvolume;
            yield return null;
        }
    }
}
