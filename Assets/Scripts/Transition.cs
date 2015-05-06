using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour {
  public string content;
  Vector3 pos;
  GUIStyle style;
  GameState state;

  AudioClip transitionClip;

  // Use this for initialization
  void Start () {
    state = GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState> ();
    style = new GUIStyle ();
    style.alignment= TextAnchor.MiddleCenter;
    style.fontSize=30;
    style.font = this.GetComponent<GUIText>().font;
    style.wordWrap = true;
    style.normal.textColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    transitionClip = Resources.Load("Scene_Transiton.mp3") as AudioClip;
    AudioSource.PlayClipAtPoint(transitionClip, new Vector3(0.0f, 0.0f, 1.0f), 0.08f);
  }
  
  // Update is called once per frame
  void FixedUpdate () {
    Color c = style.normal.textColor;
    c.a = Mathf.Min(1.0f, c.a + 0.6f * Time.deltaTime);
    style.normal.textColor = c;
    if (c.a == 1.0f) {
      state.SetState (GameState.State.END);
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
