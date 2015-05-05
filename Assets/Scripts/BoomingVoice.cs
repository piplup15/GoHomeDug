using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoomingVoice : MonoBehaviour {
	bool triggered;
	Text dialogue;
	// Use this for initialization
	void Start () {
		triggered = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "God") {
			triggered = true;
			if (other.gameObject.GetComponent<Text> () != dialogue) {
				if (dialogue != null) {
					Color h = dialogue.color;
					h.a = 0;
					dialogue.color = h;
				}
				dialogue = other.gameObject.GetComponent<Text> ();
				Color c = dialogue.color;
				c.a = 255;
				dialogue.color = c;
			}
		}
	}
}
