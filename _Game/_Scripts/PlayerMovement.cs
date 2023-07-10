using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
public class PlayerMovement : MonoBehaviour
{
    public PlayerCustomAnimator animator;
    public PlayerAttackManager attackManager;
    public float attackDistance = 2f; // Distance at which the character can attack
    private NavMeshAgent agent;
    private Transform target;
    private bool isAttacking;
    public bool Alert;
    public float walkSpeed;
    public float runSpeed;
    public Rigidbody rb;
    public bool AI;
    public Camera MainCamera;
    public bool canMove = true;
    bool lockTarget;
    public bool enemyTeam;
    public bool CantAlert = false;
    float currentSpeed;
    public GameObject arrow;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (Alert)
            SetPlayerAlert();
        else
        SetPlayerIdle();
        rb=GetComponent<Rigidbody>();
      
        CharacterHealth.OnDamage.AddListener((go, dir) =>
        {
            if (go == gameObject)
            {
                Vector3 direction = Vector3.zero;
                switch (dir)
                {
                    case Direction.Front:
                        direction = transform.forward;
                        break;
                    case Direction.Back:
                        direction =- transform.forward;
                        break;
                    case Direction.Left:
                        direction = -transform.right;
                        break;
                    case Direction.Righ:
                        direction = transform.right;
                        break;
                    default:
                        break;
                }
                rb.AddForce(direction);
                agent.velocity= direction;
            }

        });
    }
    public void SetPlayerAlert()
    {
        Alert= true;
        
        animator.SetAlert(Alert);
        if(!AI)
        agent.enabled = false;

    }
    public void FreezMotion()
    {
        canMove = false;
        StopMoving();
    }
    public void UnFreezMotion()
    {
        canMove = true;
        
    }
    public void HolsterWeapon()
    {
      
        SetPlayerIdle();
        animator.Holster();

    }
    public void LockTarget(bool lockTarget)
    {
        this.lockTarget = lockTarget;
    }
    public void UnHolsterWeapon()
    {
        
       
        SetPlayerAlert();
        animator.UnHolster();

    }
    public void SetPlayerIdle()
    {
        Alert = false;
        agent.enabled = true;
        agent.speed = walkSpeed; 
        animator.SetAlert(Alert);
    }
    float oldAngle;
    Vector3 targetVelocity;
    private void Update()
    {
        if (attackManager.takingDamage) return;
        if (AI)
        {
           

            if (lockTarget)
            {
                GameObject target =  GetComponent<BehaviorTree>().GetVariable("Target").GetValue() as GameObject;
                if (target)
                    transform.forward = Vector3.ProjectOnPlane (target.transform.position - transform.position,Vector3.up);
                //else
                //    print("NOT TARGET ON ENEMY");
            }
             Vector3 velo = transform.InverseTransformDirection(agent.velocity);
            if (!Alert)
                animator.SetSpeed(new Vector2(velo.x, velo.z) / runSpeed);
            else
                animator.SetSpeed(new Vector2(velo.x, velo.z) / runSpeed);
            return;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!CantAlert) {
                if (Alert)
                {
                    HolsterWeapon();
                }
                else
                {
                    UnHolsterWeapon();
                }
            }
            else
            {
                NotificationManager.instance.Notify("لا يمكنك استخدام سلاحك هنا");
            }
           
        }
        if (Input.GetKey(KeyCode.LeftShift) && attackManager.stamina>0)
        {
            attackManager.DrainStamina(10 * Time.deltaTime);
            currentSpeed = runSpeed;
            if (!Alert)
            {
            agent.speed = currentSpeed;
            }
          


        }
        else
        {
            currentSpeed = walkSpeed;
            if (!Alert)
            {
                agent.speed = currentSpeed;
            }
        }

        if (!canMove) return;
       
        if (Input.GetMouseButtonDown(0) && !attackManager.attacking && !attackManager.canAttack && !Alert)
        {
            RaycastHit hit;
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (!hit.collider.CompareTag("Interactable"))
                {
                   
                        agent.stoppingDistance = 0f;
                        agent.SetDestination(hit.point);
                  
                    
                    Destroy(Instantiate(arrow,hit.point,Quaternion.identity),1);
                }
             
            }
        }
        Vector3 velocity = Vector3.zero;
        float angle = 0;
        if (Alert && !attackManager.canAttack && !attackManager.attacking ) 
        {

         
            float platyerDistance = Vector3.Distance(MainCamera.transform.position, transform.position);
            Vector3 lookat = MainCamera.transform.position + MainCamera.ScreenPointToRay(Input.mousePosition).direction * platyerDistance;
            lookat = new Vector3(lookat.x, transform.position.y, lookat.z);
            Vector3 dir = lookat - transform.position;
            Vector3 oldFrwd = transform.forward;
            transform.forward = dir;
            angle = Vector3.SignedAngle(transform.forward, oldFrwd, transform.up);

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z))
            {
                velocity += transform.forward * currentSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                velocity -= transform.forward * currentSpeed;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
            {
                velocity -= transform.right * currentSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                velocity += transform.right * currentSpeed;
            }
            velocity = velocity.normalized * currentSpeed;
            targetVelocity = Vector3.Lerp(targetVelocity,velocity,3f*Time.deltaTime);
            rb.velocity = targetVelocity;

        }
        else
        {
            targetVelocity = Vector3.Lerp(targetVelocity, agent.velocity, 5f * Time.deltaTime);
        }

   
        angle = Mathf.Lerp(oldAngle, angle, 2 * Time.deltaTime);
        //if (isAttacking && target != null)
        //{
        //    float distance = Vector3.Distance(transform.position, target.position);
        //    if (distance <= attackDistance)
        //    {
        //        // Perform attack logic here
        //        Debug.Log("Attacking enemy!");
        //    }
        //}
       

       
        Vector3 dirction = transform.InverseTransformDirection(targetVelocity);
        if (!Alert)
         animator.SetSpeed(new Vector2(0, targetVelocity.magnitude / runSpeed));
        else
            animator.SetSpeed(new Vector2(dirction.x, dirction.z) / runSpeed);

       if (!Alert || targetVelocity.magnitude > 0.1f)
       {
            angle = 0;
       }
        animator.SetAngularSpeed(angle );
        oldAngle = angle;
    }
    public void StopMoving()
    {
        agent.SetDestination(this.transform.position);
       
    }
}

