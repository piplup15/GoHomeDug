using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextAppear : MonoBehaviour {
	public float timeDelay;
	public float timePeriod;

	GUIStyle style;
	bool used;
	Text dialogue;
	GameState state;
	bool fade; //true is in, false is out
	float visibleTimer = 0.0f;
	float totalTime = 0.0f;

	// Initialize Fields
	void Awake () {
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
		totalTime += Time.deltaTime;
		if (!used && visibleTimer == 0.0f && totalTime > timeDelay) {
			Color c = dialogue.color;
			if (fade) { //while text fades in 
				state.SetState (GameState.State.NOCONTROLS);
				c.a = Mathf.Min(1.0f, c.a + 0.6f * Time.deltaTime);
				dialogue.color = c;
				if (dialogue.color.a == 1.0f) {
					fade = false;
					visibleTimer = timePeriod;
				}
			} else {
				c.a = Mathf.Max(0.0f, c.a - 2.0f * Time.deltaTime);
				dialogue.color = c;
				if (dialogue.color.a == 0.0f) {
					fade = true;
					used = true;
				}
			}
		}
		visibleTimer = Mathf.Max(0.0f, visibleTimer - Time.deltaTime);
	}
}
