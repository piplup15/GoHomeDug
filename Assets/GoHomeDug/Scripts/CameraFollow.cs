using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

  protected GameObject player;

  // Use this for initialization
  protected void Start () {
    player = GameObject.Find("basketball");
  }
  
  protected void FixedUpdate () {
    Vector3 myPos = transform.position;
    myPos.x = player.transform.position.x;
    myPos.y = player.transform.position.y;
    transform.position = myPos;
  }
}
