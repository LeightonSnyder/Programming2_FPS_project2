using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerLobby : MonoBehaviour
{



    // Update is called once per frame
    void Update()
    {
                
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

}
