using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : MonoBehaviour {

	public Vector3 MoveBy;

	protected Vector3 pointA;
	protected Vector3 pointB;

	protected float speed;

	protected Animator animator = null;
	protected Rigidbody2D body = null;
	protected SpriteRenderer sr = null;


	public enum Mode {
		GoToA,
		GoToB,
		Attack,
		Die
	}

	protected Mode mode = Mode.GoToA;

	// Use this for initialization
	protected void Start () {
		Vector3 actualMove = MoveBy;
		actualMove.y = 0; actualMove.z = 0;

		if (MoveBy.x > 0) {
			this.pointA = this.transform.position;
			this.pointB = this.pointA + actualMove;
		} else {
			this.pointB = this.transform.position;
			this.pointA = this.pointB + actualMove;
		}

		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();

	}

	void FixedUpdate () {
		if (mode == Mode.Die) return;
		checkSetAttackMode ();
		if ((mode == Mode.GoToA || mode == Mode.Attack) && isArrived(pointA)) {
			mode = Mode.GoToB;
		} else if ((mode == Mode.GoToB || mode == Mode.Attack) && isArrived(pointB)) {
			mode = Mode.GoToA;
		}

		move();
		attack ();
	}

	protected virtual void checkSetAttackMode () {
		if (isRabbitReachable ()) mode = Mode.Attack;
	}

	public virtual void move () {
		Vector2 velocity = body.velocity;
		float direction = getDirection();

		if (Mathf.Abs(direction) > 0) {   
			velocity.x = direction * speed;
			body.velocity = velocity;

			if (direction < 0) sr.flipX = false;
			else sr.flipX = true;

			//animator.SetBool("hurt", false);
			//animator.SetBool("attack1", false);
			//animator.SetBool("die", false);
			//animator.SetBool("jump", false);
			//animator.SetBool("idle", false);
			//animator.SetBool("run", false);
			animator.SetBool("walk", true);
			animator.SetBool("attack", false);

		}
	}

	virtual protected void attack () {
	}

	float getDirection() {
		Vector3 my_pos = this.transform.position;
		if (mode == Mode.GoToA) {
			if (my_pos.x > pointA.x) return -1;
			else return 1;
		} else if (mode == Mode.GoToB) {
			if (my_pos.x < pointB.x) return 1;
			else return -1;
		} else if (mode == Mode.Attack) {
			if (my_pos.x < HeroRabbit.lastRabbit.transform.position.x) return 1;
			else return -1;
		}
		return 0;
	}

	virtual protected bool isRabbitReachable() {
		return false;
	}

	protected bool isRabbitDomineering() {
		Vector3 rabbit_pos = HeroRabbit.lastRabbit.transform.position;
		Vector3 my_pos = this.transform.position;
		return rabbit_pos.y > my_pos.y + 0.8 && Mathf.Abs(rabbit_pos.x - my_pos.x) < 1.5f;
	}

	virtual protected bool isRabbitClose() {
		Vector3 rabbit_pos = HeroRabbit.lastRabbit.transform.position;
		Vector3 my_pos = this.transform.position;
		if (!HeroRabbit.lastRabbit.is_big) {
		return Mathf.Abs(rabbit_pos.x - my_pos.x) < 2f && Mathf.Abs(rabbit_pos.y - my_pos.y) < 2f;
		} else {
			return Mathf.Abs(rabbit_pos.x - my_pos.x) < 1.2f && Mathf.Abs(rabbit_pos.y - my_pos.y) < 1.8f;
		}

	}

	protected bool isArrived(Vector3 target) { 
		return Mathf.Abs(this.transform.position.x-target.x) < 0.1f; 
	}

	protected IEnumerator die() {
		if (mode == Mode.Die) {
			animator.SetBool ("die", true);
			animator.SetBool ("attack1", false);
			animator.SetBool ("walk", false);
			animator.SetBool ("run", false);

			yield return new WaitForSeconds(0.8f);

			Destroy(this.gameObject);
		}
	}
}
