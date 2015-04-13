using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

  bool isJump = false;
  Vector3 startPosition;
  float deltaVx = 0.3f;
  float maxSpeed = 3f;
  float jumpVelocity = 5f;
  float scaleSize;

  // Use this for initialization
  void Awake () {
    scaleSize = transform.localScale.x;
    startPosition = transform.position;
  }

  // Use this for finding references to other components
  void Start () {
  
  }
  
  // Update is called once per frame
  void Update() {
    Debug.Log(isJump);
    Debug.Log(rigidbody.velocity);
    UpdateVelocities();
  }

  // Update velocity on each timestep
  void UpdateVelocities() {
    Vector3 vel = rigidbody.velocity;
    bool right = Input.GetKey(KeyCode.RightArrow);
    bool left = Input.GetKey(KeyCode.LeftArrow);
    bool up = Input.GetKey(KeyCode.UpArrow);
    HandleKeyMovements(left, right, up, ref vel);
    DampenAndRestrictXVelocity(left, right, ref vel);
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
  // Also, test if character is at ends of map
  void DampenAndRestrictXVelocity(bool left, bool right, ref Vector3 vel) {
    // Dampen case
    if (!right && !left && !isJump) {
      if (vel.x < 0) {
        vel.x = Mathf.Min(vel.x + deltaVx, 0.0f);
      } else {
        vel.x = Mathf.Max(vel.x - deltaVx, 0.0f);
      }
    }
    // Restrict movement case
    Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
    float boundLimit = 0.03f; // TODO(alwong): make 0.025f a parameter, and fix for dug
    if (pos.x <= boundLimit && (left || isJump)) {  
      vel.x = 0.0f;
    } else if (pos.x >= (1-boundLimit) && (right || isJump)) {
      vel.x = 0.0f;
    }
  }

  // Handle case where character falls off platform without jumping
  void TestCharacterIsJump(Vector3 vel) {
    if (approxEquals(vel.y, 0.0f) && !isJump) {
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
      Debug.Log(c.normal.y);
      if (c.normal.y > 0.5f) {
        isJump = false;
      }
    }
  }

  // Detect collision with player and dead bodies
  void OnCollisionEnter (Collision col) {
    if (col.gameObject.tag == "Enemy") {
      transform.position = startPosition; // (TODO) alwong; handle this more graceful
    }
  }


  // Perhaps put this in a utility function file?
  bool approxEquals(float x, float y, float threshold=0.001f) {
    return Mathf.Abs(x-y) >= threshold;
  }
}
