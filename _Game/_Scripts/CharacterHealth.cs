using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using BehaviorDesigner.Runtime;
using UnityEngine.UI;
[RequireComponent(typeof(PlayerCustomAnimator))]
public class CharacterHealth : MonoBehaviour
{
    public float maxHp;
    public float currentHP;
    public UnityEvent OnDeath;
    public PlayerCustomAnimator animtor;
    public PlayerMovement PlayerMovement;
    public PlayerAttackManager attackManager;
    public static UnityEvent<GameObject,Direction> OnDamage = new UnityEvent<GameObject, Direction>();
    public bool dead=false;
    public Image healthbar;
    private void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
        attackManager = GetComponent<PlayerAttackManager>();
    }
    public void Damage(float value, GameObject resposible, GameObject bodyPart , Vector3 hitPos )
    {
        if (dead) return;
        if (attackManager.blocking&& Vector3.Angle(transform.forward,resposible.transform.position-transform.position)<85f) 
        {
            value = value * 0.3f;
        }
        if (PlayerMovement.AI && !PlayerMovement.Alert)
        {
            PlayerMovement.GetComponent<BehaviorTree>().SetVariableValue("Target", resposible);
            PlayerMovement.GetComponent<BehaviorTree>().SetVariableValue("CurrentState", AIState.Alert);
        }
        currentHP = Mathf.Clamp(currentHP-value,0,maxHp);
        Direction direction = Direction.Front;
        Vector3 dir = hitPos - transform.position;
        float leftangle = Vector3.Angle(-transform.right, dir);
        float rightangle = Vector3.Angle(transform.right, dir);
        float frwrdangle = Vector3.Angle(transform.forward, dir);
        float backangle = Vector3.Angle(-transform.forward, dir);
        if (Mathf.Min(leftangle,rightangle,frwrdangle,backangle)==leftangle)
        {
            direction = Direction.Left;
        }
        if (Mathf.Min(leftangle, rightangle, frwrdangle, backangle) == rightangle)
        {
            direction = Direction.Righ;
        }
        if (Mathf.Min(leftangle, rightangle, frwrdangle, backangle) == frwrdangle)
        {
            direction = Direction.Front;
        }
        if (Mathf.Min(leftangle, rightangle, frwrdangle, backangle) == backangle)
        {
            direction = Direction.Back;
        }
        print(direction);
        animtor.SetIdle(PlayerMovement.Alert);
        animtor.Damage(direction);
        OnDamage?.Invoke(gameObject, direction);
        BloodManager.instance.SpawnBlood(hitPos, transform.rotation);
        if (healthbar)
        {
            healthbar.fillAmount = (float)currentHP/maxHp;
        }
        if (currentHP<=0)
        {
            Dead();
        }
    }


    public void Dead()
    {
        dead = true;
           OnDeath?.Invoke();

    }

}
