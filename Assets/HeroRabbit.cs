using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRabbit : MonoBehaviour {
	public float speed=1;

	public static HeroRabbit lastRabbit = null;
	public float life=3;

	bool isGrounded = false;
    bool JumpActive = false;
    float JumpTime = 0f;
    public float MaxJumpTime = 2f;
    public float JumpSpeed = 2f;

	public float dieAnimationTime = 2;
	float timeLeftToDie;

    Transform heroParent = null;
    Rigidbody2D rabbitBody = null;
    SpriteRenderer sr = null;
	Animator animator = null;

    public bool is_dead = false;
    public int currentHealth = 1;
    float time_to_wait = 0.0f;
    float time_to_wait_die = 0.0f;
    //bonus
    float red_state_time = 4.0f;
    public bool red_state = false;
    public bool is_big = false;
    public bool make_big = false;


    Rigidbody2D myBody = null;

	void Awake() {
		lastRabbit = this;
	}

	// Use this for initialization
	void Start () {
		myBody = this.GetComponent<Rigidbody2D> ();
        LevelController.current.setStartPosition(transform.position);
        this.heroParent = this.transform.parent;
		timeLeftToDie = dieAnimationTime;
	}
	
	// Update is called once per frame
	void Update () {
		//if (is_dead)
			//Play_Die();
			//Dead();
	}
	void FixedUpdate () {
		//[-1, 1]
		float value = Input.GetAxis ("Horizontal");
		//float timeLeftToDie = dieAnimationTime;
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

            if (hit.transform != null
            && hit.transform.GetComponent<MovingPlatform>() != null)
            {
               
                SetNewParent(this.transform, hit.transform);
            }
        }
        else
        {
            isGrounded = false;
            SetNewParent(this.transform, this.heroParent);
        }

		if (Input.GetButtonDown("Jump") && isGrounded)
		 {
            this.JumpActive = true;
           
        }
        
		if (this.JumpActive)
        {
            
            if (Input.GetButton("Jump"))
            {
					//Debug.Log ("Bad news");
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

		if (is_dead) {
			
			timeLeftToDie -= Time.deltaTime;
			if (timeLeftToDie <= 0) {
				LevelController.current.onRabitDeath(this);
				is_dead = false;
			}
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

	public void EatMushroom(bool r_make_big)
    {
        //Debug.Log("Bad news");
		this.make_big=r_make_big;
		if (this.make_big && !this.is_big)
        {
			this.transform.localScale += new Vector3(0.5f, 0.5f, 0f);
            this.is_big = true;
        }
        
    }

    public void MakeSmall()
    {
        this.transform.localScale -= new Vector3(0.5f, 0.5f, 0f);
        this.is_big = false;
    }


	public void Play_Die(bool isd){
		Animator animator = GetComponent<Animator> ();
		if (isd) {
			//animator.SetBool ("die", true);
			//animator.SetBool ("idle", false);
			animator.Play ("Die_Rabbit");
			timeLeftToDie = dieAnimationTime;
			//Debug.Log ("Bad news");
		} else {
			timeLeftToDie = 0;
		}
		is_dead = true;
	}
}
	