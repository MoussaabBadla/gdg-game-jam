using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Collectable : Interactable
{
    public UnityEvent collected = new UnityEvent();
    public override void OnInteract()
    {
        PlayerRef.instance.GetComponent<PlayerInteraction>().itemsCollected++;
        PlayerRef.instance.GetComponent<PlayerInteraction>().hilighted = null;
        collected?.Invoke();
        Destroy(gameObject);
    }
}
