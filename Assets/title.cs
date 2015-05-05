using UnityEngine;
using System.Collections;

public class title : MonoBehaviour {
	string content;
	Vector3 pos;
	float time, standard;
	bool countdown;
	GUIStyle style;

	// Use this for initialization
	void Start () {
		standard = 0;
		content = "PLAY";
		countdown = false;
		style = new GUIStyle ();
		style.alignment= TextAnchor.MiddleCenter;
		style.fontSize=50;
		style.font = this.GetComponent<GUIText> ().font;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space) && standard == 0) {
			content = "Credits\n\nArtists:\nJeejun (J) and Tyler\n\nProgrammers:\nAlvin and Alana";
			time = Time.deltaTime;
			style.fontSize=30;
			standard = time*500;
			countdown = true;
		}
		if (countdown) {
			time += .01f;
			if (time > standard) {
				content = "The End";
				countdown=false;
				style.fontSize=100;
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
		print (pos);

	}
}
