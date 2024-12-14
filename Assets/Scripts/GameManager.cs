using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int messesRemaining;
    public TextMeshProUGUI messesRemainingText;
    public GameObject WinTrigSpot;
    public GameObject WinTrigPrefab;
    public bool doorOpen = false;



    //private void Awake()
    //{
    //    WinTrigSpot = GetComponent<WinTrigSpot>();
    //}
    void Start()
    {
        var messes = GameObject.FindGameObjectsWithTag("Dirt");
        messesRemaining = messes.Length;
    }

    // Update is called once per frame
    void Update()
    {
                
        if (messesRemaining > 0)
        {
            messesRemainingText.text = "Spills Remaining: " + messesRemaining;
            
        }
        else
        {
            messesRemainingText.text = "Time to clock out!";

            if (doorOpen == false)
            {
                Instantiate(WinTrigPrefab, WinTrigSpot.transform.position, WinTrigSpot.transform.rotation);
                doorOpen = true;
            }
                
        }
        Debug.Log(WinTrigSpot);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public void MessCleaned()
    {
        messesRemaining--;
    }
}
