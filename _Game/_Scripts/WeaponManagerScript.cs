using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManagerScript : MonoBehaviour
{
    public float Damage;
    public Transform holsterLoc;
    public Transform UnholsterLoc;
    public GameObject parent;
    List<GameObject> hits = new List<GameObject>(); 
    private void Awake()
    {
        OnDamageEnd();

    }
    public void OnDamageStart()
    {
        hits.Clear();
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = true;
        }
    }
    public void OnDamageEnd()
    {
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        hits.Clear();
    }
    public void Holster()
    {
        transform.parent = holsterLoc;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public void UnHolster()
    {
        transform.parent = UnholsterLoc;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("Player") || other.CompareTag("AI")) && !hits.Contains(other.gameObject) && other.gameObject != parent)
        {
            var collisionPoint = other.ClosestPoint(transform.position);
            other.GetComponent<CharacterHealth>().Damage(Damage,gameObject,null, collisionPoint);
            parent.GetComponent<PlayerAttackManager>().PlayHit();
            hits.Add(other.gameObject);
        }
    }

    

}
