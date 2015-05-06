using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class doug_interact : MonoBehaviour {
	// Use this for initialization
	bool triggered;
	Vector3 pos;
	GUIStyle style;
	GUIText dialogue;


	void Start () {
		triggered = false;
		style = new GUIStyle ();
		style.fixedWidth = 200;
		style.wordWrap = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			triggered = false;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Vector3 position = this.transform.position;
			position.x--;
			this.transform.position = position;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Vector3 position = this.transform.position;
			position.x++;
			this.transform.position = position;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Vector3 position = this.transform.position;
			position.y++;
			this.transform.position = position;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Vector3 position = this.transform.position;
			position.y--;
			this.transform.position = position;
		}
	}
	

}
