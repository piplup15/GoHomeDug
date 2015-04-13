using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

  bool isJump = false;
  float deltaVx = 0.5f;
  float maxSpeed = 4f;
  float jumpVelocity = 4f;
  float scaleSize;

  // Use this for initialization
  void Awake () {
    scaleSize = transform.localScale.x;
  }

  // Use this for finding references to other components
  void Start () {
  
  }
  
  // Update is called once per frame
  void Update() {
    UpdateVelocities();
  }

  // Update velocity on each timestep
  void UpdateVelocities() {
    Vector3 vel = rigidbody.velocity;
    Debug.Log(vel);
    bool right = Input.GetKey(KeyCode.RightArrow);
    bool left = Input.GetKey(KeyCode.LeftArrow);
    bool up = Input.GetKey(KeyCode.UpArrow);
    if (right) {
      vel.x = Mathf.Min(vel.x + deltaVx, maxSpeed); 
      ChangeScaleX(1.0f);
    }
    if (left) {
      vel.x = Mathf.Max(vel.x - deltaVx, -maxSpeed); 
      ChangeScaleX(-1.0f);
    }
    if (up && !isJump) {
      vel.y = jumpVelocity;
      isJump = true;
    }
    if (!right && !left && !isJump) { // dampen x velocity
      if (vel.x < 0) {
        vel.x = Mathf.Min(vel.x + deltaVx, 0.0f);
      } else {
        vel.x = Mathf.Max(vel.x - deltaVx, 0.0f);
      }
    }
    if (vel.y != 0.0f) {
      isJump = true;
    }
    rigidbody.velocity = vel;
  }

  // Change direction of character.
  void ChangeScaleX(float dir) {
    Vector3 scale = transform.localScale;
    scale.x = dir * scaleSize;
    transform.localScale = scale; 
  }

  // Detect whether character is on ground (and can jump)
  void OnCollisionStay (Collision col) {
    foreach (ContactPoint c in col.contacts) {
      if (c.normal.y > 0.8f) {
        isJump = false;
      }
    }
  }
}
