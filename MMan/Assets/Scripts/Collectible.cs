using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
    int _points;

    // Use this for initialization
    void Start()
    {
        points = 10;
    }

    public int points
    {
        get { return _points; }
        set { _points = value; }
    }
}
