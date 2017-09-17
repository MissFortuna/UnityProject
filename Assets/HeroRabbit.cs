using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRabbit : MonoBehaviour {
	public float speed=1;
    bool isGrounded = false;
    bool JumpActive = false;
    float JumpTime = 0f;
    public float MaxJumpTime = 2f;
    public float JumpSpeed = 2f;

    Transform heroParent = null;
    Rigidbody2D rabbitBody = null;
    SpriteRenderer sr = null;
	Animator animator = null;

    bool is_dead = false;
    public int currentHealth = 1;
    float time_to_wait = 0.0f;
    
    //bonus
    float red_state_time = 4.0f;
    public bool red_state = false;
    public bool is_big = false;
    public bool make_big = false;

    Rigidbody2D myBody = null;
	// Use this for initialization
	void Start () {
		myBody = this.GetComponent<Rigidbody2D> ();
        LevelController.current.setStartPosition(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void FixedUpdate () {
		//[-1, 1]
		float value = Input.GetAxis ("Horizontal");

		Animator animator = GetComponent<Animator> ();
		if (Mathf.Abs (value) > 0) {
			Vector2 vel = myBody.velocity;
			vel.x = value * speed;
			myBody.velocity = vel;
			animator.SetBool ("run", true);
			animator.SetBool ("idle", false);
		} else {
			animator.SetBool ("run", false);
			animator.SetBool ("idle", true);
		}

		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if(value < 0) {
			sr.flipX = true;
		} else if(value > 0) {
			sr.flipX = false;
		}

        Vector3 from = transform.position + Vector3.up * 0.3f;
        Vector3 to = transform.position + Vector3.down * 0.1f;
        int layer_id = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);
        if (hit)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            this.JumpActive = true;
           
        }
        if (this.JumpActive)
        {
            
            if (Input.GetButton("Jump"))
            {
                this.JumpTime += Time.deltaTime;
                animator.SetBool("jump", true);
                animator.SetBool("run", false);
                animator.SetBool("idle", false);
                if (this.JumpTime < this.MaxJumpTime)
                {
                    Vector2 vel = myBody.velocity;
                    vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
                    myBody.velocity = vel;
                }
            }
            else
            {
                this.JumpActive = false;
                this.JumpTime = 0;
                animator.SetBool("jump", false);
                animator.SetBool("idle", true);
            }
        }
        /*if (this.isGrounded)
        {
            animator.SetBool("jump", false);
        }
        else
        {
            animator.SetBool("jump", true);
        }
        */

        if (this.is_dead)
        {
            Dead();
        }

        //bonus task
        if (red_state)
        {
            red_state_time -= Time.deltaTime;
            sr.color = Color.red;
            if (red_state_time <= 0) red_state = false;
        }
        else sr.color = Color.white;

        if (!is_big && make_big)
        {
            this.transform.localScale = new Vector3(3, 3, 0);
            is_big = true;
        }
        else if (is_big && !make_big)
        {

            this.transform.localScale = new Vector3(2, 2, 0);
            is_big = false;
        }
    }
    
    static void SetNewParent(Transform obj, Transform new_parent)
    {
        if (obj.transform.parent != new_parent)
        {
            Vector3 pos = obj.transform.position;

            obj.transform.parent = new_parent;

            obj.transform.position = pos;
        }
    }
   public void Dead()
    {
        animator.SetBool("die", true);
        animator.SetBool("idle", false);
        time_to_wait -= Time.deltaTime;
        if (time_to_wait <= 0)
        {

            is_dead = false;
            animator.SetBool("die", false);
            LevelController.current.onRabitDeath(this);
        }
        else return;
    }
    public void EatMushroom()
    {
        if (!is_big && make_big)
        {
            this.transform.localScale = new Vector3(3, 3, 0);
            is_big = true;
        }
        else if (is_big && !make_big)
        {

            this.transform.localScale = new Vector3(2, 2, 0);
            is_big = false;
        }
    }
}
