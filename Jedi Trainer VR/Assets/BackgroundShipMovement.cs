using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundShipMovement : MonoBehaviour
{
    public float speed = 5;
    public float resetTime = 1000;
    public float zPoint = 100;
    private float resetTimer = 0;
    private Vector3 resetPoint;
    void Start() {
        resetPoint = transform.position;
    }
    void Update()
    {
        if (resetTimer < resetTime) {
            transform.position += transform.forward * Time.timeScale * speed*Random.Range(.8f,1.2f);
        } else if (resetTimer == resetTime) {
            transform.position = new Vector3(-3000,150,3000);
        }else {
            transform.position = new Vector3(resetPoint.x, 150, zPoint+Random.Range(-100,100));
            resetTimer = 0;
        }
        resetTimer++;
    }
}
