using UnityEngine;
using System;
using System.Collections;

public class MovableScript : MonoBehaviour {

  public float maxDistance;
  public float increment = 0.2f;
  public string axis = "y";
  public int direction = -1;
  public int checkPointIdx = -1;
  public bool used = false;

  public enum ResetStatus {ALWAYS_DEFAULT, BASED_ON_CHECKPOINT, ONE_TIME}
  public ResetStatus resetStatus = ResetStatus.ALWAYS_DEFAULT;

  AudioSource audioSource;

  float distance = 0.0f;
  int defaultDirection;

  Vector3 defaultPosition;
  Vector3 usedPosition;

  void Awake() {
    audioSource = GetComponent<AudioSource>();
    defaultDirection = direction;
    defaultPosition = this.transform.position;
    usedPosition = this.transform.position;
    if (this.axis == "x") {
      usedPosition.x += direction * maxDistance;
    } else {
      usedPosition.y += direction * maxDistance;
    }
  }

  public void TranslateAmtInDir(float amt, string axis) {
    Vector3 pos = this.transform.position;
    if (axis == "x") {
      pos.x += amt;
    } else {
      pos.y += amt;
    }
    this.transform.position = pos;
    distance += Mathf.Abs(amt);
    this.used = true;
  }

  public bool IsUsable() {
    return !(this.used && this.resetStatus == ResetStatus.ONE_TIME);
  }

  public AudioSource GetAudioSource() {
    return audioSource;
  }

  public bool IsMoving() {
    return this.distance < this.maxDistance;
  }

  public void Reset(int cpIdx) {
    if (resetStatus == ResetStatus.ALWAYS_DEFAULT) {
      ResetToDefaultPosition();
    } else if (resetStatus == ResetStatus.ONE_TIME) {
      ResetToUsedPosition();
    } else {
      if (this.checkPointIdx < cpIdx) {
        ResetToUsedPosition();
      } else {
        ResetToDefaultPosition();
      }
    }
  }

  void ResetToDefaultPosition() {
    this.transform.position = defaultPosition;
    this.distance = 0.0f;
    this.direction = defaultDirection;
  }

  void ResetToUsedPosition() {
    this.transform.position = usedPosition;
    this.distance = 0.0f;
    this.direction = -defaultDirection;
  }

  public void ResetFlip() {
    this.distance = 0.0f;
    this.direction = -this.direction;
  }

  public float GetShiftAmt() {
    return this.direction * this.increment;
  }

  public string GetAxis() {
    return this.axis;
  }

}
