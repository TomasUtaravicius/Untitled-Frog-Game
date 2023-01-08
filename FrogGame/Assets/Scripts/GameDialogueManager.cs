using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDialogueManager : MonoBehaviour
{
    public string currentDialoguePiece;
    public static GameDialogueManager instance;

    public UIManager uiManager;
    float dialogueExpireTime;
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        DisableDialogue();
    }
    private void Update()
    {
        if(dialogueExpireTime<=Time.time)
        {
            DisableDialogue();
        }
    }
    public void EnableDialogue(float expireTime,string subtitle)
    {
        dialogueExpireTime = Time.time + expireTime;
        currentDialoguePiece = subtitle;
        uiManager.UpdateSubtitleText(currentDialoguePiece);
    }
    public void UpdateDialogue()
    {
        uiManager.UpdateSubtitleText(currentDialoguePiece);
    }
    public void DisableDialogue()
    {
        uiManager.UpdateSubtitleText("");
    }
}
