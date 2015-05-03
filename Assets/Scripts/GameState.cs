using UnityEngine;
using System;
using System.Collections;

public class GameState : MonoBehaviour {

  public string nextLevelName;
  public Vector2[] checkPoints;
  public float screenLeftBound = 0.02f;
  public float screenRightBound = 0.98f;

  GameObject[] movableObjects;
  int checkPointIdx = -1;

  GameObject player;
  Player2D playerScript;

  public enum State {BEGIN, END, PLAY, NOCONTROLS, END_ON_MOVABLE}
  State state = State.PLAY;

  float endTimer = 2.0f;

  MovableScript currentMovable;

  void Awake() {

  }

  void Start() {
    this.player = GameObject.Find("Dug");
    this.playerScript = this.player.GetComponent<Player2D>();
    this.movableObjects = GameObject.FindGameObjectsWithTag("Movable");
  }

  void Update() {
    if (this.state == State.END || this.state == State.END_ON_MOVABLE) {
      this.endTimer = Mathf.Max(0.0f, this.endTimer - Time.deltaTime);
      if (this.endTimer == 0.0f) {
        Application.LoadLevel(this.nextLevelName);
      }
    }
    if (this.currentMovable != null) {
      HandleCurrentMovable();
    }
  }

  public void SetCurrentMovable(MovableScript m) {
    this.currentMovable = m;
    this.state = State.NOCONTROLS;
  }

  void HandleCurrentMovable() {
    if (this.currentMovable.IsMoving()) {
      float shiftamt = this.currentMovable.GetShiftAmt();
      string axis = this.currentMovable.GetAxis();
      this.currentMovable.TranslateAmtInDir(shiftamt, axis);
      playerScript.TranslateAmtInDir(shiftamt, axis);
      playerScript.SetGrounded(true);
    } else {
      this.currentMovable = null;
      this.state = State.PLAY;
    }
  }

  public void ResetMovables() {
    foreach (GameObject movable in this.movableObjects) {
      movable.GetComponent<MovableScript>().Reset(checkPointIdx);
    }
  }

  bool approxEquals(float x, float y, float threshold = 0.001f) {
    return Mathf.Abs(x-y) <= threshold;
  }


  public void SetState(State s) {
    state = s;
  }

  public State GetState() {
    return state;
  }

  public float GetScreenLeftBound() {
    return screenLeftBound;
  }

  public float GetScreenRightBound() {
    return screenRightBound;
  }


}
