using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenSystem : MonoBehaviour
{

    public PlayerController _PlayerController;
    private Animator oxigenAn;

    // Start is called before the first frame update
    void Start()
    {
        _PlayerController = FindAnyObjectByType(typeof(PlayerController))as PlayerController;
        oxigenAn = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_PlayerController.isOxygenStart == true)
        {
             oxigenAn.SetTrigger("start");
             fdsafdsf
             
          


        }
    }
}
