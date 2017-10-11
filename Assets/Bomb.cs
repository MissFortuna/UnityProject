using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Collectable
{
    protected override void OnRabitHit(HeroRabbit rabit)
    {
        if (rabit.is_big)
        {
            rabit.is_big = false;
            rabit.MakeSmall();
        }
        else
        {
            //rabit.Dead();
			rabit.is_dead=true;
			rabit.Play_Die(true);
        }
        this.CollectedHide();
    }
}
