using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCarrot : Collectable {

	float speed = 2;
	float direction = 0;

	void Start() {
		StartCoroutine(destroyLater());
	}

	IEnumerator destroyLater () {
		yield return new WaitForSeconds(3.0f);
		Destroy(this.gameObject);
	}

	public void launch (float direction) {
		this.direction = direction;
		if (direction < 0) GetComponent<SpriteRenderer>().flipX = true;
	}

	private void Update () {
		Vector3 pos = this.transform.position;
		this.transform.position = pos + this.direction * Vector3.right * Time.deltaTime * speed;
	}

	protected override void OnRabitHit (HeroRabbit rabbit){ 
		if (rabbit.life == 0) {
			rabbit.is_dead = true;
			rabbit.Play_Die (true);
		}
		else if(rabbit.life>0){
			rabbit.life=rabbit.life-1;
		}
			Destroy(this.gameObject);
	}
}
