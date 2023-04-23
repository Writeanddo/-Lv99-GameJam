using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLocalController : MonoBehaviour
{

    public GameObject Puzzlechoice;
    public GameObject buttonIcon;



    private void Start()
    {
        buttonIcon.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.isInteraction = false;
            player.puzzleCurrent = null;
            
            buttonIcon.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.isInteraction = true;
            player.puzzleCurrent = Puzzlechoice;

            buttonIcon.SetActive(true);
          

        }

    }

    

}
