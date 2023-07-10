using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;
using UnityEngine.UI;
public enum AttackType
{
    Light,Heavy,Block
}
[RequireComponent(typeof(PlayerCustomAnimator))]
public class PlayerAttackManager : MonoBehaviour
{
    public PlayerCustomAnimator animnator;
    public PlayerMovement playerMovement;
    public float lerpSpeed;
    public WeaponManagerScript weapon;
    
    Vector3 targetForward;
   public  bool attacking;
    AttackType lastAttack;
    float attackTime;
    public bool canAttack;
    public bool blocking;
    public bool takingDamage;
    public float stamina=100f;
    public float staminaDrain = 30f;
    public Image StaminaBar;
    [Header("AUDIO")]
    public AudioClip swingAudio;
    public AudioClip takeDamageAudio;
    public AudioClip blockAudio;
    public AudioClip hitAudio;

    public AudioSource audioSource;
    [SerializeField]bool regenerate=true;

    public void PlayHit()
    {
        audioSource.PlayOneShot(hitAudio);
    }
    public void DrainStamina()
    {
        stamina = Mathf.Clamp(stamina - staminaDrain, 0, 100);
        if (StaminaBar)
            StaminaBar.fillAmount = stamina / 100;
        StopAllCoroutines ();
        StartCoroutine( waitStamin());
    }
    public void DrainStamina(float value)
    {
        stamina = Mathf.Clamp(stamina - value, 0, 100);
        if (StaminaBar)
            StaminaBar.fillAmount = stamina / 100;
        StopAllCoroutines();
        StartCoroutine(waitStamin());
    }
    public bool CanAttack()
    {
        return stamina - staminaDrain>0;
    }
    IEnumerator waitStamin()
    {
        regenerate = false;
        yield return new WaitForSeconds(1);
        regenerate = true;

    }
    private void Start()
    {
        if(weapon)
        weapon.parent = gameObject;
        GetComponent<CharacterHealth>().OnDeath.AddListener(Dead);

        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            if (item.gameObject!=gameObject)
            {
                item.isKinematic = true;
            }
        }
    }
    public void OnDamageStart()
    {
       weapon.OnDamageStart();
        audioSource.PlayOneShot(swingAudio);
    }
    public void OnDamageEnd()
    {
        weapon.OnDamageEnd();
    }
    public bool CanCombo;
    public void OnHolster()
    {
        //animnator.SetIdle(playerMovement.Alert);
        weapon.Holster();
        ResetVariables();
    }
    public void OnUnHolster()
    {
        //animnator.SetIdle(playerMovement.Alert);
        weapon.UnHolster();
        ResetVariables();
    }
    public void OnTakingDamage()
    {
   
        takingDamage = true;
        animnator.animator.applyRootMotion = true;
        GetComponent<NavMeshAgent>().enabled = false;
        ResetVariables();
        if(!blocking)
            audioSource.PlayOneShot(takeDamageAudio);
        else
        audioSource.PlayOneShot(blockAudio);
    }
    public void OnTakingDamagerOver()
    {
       
        takingDamage = false;
        ResetVariables();
        animnator.SetIdle(playerMovement.Alert);
        animnator.animator.applyRootMotion = false;
        if (playerMovement.AI || !playerMovement.Alert)
        {
            GetComponent<NavMeshAgent>().enabled = true;
        }
    

    }
    IEnumerator waitReset()
    {
        yield return new WaitForSeconds(1.5f);
        ResetVariables();
        takingDamage = false;

    }
    public void Dead()
    {
        animnator.animator.enabled = false;
        playerMovement.enabled=false;
        playerMovement.StopMoving();
        if (GetComponent<BehaviorTree>())
        {
            GetComponent<BehaviorTree>().enabled = false;
        }
       

        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            if (item.gameObject != gameObject)
            {
                item.isKinematic = false;
            }
        }
    }
    private void Update()
    {
        if(StaminaBar)
        StaminaBar.fillAmount = stamina/100;
        if (takingDamage) return;

        if (regenerate)
        {
            stamina = Mathf.Clamp(stamina + Time.deltaTime * 17.5f, 0, 100);
        }
        if (!playerMovement.Alert) return;
   
       
        if (canAttack)
        {

            if (!attacking)
            {
                switch (lastAttack)
                {
                    case AttackType.Light:
                        AttackLight();
                        break;
                    case AttackType.Heavy:
                        AttackHeavy();
                        break;
                    case AttackType.Block:
                        Block();
                        break;
                    default:
                        break;
                }
                
            }
        }

        if (playerMovement.AI) return;
        //Player Part
        if (Input.GetMouseButtonDown(0))
        {
            Attack(AttackType.Light);
        }
    
        if (!attacking && !canAttack && Input.GetMouseButton(1))
        {
            blocking = true;
            animnator.SetBlocking(true);
        }
        else
        {
            blocking = false;
            animnator.SetBlocking(false);
        }
        //if (blocking)
        //{
            
        //        float platyerDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
        //        Vector3 lookat = Camera.main.transform.position + Camera.main.ScreenPointToRay(Input.mousePosition).direction * platyerDistance;
        //        lookat = new Vector3(lookat.x, transform.position.y, lookat.z);
        //        Vector3 dir = lookat - transform.position;
        //        transform.forward = Vector3.MoveTowards(transform.forward, dir, lerpSpeed * Time.deltaTime);


        //}
    }
    public void StartCanCombo()
    {
        CanCombo = true;
    }
    public void EndCanCombo()
    {
        CanCombo =false;
        ResetVariables();
    }
    public void OnAttackStart()
    {
        attacking = true;
    }
    public void OnAttackEnd()
    {
     
        ResetVariables();
        animnator.SetIdle(playerMovement.Alert);
       
    }
    void ResetVariables()
    {
        attacking = false;
        canAttack = false;
        CanCombo=false;
     
    }
   
    public void Attack(AttackType attack)
    {
        if (takingDamage) return;
        if (attacking && !CanCombo) return;
        if (!CanAttack()) return;
        DrainStamina();
        lastAttack = attack;
        attackTime= Time.time;
        attacking=false;
       
        canAttack = true;
        playerMovement.StopMoving ();
    }
    
    public void ShowHilight(AttackType attack)
    {
        switch (attack)
        {
            case AttackType.Light:

                break;
            case AttackType.Heavy:

                break;
            case AttackType.Block:

                break;
            default:
                break;
        }
    }

    public void AttackLight()
    {
        animnator.SetAttack(AttackType.Light, CanCombo);
    }
    public void AttackHeavy()
    {
        animnator.SetAttack(AttackType.Heavy);
    }
    public void Block()
    {

    }
}
