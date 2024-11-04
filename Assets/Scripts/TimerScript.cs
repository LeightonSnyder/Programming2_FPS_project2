using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TimerScript : MonoBehaviour
{
    public TextMeshProUGUI timeRemainingText;
    public float timeLeft;
    public bool timerOn;
    public int GameOverScene;

    public SceneChanger sceneChanger;

    // Start is called before the first frame update
    private void Awake()
    {
        sceneChanger = GetComponent<SceneChanger>();
    }
    void Start()
    {
        timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            if(timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                updateTimer(timeLeft);
            }
            else
            {
                timeLeft = 0;
                timerOn = false;
                sceneChanger.MoveToScene(GameOverScene);
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        if (seconds < 10)
        {
            timeRemainingText.text = minutes + ":0" + seconds;
        }
        else
        {
            timeRemainingText.text = minutes + ":" + seconds;
        }
        
    }
}
