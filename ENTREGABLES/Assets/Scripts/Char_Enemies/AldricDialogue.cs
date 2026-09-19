using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class AldricInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private List<string> dialogueLines; // List to hold the lines of dialogue for Aldric
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private bool loadSceneOnEnd = false;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string dialogueId;

    private bool isPlayerInRange;
    private bool didDialogueStart; // Flag to check if the dialogue has started
    private int lineIndex; // Index to keep track of the current line of dialogue
    private float typingTime = 0.05f;
    private bool isTyping; // Flag to check if the text is currently being typed
    private bool hasTriggered;

    private Coroutine typingCoroutine; // Coroutine reference to manage the typing effect

    public void Interact()
    {
        if (GameManager.Instance.hasDialogueTriggered(dialogueId))
        {
            hasTriggered = true;

            if (!loadSceneOnEnd)
            {
                playerMovement.EnableJump();
            }
        }
        else if (!didDialogueStart)
        {
            StartDialogue();
        }
    }
    private void Update()
    {
        if (isPlayerInRange && !hasTriggered)
        {
            Interact();
        }

        if (didDialogueStart && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                isTyping = false;
                dialogueText.text = dialogueLines[lineIndex];
            }
            else
            {
                NextLine();
            }
        }
    }
    private void StartDialogue() // Method to start the dialogue
    {
       
        playerMovement.SetInputLocked(true);
        didDialogueStart = true;
        hasTriggered = true;
        GameManager.Instance.MarkDialogueTriggered(dialogueId);
        dialoguePanel.SetActive(true);
        lineIndex = 0;
        typingCoroutine = StartCoroutine(ShowLine());
    }

    private IEnumerator ShowLine() // Coroutine to show the dialogue line character by character
    {
        isTyping = true;
        dialogueText.text = string.Empty; // Clear the dialogue text before showing the new line

        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch; // Add each character to the dialogue text
            yield return new WaitForSeconds(typingTime); // Wait for a short duration before adding the next character
        }

        isTyping = false; // Finished without skipping
    }

    private void NextLine()
    {
        lineIndex++;
        if (lineIndex < dialogueLines.Count)
        {
            typingCoroutine = StartCoroutine(ShowLine());
        }
        else
        {
            dialoguePanel.SetActive(false);

            if (loadSceneOnEnd)
            {
                GameManager.Instance.LoadSceneWithFade(sceneToLoad);
            }
            else
            {
                playerMovement.SetInputLocked(false);
                didDialogueStart = false;
                playerMovement.EnableJump();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerInRange = true;
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerInRange = false;
        
    }
}
