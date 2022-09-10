using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    defDamage,
    fireDamage,
    iceDamage,
    nukeDamage,
    explDamage,
    poisonDamage
}

public class TowerAttack : MonoBehaviour
{
    AudioSource Shoot;
    TurretRotation turret;
    public float KD = 0.5f;
    public float damage;
    public DamageType damageType;
    public bool isFlying;
    protected float timeLeft;
    public int upgradeLVL = 0;

    protected List<Runner> runnersDetected;
    
    public void AddRunner(Runner runner)
    {
        if (!runnersDetected.Contains(runner))
            runnersDetected.Add(runner);
    }

    public void RemoveRunner(Runner runner)
    {
        if (runnersDetected.Contains(runner))
        {
            if(turret.target == runner)
                turret.target = null;
            runnersDetected.Remove(runner);
        }
    }
    private void Start()
    {
        runnersDetected = new List<Runner>();
        timeLeft = 0f;
        turret = GetComponentInChildren<TurretRotation>();
        Shoot = GetComponent<AudioSource>();
        Shoot.volume = SettingsController.SoundsVolume;
    }

    private void FixedUpdate()
    {
        if (!isFlying)
        {
            Attack();
        }
    }


    public virtual void Attack()
    {
        if (runnersDetected.Count > 0)
        {
            if (runnersDetected[0] != null)
                turret.target = runnersDetected[0].transform;
            if (timeLeft <= 0)
            {
                runnersDetected[0].Hit(damage, damageType);
                Shoot.Play();

                if (runnersDetected[0].Health <= 0)
                {
                    runnersDetected.RemoveAt(0);
                    turret.target = null;
                }
                timeLeft = KD;
            }
            else
            {
                timeLeft -= Time.fixedDeltaTime;
            }
        }
    }

    internal void LVLUP()
    {
        upgradeLVL++;
        damage += damage * 0.2f;
    }
}
