using UnityEngine;
using System;
using System.Collections;

public class Player2D : MonoBehaviour {

  // Movement-related variables
  Vector3 startPosition;
  float deltaVx = 0.5f;
  public float maxSpeed = 8f;
  public float jumpVelocity = 14f;
  public bool disableRight = false;
  public bool disableJump = false;
  float scaleSize;
  Vector3 prevVelocity;

  // Animation-related variables
  Animator animator;

  // Game state
  GameState gs;

  // Sprite Renderer
  SpriteRenderer sr;

  // Audiosources
  AudioSource walkAudio;
  AudioSource jumpAudio;
  AudioSource jumpLandAudio;
  AudioSource respawnAudio;

  // Use this for initialization
  void Awake () {
    this.scaleSize = this.transform.localScale.x;
    this.startPosition = this.transform.position;
    FindAudioSources();
  }

  void FindAudioSources() {
    foreach (AudioSource audio in GetComponents<AudioSource>()) {
      if (audio.clip.name == "Walking.mp3") {
        this.walkAudio = audio;
      } else if (audio.clip.name == "Jump.mp3") {
        this.jumpAudio = audio;
      } else if (audio.clip.name == "Jump_Landing.mp3") {
        this.jumpLandAudio = audio;
      } else if (audio.clip.name == "Spawn_Noise.mp3") {
        this.respawnAudio = audio;
      }
    }
  }


  // Use this for finding references to other components
  void Start () {
    this.gs = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    if (gs.GetState() == GameState.State.BEGIN || gs.GetState() == GameState.State.BEGIN_ON_MOVABLE) {
      Vector3 pos = Camera.main.WorldToViewportPoint (this.transform.position);
      pos.x = 1.02f;
      if (gs.GetState() == GameState.State.BEGIN_ON_MOVABLE) {
        pos.y = gs.GetBeginMovableScreenOffsetY();
      }
      this.transform.position = Camera.main.ViewportToWorldPoint(pos);
    }  
  }

  // Update is called once per frame
  void FixedUpdate() {
    UpdateVelocities();
    PlayAudio();
    MakeSpriteTransparent(this.gs.GetState() == GameState.State.RESPAWN);
  }

  /**************************************************************/
  /*                    MOVEMENT MECHANICS                      */
  /**************************************************************/

