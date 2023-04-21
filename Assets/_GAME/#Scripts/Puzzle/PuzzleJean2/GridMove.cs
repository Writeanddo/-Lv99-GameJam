using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMove : MonoBehaviour
{
    [SerializeField] private float timeMoveGrid;
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IE_moveGrid());
    }


    private IEnumerator IE_moveGrid()
    {
        yield return new WaitForSeconds(timeMoveGrid);
        int randIndex = Random.Range(0, 4);
        PositionMoveGrid(randIndex);

    }

    private IEnumerator IE_MoveObjectOverTime(Vector3 offset, float duration)
    {
        Vector3 endPosition = transform.position + offset;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position += offset * (Time.deltaTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPosition;
        StartCoroutine(IE_moveGrid());

    }


    private void PositionMoveGrid(int direction)
    {
        switch (direction)
        {
            case 0: //UP
                StartCoroutine(IE_MoveObjectOverTime(Vector3.up * 0.1f, moveSpeed));

                break;
            case 1: //Right
                StartCoroutine(IE_MoveObjectOverTime(Vector3.right * 0.1f, moveSpeed));

                break;
            case 2: //Down
                StartCoroutine(IE_MoveObjectOverTime(Vector3.down * 0.1f, moveSpeed));
                break;
            case 3://Left
                StartCoroutine(IE_MoveObjectOverTime(Vector3.left * 0.1f, moveSpeed));
                break;
        }

    }
}
