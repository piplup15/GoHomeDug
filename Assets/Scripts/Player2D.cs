using UnityEngine;
using System.Collections;

public class Player2D : MonoBehaviour {

  bool grounded = false;
  Vector3 startPosition;
  float deltaVx = 0.5f;
  public float maxSpeed = 6f;
  public float jumpVelocity = 15f;
  float scaleSize;

  // Use this for initialization
  void Awake () {
    this.scaleSize = this.transform.localScale.x;
    this.startPosition = this.transform.position;
  }

  // Use this for finding references to other components
  void Start () {

  }

  // Update is called once per frame
  void FixedUpdate() {
    UpdateVelocities();
  }

  // Update velocity on each timestep
  void UpdateVelocities() {
    Vector3 vel = rigidbody2D.velocity;
    bool right = Input.GetKey(KeyCode.RightArrow);
    bool left = Input.GetKey(KeyCode.LeftArrow);
    bool up = Input.GetKey(KeyCode.UpArrow);
    MuteSlopes(left, right, ref vel);
    HandleKeyMovements(left, right, up, ref vel);
    DampenXVelocity(left, right, ref vel);
    RestrictXVelocity(left, right, ref vel);
    TestCharacterGrounded(vel);
    rigidbody2D.velocity = vel;
  }

  // Update velocity based on left, right, up press
  void HandleKeyMovements(bool left, bool right, bool up, ref Vector3 vel) {
    if (right) {
      vel.x = Mathf.Min(vel.x + deltaVx, maxSpeed); 
      ChangeScaleX(-1.0f); // Cause Dug faces left normally
    }
    if (left) {
      vel.x = Mathf.Max(vel.x - deltaVx, -maxSpeed); 
      ChangeScaleX(1.0f);
    }
    if (up && this.grounded) {
      vel.y = jumpVelocity;
      this.grounded = false;
    }
  }

  // Dampen x velocity if left and keys are not pressed and character is on floor.
  void DampenXVelocity(bool left, bool right, ref Vector3 vel) {
    if (!right && !left && this.grounded) {
      if (vel.x < 0) {
        vel.x = Mathf.Min(vel.x + deltaVx, 0.0f);
      } else {
        vel.x = Mathf.Max(vel.x - deltaVx, 0.0f);
      }
    }
  }

  // Test if character is at ends of map
  void RestrictXVelocity(bool left, bool right, ref Vector3 vel) {
    Vector3 pos = Camera.main.WorldToViewportPoint (this.transform.position);
    float boundLimit = 0.03f; // TODO(alwong): make a parameter, and fix for dug
    if (pos.x <= boundLimit) {
      if (left || (!this.grounded && vel.x < 0)) {
        vel.x = 0.0f;
      }
    } else if (pos.x >= (1-boundLimit)) {
      if (right || (!this.grounded && vel.x > 0)) {
        vel.x = 0.0f;
      }
    } 
  }

  // Handle case where character falls off platform without jumping
  void TestCharacterGrounded(Vector3 vel) {
    if (!approxEquals(vel.y, 0.0f, 0.1f) && grounded) {
      this.grounded = false;
    }
  }


  // Change direction of character.
  void ChangeScaleX(float dir) {
    Vector3 scale = transform.localScale;
    scale.x = dir * scaleSize;
    transform.localScale = scale; 
  }

  // Detect whether character is on ground
  void OnCollisionStay2D (Collision2D col) {
    Debug.Log("WAT2");
    foreach (ContactPoint2D c in col.contacts) {
      if (c.normal.y > 0.5f) {
        this.grounded = true;
      }
    }
  }

  // Detect collision with player and dead bodies
  void OnCollisionEnter2D (Collision2D col) {
    if (col.gameObject.tag == "Enemy") {
      this.transform.position = this.startPosition; // (TODO) alwong; handle this more graceful
    }
  }

  void MuteSlopes(bool left, bool right, ref Vector3 vel) {
    if (this.grounded) {
      RaycastHit2D hit = Physics2D.Raycast(this.transform.position, -Vector2.up, Mathf.Infinity, ~(1 << 8));
      if (hit && Mathf.Abs(hit.normal.x) > 0.1) {
        vel.x -= hit.normal.x * 0.6f; // What is this number?
        int sign = (vel.x - hit.normal.x > 0 ? 1 : -1);
        Vector3 pos = this.transform.position;
        pos.y += -hit.normal.x * Mathf.Abs(vel.x) * Time.deltaTime * sign;
        this.transform.position = pos;
      }
    }
  }


  // Perhaps put this in a utility function file?
  bool approxEquals(float x, float y, float threshold = 0.001f) {
    return Mathf.Abs(x-y) <= threshold;
  }
}
