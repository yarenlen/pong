using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRacket : MonoBehaviour
{
    public float speed;
    public string axis;

    // Update is called once per frame
    void FixedUpdate()
    {
        float vert = Input.GetAxis(axis);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,vert)*speed; 
    }


}