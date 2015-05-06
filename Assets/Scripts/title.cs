using UnityEngine;
using System.Collections;

public class title : MonoBehaviour {
	string content;
	Vector3 pos;
	float time, standard;
	int countdown;
	GUIStyle style;
	GameState state;

	// Use this for initialization
	void Start () {
		state = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();
		time = 0;
		standard = 0;
		content = "PLAY";
		countdown = 0;
		style = new GUIStyle ();
		style.alignment= TextAnchor.MiddleCenter;
		style.fontSize=50;
		style.font = this.GetComponent<GUIText> ().font;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//print (standard - time);
		if (Input.GetKey (KeyCode.Space) && standard == 0) {
			content = "Sound Effects Recorded by\nBlastwaveFx.com\nNPS.gov\nJuskiddink\nCaroline Ford\nMark DiAngelo";
			style.fontSize = 25;
			//standard = Time.deltaTime * 1500;
			standard = 1.0f; // 1 second?
			countdown += 1;
		} else {
			switch (countdown) {
			case 1:
				time += Time.deltaTime;
				if (time > standard) {
					content = "\"Sad Day\" and \"Better Days\"\nRoyalty Free Music from Bensound";
					time = 0;
					countdown += 1;
				}
				break;
			case 2:
				time += Time.deltaTime;
				if (time > standard) {
					content = "\"Lightless Dawn\"\nKevin MacLeod (incompetech.com)\n" +
						"Licensed under\nCreative Commons: By Attribution 3.0\n" +
						"creativecommons.org/licenses/by/3.0";
					time = 0;
					countdown += 1;
				}
				break;
			case 3:
				time += Time.deltaTime;
				if (time > standard) {
					content = "\"Spacial Harvest\"\nKevin MacLeod (incompetech.com)\n" +
						"Licensed under\nCreative Commons: By Attribution 3.0\n" +
						"creativecommons.org/licenses/by/3.0";
					time = 0;
					countdown += 1;
				}
				break;
			case 4:
				time += Time.deltaTime;
				if (time > standard) {
					content = "Credits\n\nArtists:\nJeejun (J) and Tyler\n\nProgrammers:\nAlvin and Alana";
					time = 0;
					countdown += 1;
				}
				break;
			case 5:
				time += Time.deltaTime;
				if (time > standard) {
					content = "The End";
					style.fontSize = 100;
					time = 0;
					countdown += 1;
				}
				break;
			case 6:
				time += Time.deltaTime;
				if (time > standard) {
					state.SetState (GameState.State.END);
				}
				break;
			}
		}
	}

	void OnGUI() {
		pos = Camera.main.WorldToScreenPoint (this.transform.position);
		Rect r = new Rect();
		r.x = pos.x;
		r.y = pos.y;
		//r.y = Screen.height - pos.y - style.CalcHeight(new GUIContent(content), style.fixedWidth)*2;
		GUI.Box(r, content, style);

	}
}
