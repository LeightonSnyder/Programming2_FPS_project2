using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessScript : MonoBehaviour
{
    //Pub Floats
    public float health = 3;



    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        

        //if (health <= 0)
        //{
        //    gameManager.MessCleaned();
        //    Destroy(gameObject);
        //}
    }

    public void MessDamaged()
    {
        health--;
    }
}
