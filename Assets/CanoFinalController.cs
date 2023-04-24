using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CanoFinalController : MonoBehaviour
{

    public GameObject notComplete; // Painel dentro do próprio prefab
    public GameObject FinalizouoJogo; // Puxar da HUD UIGameplay se caso gerar erro na cena final

    public UiDetectGamepad gamepad;

    private void Start()
    {
        gamepad.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            
            player.isInteraction = true;
            player.isfinalParte = true;

            gamepad.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.isInteraction = false;
            player.isfinalParte = false;
            notComplete.SetActive(false);

            gamepad.gameObject.SetActive(false);
        }

    }

   
}
