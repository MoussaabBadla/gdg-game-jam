using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


using UnityEngine.EventSystems;
namespace ProjectMage.Dialogue {
    public class NPCDialogue : MonoBehaviour
    {

        public Speaker mainSpeaker;
        public Speaker sideSpeaker;
        public DialogueBox dialogueBox;
        private TextMeshProUGUI dialoguetext;
        public List<DialogueSO> dialogueList;
        public UnityEvent OnDialogueFinished;
        public UnityEvent OnDialogueStarted;
        public UnityEvent OnDialogueLoaded;
        public UnityEvent OnDialogueNewLine;
        public DialogueSO currentDialogue;
        public DialogueSO.DialogueLine currentLine;
        [HideInInspector    ]public int index=-1;
        public bool freezDialogue;
        public bool startOnAwake = true;
        public Sprite PlayerImage;
        public Sprite Salah;
        public Sprite Mage;
        public Sprite Sailor;
        public float WaitBeforeStart;
        public AudioSource audioSource;
        private void Start()
        {
            dialoguetext=dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
            if (startOnAwake)
                StartCoroutine(wait());
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.minDistance = 1000;
            audioSource.playOnAwake = false;
        }
        public IEnumerator wait()
        {
            yield return new WaitForSeconds(WaitBeforeStart);
            LoadNextDialogueSet();
        }
        private void Update()
        {
            

          
         
        }
        public void LoadNextDialogueSet()
        {
            if (freezDialogue) return;
            if (OnDialogueFinished!=null && currentDialogue!=null) OnDialogueFinished?.Invoke();
            //PlayerPrefab.RefrenceManger.instance.player.GetComponent<ControllerParamters>().freezMovement = false;
            if (dialogueList.Count > 0)
            {
               
                currentDialogue = dialogueList[0];
                dialogueList.RemoveAt(0);
                //if (currentDialogue.scripts[index].freezPlayerMovement)
                //    PlayerPrefab.RefrenceManger.instance.player.GetComponent<ControllerParamters>().freezMovement = true;
                index = -1;
                OnDialogueLoaded?.Invoke();
                SkipDialogue();
            }
            else
            {
                
                HideDialogue();
            }
        }
        public void AddDialogueSet(DialogueSO dialogueSO)
        {
            dialogueList.Add(dialogueSO);
        }
        public void Speak(string text,AudioClip audio, Speaker speaker,float wait,float volume)
        {
            dialogueBox.gameObject.SetActive(true);

            Sprite sprite = PlayerImage;
            string speakername = "";
            dialogueBox.Show("text", sprite, speaker == mainSpeaker, speakername);
            switch (speaker)
            {
                case Speaker.Player:
                    sprite = PlayerImage;
                    speakername = "اوس";
                    break;
                case Speaker.Salah:
                    sprite = Salah;
                    speakername = "صلاح الدين الايوبي";
                    break;
                case Speaker.Mage:
                    sprite = Mage;
                    speakername = "الساحر";
                    break;
                case Speaker.Sailor:
                    sprite = Sailor;
                    speakername = "ملاح السفينة";
                    break;
                default:
                    break;
            }
            
            dialogueBox.Show(text, sprite,speaker==mainSpeaker, speakername);
            audioSource.volume=volume;
            audioSource.clip = audio;
            audioSource.Play();
           
            StartCoroutine(WaitDialogue(audio.length + wait));
            

        }
        public void HideDialogue()
        {
            dialogueBox.gameObject.SetActive(false);
        }
        IEnumerator WaitDialogue(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            SkipDialogue();
        }
        public void SkipDialogue()
        {
            if (freezDialogue) 
                return;

            if(index<0)
                OnDialogueStarted?.Invoke();

            if (index > 0 && currentDialogue.scripts[index].stopOnLine) 
            {
                freezDialogue=true;
                if (currentDialogue.scripts[index].hideOnStop)
                    HideDialogue(); 
                return; 
            }

            if (index + 1 >= currentDialogue.scripts.Count ) 
            { 
                HideDialogue();
                OnDialogueFinished?.Invoke ();
                return; 
            }

            if (index + 1< currentDialogue.scripts.Count)
            {
               
                index++;
                currentLine = currentDialogue.scripts[index];
               
                
                //if (currentDialogue.scripts[index].freezPlayerMovement)
                //    PlayerPrefab.RefrenceManger.instance.player.GetComponent<ControllerParamters>().freezMovement = true;
                //else
                //    PlayerPrefab.RefrenceManger.instance.player.GetComponent<ControllerParamters>().freezMovement = false;
                Speak(currentDialogue.scripts[index].text, currentDialogue.scripts[index].audioClip, currentDialogue.scripts[index].speaker, currentDialogue.scripts[index].WaitafterAudio, currentDialogue.scripts[index].volume   );
                OnDialogueNewLine?.Invoke();
                return;
             }
    
            LoadNextDialogueSet();
           
        }
        public void ForceSkipDialogue()
        {

            return;
            freezDialogue = false;
            if (index < 0) OnDialogueStarted?.Invoke();
            if (index + 1 >= currentDialogue.scripts.Count ) { HideDialogue(); return; }

            if (index + 1 < currentDialogue.scripts.Count)
            {
                index++;
               
                currentLine = currentDialogue.scripts[index];
                //if (currentDialogue.scripts[index].freezPlayerMovement)
                //    PlayerPrefab.RefrenceManger.instance.player.GetComponent<ControllerParamters>().freezMovement = true;
                //else
                //    PlayerPrefab.RefrenceManger.instance.player.GetComponent<ControllerParamters>().freezMovement = false;
                Speak(currentDialogue.scripts[index].text, currentDialogue.scripts[index].audioClip, currentDialogue.scripts[index].speaker, currentDialogue.scripts[index].WaitafterAudio, currentDialogue.scripts[index].volume);
                OnDialogueNewLine?.Invoke();
                return;
            }
            HideDialogue();
            //LoadNextDialogueSet();

        }
        public void ForceSkipDialogueToTag(string tag)
        {
            return;
            freezDialogue = false;
            if (index < 0) OnDialogueStarted?.Invoke();
            if (index + 1 >= currentDialogue.scripts.Count) { HideDialogue(); return; }

            for (int i = 0; i< currentDialogue.scripts.Count;i++ )
            {
                if ( currentDialogue.scripts[i].tag==tag)
                {
                    index=i;

                    currentLine = currentDialogue.scripts[index];
                    //if (currentDialogue.scripts[index].freezPlayerMovement)
                    //    PlayerPrefab.RefrenceManger.instance.player.GetComponent<ControllerParamters>().freezMovement = true;
                    //else
                    //    PlayerPrefab.RefrenceManger.instance.player.GetComponent<ControllerParamters>().freezMovement = false;
                    Speak(currentDialogue.scripts[index].text, currentDialogue.scripts[index].audioClip, currentDialogue.scripts[index].speaker, currentDialogue.scripts[index].WaitafterAudio, currentDialogue.scripts[index].volume);
                    OnDialogueNewLine?.Invoke();
                    return;
                }
            }
           
            
            //LoadNextDialogueSet();

        }
    }
}

