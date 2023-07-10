using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBook : Interactable
{
    public GameObject player;
    public ParticleSystem prsystem;
    public SceneLoader sceneLoader;
  
    public override void OnInteract()
    {
        prsystem.transform.position=player.transform.position;
        prsystem.Play();
        player.SetActive(false);
        StartCoroutine(wait());
    }
    public IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        sceneLoader.LoadScene();
    }
}
