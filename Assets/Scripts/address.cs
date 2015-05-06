using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class address : MonoBehaviour {
	GUIStyle style;
	bool triggered;
	Text dialogue;
	GameState state;
	bool fade; //true is in, false is out
	
	// Use this for initialization
	void Start () {
		triggered = false;
		style = new GUIStyle ();
		style.wordWrap = true;
		state = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();
		fade = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (triggered) { //dug should not move
			if (fade) { //while text fades in 
				Color c = dialogue.color;
				c.a += 0.1f * Time.deltaTime * 6;
				dialogue.color = c;
				//print ("text fades in");
				if (dialogue.color.a >= 1) {
					fade = false; //false
					//print ("dialogue.color.a >= 1");
				}
			}
			
			else { //or text fades out
				state.SetState (GameState.State.PLAY);
				Color c = dialogue.color;
				c.a -= 0.1f * Time.deltaTime * 6;
				dialogue.color = c;
				//print ("text fades out");
				if (dialogue.color.a <= 0){ //done
					fade = true; //true
					triggered = false;
					//print ("dialogue.color.a <= 0");
					dialogue = null;
				}
			}
		}
		
	}
	
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "God") {
			triggered = true;
			if (other.gameObject.GetComponent<Text> () != dialogue) {
				dialogue = other.gameObject.GetComponent<Text> ();
				Color c = dialogue.color;
				c.a = 0;
				dialogue.color = c;
				state.SetState (GameState.State.NOCONTROLS);
			}
		}
	}
	
	
}
