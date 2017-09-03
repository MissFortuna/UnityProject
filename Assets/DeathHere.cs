using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHere : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collider)
    {
        HeroRabbit rabit = collider.GetComponent<HeroRabbit>();

        if (rabit != null)
        {
            LevelController.current.onRabitDeath(rabit);
        }
    }
}
