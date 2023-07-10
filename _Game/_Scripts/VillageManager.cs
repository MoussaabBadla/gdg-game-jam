using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;       
public class VillageManager : MonoBehaviour
{
    public List<CharacterHealth> enemies= new List<CharacterHealth>();
    public float waitBeforeLoading = 5;
    public SceneLoader sceneLoader;
    public CinemachineVirtualCamera virtualCamera;
    public List<CharacterHealth> allies = new List<CharacterHealth>();
    public Transform moveTo;
    public AudioClip audioClip;
    bool end;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (end) return;
        foreach (var item in enemies)
        {
            if (!item.dead) return;

        }
        end=true;
        //CutScene();
        AudioSource.PlayClipAtPoint(audioClip, virtualCamera.transform.position);
        StartCoroutine(WaitLoad());
    }

    public void CutScene()
    {
        virtualCamera.Follow = allies[0].transform;
        foreach (var allie in allies)
        {
            allie.GetComponent<NavMeshAgent>().SetDestination( moveTo.transform.position);
        }
     
     
        StartCoroutine(WaitLoad());
    }
    IEnumerator WaitLoad()
    {
        yield return new WaitForSeconds(waitBeforeLoading);
        sceneLoader.LoadScene();
    }
}
