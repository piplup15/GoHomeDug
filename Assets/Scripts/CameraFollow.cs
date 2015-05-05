using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

  GameObject player;
  float leftBound = 0f; // Assume map always ends at x = 0
  float rightBound;
  float offset;

  // Use this for initialization
  void Start () {
    player = GameObject.Find("Dug");
    rightBound = player.GetComponent<Player2D>().GetStartPosition().x;
	offset = 10;
  }
  
  // Update camera to follow player
  void FixedUpdate () {
    Vector3 myPos = transform.position;
    myPos.x = Mathf.Min(player.transform.position.x, rightBound);
    myPos.x = Mathf.Max(myPos.x, leftBound);
    myPos.y = player.transform.position.y + offset;
    transform.position = myPos;
  }
}
