using UnityEngine;
using System;
using System.Collections;

public class GameState : MonoBehaviour {

  public string nextLevelName;
  public float screenLeftBound = 0.02f;
  public float screenRightBound = 0.98f;
  public Vector2[] checkPoints; // err, not sure why this isn't showing up on the inspector...

  GameObject[] movableObjects;
  int checkPointIdx = -1;

  GameObject player;
  Player2D playerScript;

  public enum State {BEGIN, END, PLAY, NOCONTROLS, END_ON_MOVABLE, RESPAWN}
  State state = State.PLAY;

  float endTimer = 2.0f;
  float respawnTime = 0.0f;
  float respawnTimer;

  MovableScript currentMovable;

  void Awake() {
    respawnTimer = respawnTime;
    // Manually add checkpoints in code:
    if (Application.loadedLevelName == "Level1") {
      checkPoints = new Vector2[1];
      checkPoints[0] = new Vector2(184.0f, 52.0f);
    } else if (Application.loadedLevelName == "Level2") {
      checkPoints = new Vector2[2];
      checkPoints[0] = new Vector2(133.0f, 106.0f);
      checkPoints[1] = new Vector2(264.0f, 4.0f);
    }
  }

  void Start() {
    this.player = GameObject.Find("Dug");
    this.playerScript = this.player.GetComponent<Player2D>();
    this.movableObjects = GameObject.FindGameObjectsWithTag("Movable");
  }

  void Update() {
    if (this.state == State.END || this.state == State.END_ON_MOVABLE) {
      HandleEndGameTimer();
    } else if (this.state == State.RESPAWN) {
      HandleRespawnTimer();
    } 
    if (this.currentMovable != null) {
      HandleCurrentMovable();
    }
    HandleCheckPoints();
  }

  void HandleEndGameTimer() {
    this.endTimer = Mathf.Max(0.0f, this.endTimer - Time.deltaTime);
    if (this.endTimer == 0.0f) {
      Application.LoadLevel(this.nextLevelName);
    }
  }

  void HandleRespawnTimer() {
    this.respawnTimer = Mathf.Max(0.0f, this.respawnTimer - Time.deltaTime);
    if (this.respawnTimer == 0.0f) {
      respawnTimer = respawnTime;
      this.state = State.PLAY;
      if (checkPointIdx == -1) {
        player.transform.position = playerScript.GetStartPosition();
      } else {
        player.transform.position = this.checkPoints[checkPointIdx];
      }
      playerScript.ResetVelocity();
    }
  }

  void HandleCheckPoints() {
    for (int i = 0; i < checkPoints.Length; i++) {
      if (approxEquals(player.transform.position.x, checkPoints[i].x, 1.0f)) {
        if (approxEquals(player.transform.position.y, checkPoints[i].y, 10.0f)) {
          checkPointIdx = Mathf.Max(checkPointIdx, i);
        }
      }
    }
  }

  public void SetCurrentMovable(MovableScript m) {
    this.currentMovable = m;
    this.currentMovable.GetAudioSource().Play();
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
      this.currentMovable.GetAudioSource().Stop();
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
