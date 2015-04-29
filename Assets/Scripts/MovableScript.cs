using UnityEngine;
using System;
using System.Collections;

public class MovableScript : MonoBehaviour {

  public float maxDistance;
  bool used = false;
  float distance = 0.0f;
  public float increment = -0.2f;
  public string direction = "y";

  public void TranslateAmtInDir(float amt, string dir) {
    Vector3 pos = this.transform.position;
    if (dir == "x") {
      pos.x += amt;
    } else {
      pos.y += amt;
    }
    this.transform.position = pos;
    distance += amt;
  }

  public void SetUsed(bool used) {
    this.used = used;
  }

  public bool IsMoving() {
    return this.used && (Mathf.Abs(this.distance) < this.maxDistance);
  }

  public void Reset() {
    TranslateAmtInDir(-this.distance, this.direction);
    this.used = false;
    this.distance = 0.0f;
  }

  public float GetIncrement() {
    return this.increment;
  }

  public string GetDirection() {
    return this.direction;
  }
}
