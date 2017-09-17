using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{

    protected abstract void OnRabitHit(HeroRabbit rabit);

    public bool hideAnimation = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        HeroRabbit heroController = collider.GetComponentInParent<HeroRabbit>();
        if (heroController != null)
        {
            this.OnRabitHit(heroController);
        }
    }

    public void CollectedHide()
    {
        Destroy(this.gameObject);
    }
}
