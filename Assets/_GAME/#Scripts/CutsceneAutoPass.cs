using UnityEngine;
using UnityEngine.Playables;

public class CutsceneAutoPass : MonoBehaviour
{
    private PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        director.stopped += Director_stopped;
    }

    private void Director_stopped(PlayableDirector obj)
    {
        SceneLoader.Instance.NextScene();
    }
}
