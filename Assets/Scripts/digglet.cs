using UnityEngine;
using System.Collections;

public class digglet : MonoBehaviour {
	// Use this for initialization

	bool triggered;
	string trig;
	GUIStyle style;

	void Start () {
		triggered = false;
		trig = "";
		style = new GUIStyle ();
		style.richText = GetComponent<GUIText> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			print ("fwoosh!");
			triggered = false;
			trig = "";
		}
	}

	// when triggered, text shows up
	void OnTriggerEnter2D(Collider2D other) {
		print ("crash!");
		triggered = true;
		trig = other.name;
	}
	
	void OnGUI() {
		if (triggered) {
			switch (trig)
			{
			case "one":
				GUI.Box(new Rect(80,100,500,500), "Thanks for saving us Doug!");
				break;
			case "two":
				GUI.Box(new Rect(80,100,500,500), "Thanks for saving us Doug!");
				break;
			case "three":
				GUI.Box(new Rect(80,100,500,500), "Thanks for saving us Doug!");
				break;
			case "four":
				GUI.Box(new Rect(80,100,500,500), "Thanks for saving us Doug!");
				break;
			case "five":
				GUI.Box(new Rect(80,100,500,500), "Thanks for saving us Doug!");
				break;
			case "six":
				GUI.Box(new Rect(80,100,500,500), "Thanks for saving us Doug!");
				break;
			default:
				break;
			}
		}
	}
}