using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Speaker
{
    Player,Salah,Mage,Sailor
}
namespace ProjectMage.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueScript", menuName = "DialogueSystem/Dialogue Script", order = 3)]
    public class DialogueSO : ScriptableObject
    {
        [System.Serializable]
        public class DialogueLine
        {
            public string text;
            public AudioClip audioClip;
            public bool flip;
            public float WaitafterAudio;
            public float volume=1f;
            public Speaker speaker;
            public bool stopOnLine;
            public bool hideOnStop;
            public bool freezPlayerMovement;
            public string tag;
        }
        public List<DialogueLine> scripts = new List<DialogueLine>();
      
    }
}