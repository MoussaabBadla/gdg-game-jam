using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    public int itemsCollected;
    public Interactable hilighted;
    public bool canInteract = true;
    private void Start()
    {
        PlayerMovement=GetComponent<PlayerMovement>();
    }
    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit;
        Camera cam = Camera.main;
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r,out hit))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                if(hilighted!=null&& hit.collider.GetComponent<Interactable>() != hilighted)
                {
                    hilighted.UnHilight();
                }
                hilighted = hit.collider.GetComponent<Interactable>();
                hilighted.Hilight();
                if (Input.GetMouseButtonDown(0) ) 
                
                {
                    if (!PlayerMovement.Alert && canInteract)
                    {
                        itemsCollected++;
                        hilighted.OnInteract();
                        hilighted = null;
                    }
                    else
                    {
                        if(NotificationManager.instance)
                        NotificationManager.instance.Notify(" يجب عليك أن تضع سلاحك في الحافظة باستخدام الحرف \"H\" أولاً قبل أن تتمكن من التفاع");
                    }
                    
                }


            }
            else
            {
                if (hilighted != null)
                {
                    hilighted.UnHilight();
                    hilighted = null;
                }
              
            }
        }
        else
        {
            if (hilighted != null)
            {
                hilighted.UnHilight();
                hilighted = null;
            }

        }
    }

    public void StopInteract()
    {
        canInteract = false;
    }
    public void StartInteract()
    {
        canInteract=true;
    }
}