  // Update velocity on each timestep
  void UpdateVelocities() {
    Vector3 vel = rigidbody2D.velocity;
    bool right = (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d")) && !disableRight;
    bool left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a");
    bool up = (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w")) && !disableJump;
    if (gs.GetState() == GameState.State.PLAY) {
      HandleKeyMovements(left, right, up, ref vel);
      DampenXVelocity(left, right, ref vel);
      RestrictXVelocity(left, right, ref vel);
      TestCharacterGrounded(vel);
    } else if (gs.GetState() == GameState.State.BEGIN || gs.GetState() == GameState.State.BEGIN_ON_MOVABLE) {
      vel.x = (gs.GetState() == GameState.State.BEGIN) ? -this.maxSpeed : 0.0f; // Dug enters from right screen
      this.animator.SetBool("isIdle", false);
      if (this.transform.position.x < this.startPosition.x + gs.GetBeginMargin()) {
        gs.SetState(GameState.State.PLAY);
      }
    } else if (gs.GetState() == GameState.State.END) {
      vel.x = -this.maxSpeed; // Dug leaves left screen
      this.animator.SetBool("isIdle", false);
    } else if (gs.GetState() == GameState.State.NOCONTROLS) {
      vel.x = 0.0f;
      RestrictXVelocity(left, right, ref vel);
      this.animator.SetBool("isIdle", true);
    } else if (gs.GetState() == GameState.State.RESPAWN) {
      vel.x = 0.0f;
      this.animator.SetBool("isIdle", true);
    }
    prevVelocity = vel;
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
    if (up && this.animator.GetBool("grounded")) {
      vel.y = jumpVelocity;
      this.animator.SetBool("grounded", false);
      this.animator.SetBool("justJumped", true);
      this.jumpAudio.Play();
    }
  }

  // Dampen x velocity if left and keys are not pressed and character is on floor.
  void DampenXVelocity(bool left, bool right, ref Vector3 vel) {
    if (!right && !left && this.animator.GetBool("grounded")) {
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
    if (pos.x <= gs.GetScreenLeftBound()) {
      if (left || (!this.animator.GetBool("grounded") && vel.x < 0)) {
        vel.x = 0.0f;
      }
      if (this.animator.GetBool("grounded")) {
        if (gs.GetState() == GameState.State.PLAY) {
          gs.SetState(GameState.State.END); // regular ending, Dug walks left
        } else if (gs.GetState() == GameState.State.NOCONTROLS) {
          gs.SetState(GameState.State.END_ON_MOVABLE);
        }
      }
    } else if (pos.x >= gs.GetScreenRightBound()) {
      if (right || (!this.animator.GetBool("grounded") && vel.x > 0)) {
        vel.x = 0.0f;
      }
    } 
  }

  // Handle case where character falls off platform without jumping
  void TestCharacterGrounded(Vector3 vel) {
    if (!approxEquals(vel.y, 0.0f, 0.1f) && this.animator.GetBool("grounded")) {
      this.animator.SetBool("grounded", false);
    }
  }

  void PlayAudio() {
    if (!this.animator.GetBool("isIdle") && this.animator.GetBool("grounded")) {
      if (!walkAudio.isPlaying) {
        walkAudio.Play();
      }
    } else {
      walkAudio.Stop();
    }
  }

  // Get Starting Position of player
  public Vector3 GetStartPosition() {
    return this.startPosition;
  }

  // Set state of grounded flag
  public void SetGrounded(bool grounded) {
    this.animator.SetBool("grounded", grounded);
  }

  // Change color of sprite
  void MakeSpriteTransparent(bool makeTransparent) {
    if (makeTransparent) {
      this.sr.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Max(this.sr.color.a - 0.02f, 0.0f));
    } else { 
      this.sr.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Min(this.sr.color.a + 0.02f, 1.0f));
    }
  }

  // Change direction of character.
  void ChangeScaleX(float dir) {
    Vector3 scale = transform.localScale;
    scale.x = dir * scaleSize;
    transform.localScale = scale; 
  }

  void OnCollisionStay2D (Collision2D col) {
    foreach (ContactPoint2D c in col.contacts) {
      if (c.normal.y > 0.5f) {
        this.animator.SetBool("grounded", true);
      }
    }
  }

  void OnCollisionExit2D (Collision2D col) {
    if (col.gameObject.tag == "Movable") {
      if (col.contacts[0].normal.y > 0.9f) {
        MovableScript m = col.gameObject.GetComponent<MovableScript>();
        if (m.IsUsable()) {
          m.ResetFlip();
        }
      }
    }
  }


  // Detect collision with player and dead bodies
  void OnCollisionEnter2D (Collision2D col) {
    if (col.gameObject.tag == "Enemy") {
      gs.SetState(GameState.State.RESPAWN);
      if (col.contacts[0].normal.y < 0.9f) {
        prevVelocity.y = 0;
      }
      this.rigidbody2D.velocity = prevVelocity;
      gs.TurnOnEnemyColliders(false); // probably should have used a trigger.....
    }
    if (col.gameObject.tag == "Movable") {
      if (col.contacts[0].normal.y > 0.9f) {
        MovableScript m = col.gameObject.GetComponent<MovableScript>();
        if (m.IsUsable()) {
          gs.SetCurrentMovable(m);
        }
      }
    }
    if (col.contacts[0].normal.y > 0.9f) {
      this.animator.SetBool("justJumped", false);
      if (!this.animator.GetBool("grounded")) {
        this.jumpLandAudio.Play();
      }
    }
  }


  public void Reset() {
    this.rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
    ChangeScaleX(1.0f); // Make dug face left
    this.respawnAudio.Play();
    gs.ResetMovables();
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
  /*                      UTIL FUNCTIONS                        */
  /**************************************************************/

  bool approxEquals(float x, float y, float threshold = 0.001f) {
    return Mathf.Abs(x-y) <= threshold;
  }

}
