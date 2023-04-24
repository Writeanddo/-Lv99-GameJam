using UnityEngine;
using UnityEngine.Playables;

public class CutsceneAutoPass : MonoBehaviour
{
    private PlayableDirector director;

    private bool next;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (director.state == PlayState.Paused && next == false)
        {
            next = true;
            SceneLoader.Instance.NextScene();
        }
    }
}
