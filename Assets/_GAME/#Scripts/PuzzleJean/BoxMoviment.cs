using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMoviment : MonoBehaviour
{
    [SerializeField] Transform boxMoviment;
    public float initialScale;
    public float finalScale;
    public float speed;
    public float duration = 1f;
    [SerializeField] float timeDelay;

    void Start()
    {
     
            StartCoroutine(IE_ScaleObject());

        
    }


    private IEnumerator IE_ScaleObject()
    {
        yield return new WaitForSeconds(timeDelay);

        float elapsedTime = 0f;
        float scale = initialScale;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            scale = Mathf.Lerp(initialScale, finalScale, t * t);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        yield return new WaitForSeconds(timeDelay);
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            scale = Mathf.Lerp(finalScale, initialScale, t * t);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        transform.localScale = new Vector3(initialScale, initialScale, initialScale);

        StartCoroutine(IE_ScaleObject());
    }


}
