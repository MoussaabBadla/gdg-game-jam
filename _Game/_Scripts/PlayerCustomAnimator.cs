using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Front,Back,Left,Righ
}
[RequireComponent(typeof(Animator))]
public class PlayerCustomAnimator : MonoBehaviour
{
    public Animator animator;
    bool alert;
    int LastAttackCombo;
    public int numberOfCombos;
    public void SetAttack(AttackType attack, bool combo = false)
    {
        if (!combo)
        {
            LastAttackCombo = Random.Range(0, numberOfCombos);

            animator.Play("AttackCombo" + LastAttackCombo, 2);
        }
        else 
        {
            animator.Play("AttackCombo" + LastAttackCombo + "0", 2);
           // animator.Play()
        }
      
    }
    public void SetIdle(bool alert)
    {
        if (alert)
        {
            animator.Play("IdleAlert", 0);
        }
        else
        {
            animator.Play("Idle", 0);

        }
        this.alert = alert;


    }
    public void Holster() { animator.Play("Holster", 0); }
    public void UnHolster() { animator.Play("UnHolster", 0); }
    public void Damage(Direction hitDirection)
    {
        switch (hitDirection)
        {
            case Direction.Front:
                animator.Play("DamageFront", 0,0);
                break;
            case Direction.Back:
                animator.Play("DamageBack", 0, 0);
                break;
            case Direction.Left:
                animator.Play("DamageLeft", 0, 0);
                break;
            case Direction.Righ:
                animator.Play("DamageRight", 0, 0);
                break;
            default:
                break;
        }
        animator.Play("Idle", 2);

    }
    public void OnDamageTakenEnd()
    {
        if (alert)
        {
            animator.Play("IdleAlert", 0);
        }
        else
        {
            animator.Play("Idle", 0);

        }
    }
    public void SetAlert(bool value)
    {
        animator.SetBool("Alert", value);
    }
    public void SetSpeed(Vector2 speed)
    {
        animator.SetFloat("SpeedX", speed.x);
        animator.SetFloat("SpeedY", speed.y);
    }

    public void SetAngularSpeed(float angSpeed)
    {
        
        animator.SetFloat("Rot", angSpeed);
     
    }

    public void SetBlocking(bool value)
    {
        animator.SetBool("Blocking", value);
    }
}
