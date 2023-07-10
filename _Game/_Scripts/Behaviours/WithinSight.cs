using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class WithinSight : Conditional
{
    public float fieldOfViewAngle;
    public float maxDistance = 6f;
    public string targetTag;
    public SharedGameObject target;
    public SharedFloat distanceToTarget;
    private List<GameObject> possibleTargets = new List<GameObject>();
    public override void OnAwake()
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement)
        {
            //if(playerMovement.enemyTeam)
            //{
            //    targetTag = "Muslim";

                
            //}
            //else
            //{
            //    targetTag = "kafir";
            //}
            possibleTargets.AddRange(GameObject.FindGameObjectsWithTag("Player"));
            possibleTargets.AddRange(GameObject.FindGameObjectsWithTag("AI"));


        }
        
    }
    public override TaskStatus OnUpdate()
    {
        float min = Mathf.Infinity;
        bool targetFound = false;
        for (int i = 0; i < possibleTargets.Count; ++i)
        {
            if (possibleTargets[i].GetComponent<PlayerMovement>() == null) continue;
            if (possibleTargets[i].GetComponent<CharacterHealth>() != null && possibleTargets[i].GetComponent<CharacterHealth>().dead) continue;
            if (possibleTargets[i].GetComponent<PlayerMovement>().enemyTeam == GetComponent<PlayerMovement>().enemyTeam) continue;
            if (Vector3.Distance(possibleTargets[i].transform.position, transform.position) > maxDistance) continue;

            if (CheckWithinSight(possibleTargets[i].transform, fieldOfViewAngle) && Vector3.Distance(possibleTargets[i].transform.position,transform.position)<min )
            {
                target.Value = possibleTargets[i].gameObject;
                distanceToTarget.Value = Vector3.Distance(possibleTargets[i].transform.position, transform.position);
                min = Vector3.Distance(possibleTargets[i].transform.position, transform.position);
                targetFound = true;
            }

        }
        if (targetFound)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

   public bool CheckWithinSight(Transform targetTransform, float fieldOfViewAngle)
   {
      Vector3 direction = targetTransform.position - transform.position;
        if (Vector3.Distance(targetTransform.position, transform.position)<2.2f)
        {
            return true;
        }
      return Vector3.Angle(direction, transform.forward) < fieldOfViewAngle;
   }
}
