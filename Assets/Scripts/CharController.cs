using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharController : MonoBehaviour {
    Animator animator;

    public int speed;
    public int startingHealth = 100;
    public Slider healthSlider;

    private bool _isGrounded = true;
    bool _isPlaying_crouch = false;
    bool _isPlaying_walk = false;
    bool damaged;
    bool isDead;
    public int currentHealth;

    const int STATE_IDLE = 0;
    const int STATE_WALK = 1;
    const int STATE_CROUCH = 2;
    const int STATE_JUMP = 3;

    string _currentDirection = "left";
    int _currentAnimationState = STATE_IDLE;

    // Use this for initialization
    void Start () {
        currentHealth = startingHealth;
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate() {

        healthSlider.value = currentHealth;

        //Check for keyboard input
        if (Input.GetKeyDown(KeyCode.Space)) {
			//
			animator.Play("alucard-crouch");
        } else if (Input.GetKey("up") && !_isPlaying_crouch) {
            if (_isGrounded) {
                _isGrounded = false;
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 200));
                changeState(STATE_JUMP);
            }
        } else if (Input.GetKey("down")) {
            changeState(STATE_CROUCH);
        } else if (Input.GetKey("right")) {
            changeDirection("right");
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (_isGrounded) {
                changeState(STATE_WALK);
            }
        } else if (Input.GetKey("left")) {
            changeDirection("left");
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (_isGrounded) {
                changeState(STATE_WALK);
            }
        } else {
            if (_isGrounded) {
                changeState(STATE_IDLE);
            }
        }

        //check if crouch animation is playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("alucard-crouch")) {
            _isPlaying_crouch = true;
        } else {
            _isPlaying_crouch = false;
        }

        //check if strafe animation is playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("alucard-walking")) {
            _isPlaying_walk = true;
        } else {
            _isPlaying_walk = false;
        }

    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.CompareTag("Floor")) {
            _isGrounded = true;
            changeState(STATE_IDLE);
        }
    }


    void changeState(int state) {
        if (_currentAnimationState == state)
            return;

        switch (state) {

            case STATE_WALK:
                animator.Play("alucard-walking");
                //animator.SetInteger("state", STATE_WALK);
                break;

            case STATE_CROUCH:
                animator.Play("alucard-crouch");
                //animator.SetInteger("state", STATE_CROUCH);
                break;

            case STATE_JUMP:
                animator.Play("alucard-jumping");
                break;

            case STATE_IDLE:
                animator.CrossFade("alucard-idle", 0.5f);
                break;
        }
        _currentAnimationState = state;
    }

    void changeDirection(string direction) {

        if (_currentDirection != direction) {
            if (direction == "right") {
                _currentDirection = "right";
				this.GetComponent<SpriteRenderer> ().flipX = false;
            } else if (direction == "left") {
				this.GetComponent<SpriteRenderer> ().flipX = true;
                _currentDirection = "left";
            }
        }

    }


    public void TakeDamage(int amount) {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        //playerAudio.Play();
        if (currentHealth <= 0 && !isDead){
            //Death();
        }
    }

}
