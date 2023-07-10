using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FindPositionAroundTarget : Conditional
{


    public SharedGameObject target;
    public SharedInt StepCount =10;
    public SharedFloat raduis = 4;
    public SharedFloat raduisVariation = 0.5f;
    public SharedVector3 targetPos;
    public override void OnAwake()
    {
       
    }
    public override TaskStatus OnUpdate()
    {


        for (int i = 0; i < StepCount.Value; i++)
        {
            Vector3 dir = transform.position - target.Value.transform.position;
            dir = Quaternion.AngleAxis(Random.Range(-90,90f)+ Random.Range(-90, 90f), Vector3.up) * dir;
            Vector3 direction = dir.normalized * (Random.Range(0,-raduisVariation.Value+0.01f) + raduis.Value);
            Vector3 pos = target.Value.transform.position + direction;
            RaycastHit hit;
           
            if (Physics.Raycast(pos, target.Value.transform.position - pos, out hit) && (hit.collider.gameObject==target.Value|| hit.collider.CompareTag("AI") || hit.collider.CompareTag("Player")))
            {

                targetPos.SetValue(pos);
                return TaskStatus.Success;
            }
        }
     
        return TaskStatus.Failure;

    }

    
}
