using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueSys : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] TMP_Text textLabel;
    [SerializeField] private DialogueObj testDialogue;

    private Typewritereffect typewritereffect;

    private void Start()
    {
        typewritereffect = GetComponent<Typewritereffect>();
        ShowDialogue(testDialogue);
    }

    public void ShowDialogue(DialogueObj dialogueObj)
    {
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObj));
    }

    private IEnumerator StepThroughDialogue(DialogueObj dialogueObj)
    {
        foreach (string dialogue in dialogueObj.Dialogue)
        {
            yield return typewritereffect.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.J));
        }

        CloseDialogueBox();
    }
    private void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
