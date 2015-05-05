using UnityEngine;
using System;
using System.Collections;

public class GameState : MonoBehaviour {

  public string nextLevelName;
  public float screenLeftBound = 0.02f;
  public float screenRightBound = 0.98f;
  public Vector2[] checkPoints; // err, not sure why this isn't showing up on the inspector...
  public float beginMargin = 0.5f;
  public float beginMovableScreenOffsetY = 0.62f;

  GameObject[] movableObjects;
  GameObject[] enemies;
  GameObject[] arrows;
  int checkPointIdx = -1;

  GameObject player;
  Player2D playerScript;

  public enum State {BEGIN, END, PLAY, NOCONTROLS, END_ON_MOVABLE, RESPAWN, BEGIN_ON_MOVABLE}
  public State state = State.BEGIN;

  float endTimer = 2.0f;
  float respawnTime = 3.0f;
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
    this.enemies = GameObject.FindGameObjectsWithTag("Enemy");
    this.arrows = GameObject.FindGameObjectsWithTag("Checkpoint");
  }

  void FixedUpdate() {
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
      TurnOnEnemyColliders(true);
      if (checkPointIdx == -1) {
        player.transform.position = playerScript.GetStartPosition();
      } else {
        player.transform.position = this.checkPoints[checkPointIdx];
      }
      playerScript.Reset();
    }
  }

  void HandleCheckPoints() {
    for (int i = 0; i < checkPoints.Length; i++) {
      if (approxEquals(player.transform.position.x, checkPoints[i].x, 1.0f)) {
        if (approxEquals(player.transform.position.y, checkPoints[i].y, 10.0f)) {
          checkPointIdx = Mathf.Max(checkPointIdx, i);
          FindClosestCheckpointObject().GetComponent<Animator>().SetBool("hasSpun", true);
        }
      }
    }
  }

  GameObject FindClosestCheckpointObject() {
    float bestDist = 99999f; // should be large enough
    GameObject bestArrow = null;
    foreach (GameObject arrow in this.arrows) {
      float dist = ManhattanDistance2D(arrow.transform.position, player.transform.position);
      if (dist < bestDist) {
        bestDist = dist;
        bestArrow = arrow;
      }
    }
    return bestArrow;
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
      this.currentMovable.GetAnimator().SetBool("isIdle", false);
      playerScript.TranslateAmtInDir(shiftamt, axis);
      playerScript.SetGrounded(true);
    } else {
      this.currentMovable.GetAudioSource().Stop();
      this.currentMovable.GetAnimator().SetBool("isIdle", true);
      this.currentMovable = null;
      this.state = State.PLAY;
    }
  }

  public void ResetMovables() {
    foreach (GameObject movable in this.movableObjects) {
      movable.GetComponent<MovableScript>().Reset(checkPointIdx);
    }
  }

  public void TurnOnEnemyColliders(bool on) {
    foreach (GameObject enemy in this.enemies) {
      foreach (Collider2D c in enemy.GetComponents<Collider2D>()) {
        c.enabled = on;
      }
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

  public float GetBeginMargin() {
    return beginMargin;
  }

  public float GetBeginMovableScreenOffsetY() {
    return beginMovableScreenOffsetY;
  }

  float ManhattanDistance2D(Vector3 v1, Vector3 v2) {
    return Mathf.Abs(v1.x - v2.x) + Mathf.Abs(v1.y - v2.y);
  }
}
