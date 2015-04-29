using UnityEngine;
using System;
using System.Collections;

public class LiftScript : MonoBehaviour {

  public float maxDistance;
  bool used = false;
  float distance = 0.0f;

  public void TranslateY(float amt) {
    Vector3 pos = this.transform.position;
    pos.y += amt;
    this.transform.position = pos;
    distance += Math.Abs(amt);
  }

  public void SetUsed(bool used) {
    this.used = used;
  }

  public bool IsMoving() {
    return this.used && (this.distance < this.maxDistance);
  }

  public void Reset() {
    TranslateY(this.distance); // Assume this always translates upwards.
    this.used = false;
    this.distance = 0.0f;
  }
}
