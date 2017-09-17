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
        }
        else
        {
            rabit.Dead();
        }
        this.CollectedHide();
    }
}
