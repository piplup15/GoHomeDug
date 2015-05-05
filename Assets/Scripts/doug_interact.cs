using UnityEngine;
using System.Collections;

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
	
	// when triggered, text shows up
	void OnTriggerEnter2D(Collider2D other) {
		triggered = true;
		//pos = other.transform.position;
		//pos = Camera.main.WorldToViewportPoint(other.transform.position);
		pos = Camera.main.WorldToScreenPoint (other.transform.position);
		dialogue = other.GetComponent<GUIText> ();
		if (dialogue != null) {
			style.font = dialogue.font;
			style.fontSize = dialogue.fontSize;
			print(pos);
		}
	}
	
	void OnGUI() {
		if (triggered) {
			Rect r = new Rect();
			r.x = pos.x - style.fixedWidth/2;
			r.y = Screen.height - pos.y - style.CalcHeight(new GUIContent(dialogue.text), style.fixedWidth)*2;
			GUI.Box(r, dialogue.text, style);
			//GUI.Label(r, new GUIContent(dialogue.text),style);
		}
	}
}
