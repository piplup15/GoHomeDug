using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class voice : MonoBehaviour {
	GUIStyle style;
	bool triggered;
	bool used;
	Text dialogue;
	GameState state;
	bool fade; //true is in, false is out

	// Initialize Fields
	void Awake () {
		triggered = false; // triggers transparent effects
		used = false; // one time use
		dialogue = GetComponent<Text> ();
		style = new GUIStyle ();
		style.wordWrap = true;
		fade = true;
	}

	// Use this for initialization
	void Start () {
		state = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!used && triggered) { //dug should not move
			Color c = dialogue.color;
			if (fade) { //while text fades in 
				state.SetState (GameState.State.NOCONTROLS);
				c.a = Mathf.Min(1.0f, c.a + 0.6f * Time.deltaTime);
				dialogue.color = c;
				if (dialogue.color.a == 1.0f) {
					fade = false;
					state.SetState (GameState.State.PLAY);
				}
			} else {
				c.a = Mathf.Max(0.0f, c.a - 0.6f * Time.deltaTime);
				dialogue.color = c;
				if (dialogue.color.a == 0.0f) {
					fade = true;
					used = true;
				}
			}
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			triggered = true;
		}
	}
}
