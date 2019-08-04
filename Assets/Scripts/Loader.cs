using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Loader : MonoBehaviour
{
    public Text text;
    public VideoPlayer player;
    public AudioSource src;
    public Settings settings;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Skip());
        player.loopPointReached += StartSkip;
    }

    bool ended = false;

    void StartSkip(VideoPlayer _)
    {
        ended = true;
    }

    void Update()
    {
        src.volume = settings.GetMusicVolume();
        if (player != null && player.canSetDirectAudioVolume) player.SetDirectAudioVolume(0, settings.GetGameVolume());
    }

    IEnumerator Skip()
    {
        float a = 1f;
        while (!Input.GetKeyDown(KeyCode.Space) && !ended)
        {
            yield return null;
            if (a > 0)
            {
                a = Mathf.Clamp(a - Time.deltaTime * 0.2f, 0f, 1f);
                Color c = text.color;
                c.a = a;
                text.color = c;
            }
        }

        Color x = text.color;
        x.a = 1f;
        text.color = x;
        text.text = "Loading";

        AsyncOperation op = SceneManager.LoadSceneAsync("DialogScene");
        while (!op.isDone) yield return null;
        Scene scene = SceneManager.GetSceneByName("DialogScene");
        Scene oldScene = SceneManager.GetActiveScene();

        player = null;

        SceneManager.SetActiveScene(scene);
        SceneManager.UnloadSceneAsync(oldScene);
    }

}
