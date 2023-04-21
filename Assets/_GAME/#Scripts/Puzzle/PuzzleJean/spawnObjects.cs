using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnObjects : MonoBehaviour
{
    public GameObject prefabBullet;
    public Transform[] spawnPoints;
    public Transform targetObject;
    public float delayBullet;

    private void Start()
    {
        StartCoroutine(IE_spawnBullet());

    }


    public IEnumerator IE_spawnBullet()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        GameObject newObject = Instantiate(prefabBullet, spawnPoints[randomIndex].position, Quaternion.identity);
        newObject.GetComponent<bullet>().targetObject = targetObject;

        yield return new WaitForSeconds(delayBullet);

        StartCoroutine(IE_spawnBullet());



    }
}
