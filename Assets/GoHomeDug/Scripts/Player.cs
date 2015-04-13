using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

  bool isJump = false;
  float deltaVx = 0.3f;
  float maxSpeed = 3f;
  float jumpVelocity = 5f;
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
    bool right = Input.GetKey(KeyCode.RightArrow);
    bool left = Input.GetKey(KeyCode.LeftArrow);
    bool up = Input.GetKey(KeyCode.UpArrow);
    HandleKeyMovements(left, right, up, ref vel);
    DampenXVelocity(left, right, ref vel);
    TestCharacterIsJump(vel);
    rigidbody.velocity = vel;
  }

  // Update velocity based on left, right, up press
  void HandleKeyMovements(bool left, bool right, bool up, ref Vector3 vel) {
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
  }

  // Dampen x velocity if left and keys are not pressed and character is on floor.
  void DampenXVelocity(bool left, bool right, ref Vector3 vel) {
    if (!right && !left && !isJump) { // dampen x velocity
      if (vel.x < 0) {
        vel.x = Mathf.Min(vel.x + deltaVx, 0.0f);
      } else {
        vel.x = Mathf.Max(vel.x - deltaVx, 0.0f);
      }
    }
  }

  // Handle case where character falls off platform without jumping
  void TestCharacterIsJump(Vector3 vel) {
    if (vel.y != 0.0f && !isJump) {
      isJump = true;
    }
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
      if (c.normal.y > 0.9f) {
        isJump = false;
      }
    }
  }
}
