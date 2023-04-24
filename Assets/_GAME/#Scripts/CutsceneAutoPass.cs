using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneAutoPass : MonoBehaviour
{
    private PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();

        double time = director.duration;

        StartCoroutine(CutsceneDuration(time));
    }

    private IEnumerator CutsceneDuration(double time)
    {
        yield return new WaitForSeconds((float)time);

        SceneLoader.Instance.NextScene();
    }
}
