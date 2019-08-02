using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {

    private Character thePlayer;
	// Use this for initialization
	void Start () {
        thePlayer = FindObjectOfType<Character>();
	}

    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Player")
        {
            thePlayer.onLadder = true;
        }
    }
    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            thePlayer.onLadder = false;
        }
    }
}
