using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

  protected GameObject player;
  protected float leftBound = 0f; // Assume map always ends at x = 0
  protected float rightBound;

  // Use this for initialization
  protected void Start () {
    player = GameObject.Find("basketball");
    rightBound = player.transform.position.x;
  }
  
  // Update camera to follow player
  protected void FixedUpdate () {
    Vector3 myPos = transform.position;
    myPos.x = Mathf.Min(player.transform.position.x, rightBound);
    myPos.x = Mathf.Max(myPos.x, leftBound);
    myPos.y = player.transform.position.y;
    transform.position = myPos;
  }
}
