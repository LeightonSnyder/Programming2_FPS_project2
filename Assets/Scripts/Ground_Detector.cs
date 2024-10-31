using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Detector : MonoBehaviour
{
    public bool touchingGround = true;

    private void Update()
    {
        Debug.Log(IsHittingTerrain());
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            touchingGround = true;
        }

    }
    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            touchingGround = false;
        }

    }
    public bool IsHittingTerrain()
    {
        //Collider col = GetComponent<Collider>();
        //Collider[] hits = Physics.OverlapBox(col.bounds.center, col.bounds.extents, transform.rotation);
        //foreach(Collider hit in hits)
        //{
        //    if (hit.CompareTag(tag))
        //    {
        //        return true;
        //    }
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //}
        //return false;

        if (touchingGround)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
