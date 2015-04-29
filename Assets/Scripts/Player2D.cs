using UnityEngine;
using System;
using System.Collections;

public class Player2D : MonoBehaviour {

  // Movement-related variables
  bool grounded = false;
  Vector3 startPosition;
  float deltaVx = 0.5f;
  public float maxSpeed = 6f;
  public float jumpVelocity = 15f;
  float scaleSize;

  // Controls-related variables
  bool noControls = false;
  bool endLevel = false;
  bool beginLevel = false;

  // Time-related variables
  float endTimer;
  float endTime = 2.0f; 

  // Animation-related variables
  Animator animator;

  // Level-Dependent Variables
  GameObject[] movableObjects;

  // Use this for initialization
  void Awake () {
    this.scaleSize = this.transform.localScale.x;
    this.startPosition = this.transform.position;
    this.animator = GetComponent<Animator>();
  }

  // Use this for finding references to other components
  void Start () {
    this.movableObjects = GameObject.FindGameObjectsWithTag("Movable");
  }

  // Update is called once per frame
  void FixedUpdate() {
    UpdateVelocities();
    TimeUpdate();
  }


  /**************************************************************/
  /*                      TIMER UPDATES                         */
  /**************************************************************/

  void TimeUpdate() {
    if (this.endLevel) {
      this.endTimer = Mathf.Max(0.0f, this.endTimer - Time.deltaTime);
      if (this.endTimer == 0.0f) {
        ChangeLevels();
      }
    }
  }


  /**************************************************************/
  /*                    MOVEMENT MECHANICS                      */
  /**************************************************************/

  // Update velocity on each timestep
  void UpdateVelocities() {
    Vector3 vel = rigidbody2D.velocity;
    bool right = Input.GetKey(KeyCode.RightArrow);
    bool left = Input.GetKey(KeyCode.LeftArrow);
    bool up = Input.GetKey(KeyCode.UpArrow);
    if (!noControls) {
      HandleKeyMovements(left, right, up, ref vel);
      DampenXVelocity(left, right, ref vel);
      RestrictXVelocity(left, right, ref vel);
      TestCharacterGrounded(vel);
    } else if (endLevel || beginLevel) {
      vel.x = -this.maxSpeed; // Dug leaves left screen
      vel.y = 0.0f;
      this.rigidbody2D.gravityScale = 0.0f;
      Debug.Log(this.rigidbody2D.gravityScale);
    } else {
      vel.x = 0.0f;
      vel.y = 0.0f;
      RestrictXVelocity(left, right, ref vel);
      this.animator.SetBool("isIdle", true);
    }
    rigidbody2D.velocity = vel;
  }

  // Update velocity based on left, right, up press
  void HandleKeyMovements(bool left, bool right, bool up, ref Vector3 vel) {
    if (right) {
      vel.x = Mathf.Min(vel.x + deltaVx, maxSpeed); 
      ChangeScaleX(-1.0f); // Cause Dug faces left normally
      this.animator.SetBool("isIdle", false);
    }
    if (left) {
      vel.x = Mathf.Max(vel.x - deltaVx, -maxSpeed); 
      ChangeScaleX(1.0f);
      this.animator.SetBool("isIdle", false);
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
      if (vel.x == 0.0f) {
        this.animator.SetBool("isIdle", true);
      }
    }
  }

  // Test if character is at ends of map
  void RestrictXVelocity(bool left, bool right, ref Vector3 vel) {
    Vector3 pos = Camera.main.WorldToViewportPoint (this.transform.position);
    float boundLimit = 0.02f; // TODO(alwong): make a parameter, and fix for dug
    if (pos.x <= boundLimit) {
      if (left || (!this.grounded && vel.x < 0)) {
        vel.x = 0.0f;
      }
      if (this.grounded) {
        this.noControls = true;
        this.endTimer = this.endTime;
        this.endLevel = true;
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

  // Get Starting Position of player
  public Vector3 GetStartPosition() {
    return this.startPosition;
  }


  // Change direction of character.
  void ChangeScaleX(float dir) {
    Vector3 scale = transform.localScale;
    scale.x = dir * scaleSize;
    transform.localScale = scale; 
  }

  // Detect whether character is on ground
  void OnCollisionStay2D (Collision2D col) {
    foreach (ContactPoint2D c in col.contacts) {
      if (c.normal.y > 0.5f) {
        this.grounded = true;
      }
    }
    if (col.gameObject.tag == "Movable") {
      MovableScript m = col.gameObject.GetComponent<MovableScript>();
      m.SetUsed(true);
      this.noControls = m.IsMoving();
      if (m.IsMoving()) {
        float shiftamt = m.GetIncrement();
        m.TranslateAmtInDir(shiftamt, m.GetDirection());
        this.TranslateAmtInDir(shiftamt, m.GetDirection());
        this.grounded = true;
      } else {
        this.noControls = false;
      }
    }
  }

  // Detect collision with player and dead bodies
  void OnCollisionEnter2D (Collision2D col) {
    if (col.gameObject.tag == "Enemy") {
      this.transform.position = this.startPosition; // (TODO) alwong; handle this more graceful
      this.rigidbody2D.velocity = new Vector2(0.0f, 0.0f);

      // If there are any movables, reset them:
      foreach (GameObject movable in this.movableObjects) {
        movable.GetComponent<MovableScript>().Reset();
      }
    }
  }

  // Translate amt units in a direction
  public void TranslateAmtInDir(float amt, string dir) {
    Vector3 pos = this.transform.position;
    if (dir == "x") {
      pos.x += amt;
    } else {
      pos.y += amt;
    }
    this.transform.position = pos;
  }


  /**************************************************************/
  /*                      LEVEL CONTROLS                        */
  /**************************************************************/

  string[] levels = 
    new string[6] {"ThroneRoom",
                   "Tutorial",
                   "OutsideCastle",
                   "Level1",
                   "Level2",
                   "PathToCliffside"};

  void ChangeLevels() {
    int idx = Array.IndexOf(levels, Application.loadedLevelName);
    Application.LoadLevel(levels[idx+1]);
  }


  /**************************************************************/
  /*                      UTIL FUNCTIONS                        */
  /**************************************************************/

  // Perhaps put this in a utility function file?
  bool approxEquals(float x, float y, float threshold = 0.001f) {
    return Mathf.Abs(x-y) <= threshold;
  }

}
