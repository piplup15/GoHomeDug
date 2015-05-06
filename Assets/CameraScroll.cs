using UnityEngine;
using System.Collections;

public class CameraScroll : MonoBehaviour {

	public float offsetY;
	
	GameObject player;
	//float upBound = -1000f; // Assume map ends at y = 1000
	//float downBound;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Dug");
		//downBound = player.GetComponent<Player2D>().GetStartPosition().y;

	}
	
	// Update camera to follow player
	void FixedUpdate () {
		Vector3 myPos = transform.position;
		//myPos.y = Mathf.Min(player.transform.position.y, downBound);
		//myPos.y = Mathf.Max(myPos.y, upBound);
		myPos.x = player.transform.position.x;
		myPos.y = player.transform.position.y;
		transform.position = myPos;
	}
}
