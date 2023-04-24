using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTelaInicial : MonoBehaviour
{
    public GameObject TelaInicial;
    // Start is called before the first frame update
    private void inicial()
    {
        TelaInicial.SetActive(true);
    }

    private void voltarInvisible()
    {
        TelaInicial.SetActive(false);
    }
}
