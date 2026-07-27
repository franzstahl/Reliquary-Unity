using UnityEngine;
using TMPro;
using System.Collections;

public class AldricInteraction : MonoBehaviour
{
    [SerializeField] private string[] dialogueLines; // Array to hold the lines of dialogue
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;

    private bool isPlayerInRange;
    private bool didDialogueStart; // Flag to check if the dialogue has started
    private int lineIndex; // Index to keep track of the current line of dialogue
    private float typingTime = 0.05f;

    private void Update()
    {
        if (isPlayerInRange)
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
        }
    }

    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        lineIndex = 0;
        StartCoroutine(ShowLine());
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty; // Clear the dialogue text before showing the new line

        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch; // Add each character to the dialogue text
            yield return new WaitForSeconds(typingTime); // Wait for a short duration before adding the next character
        }
    }

    private void NextLine()
    {
        lineIndex++; // Move to the next line of dialogue
        if (lineIndex < dialogueLines.Length) 
        {
            StartCoroutine(ShowLine()); // Show the next line of dialogue

        }
        else
        {
            didDialogueStart = false; 
            dialoguePanel.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerInRange = true;
        //Time.timeScale = 0f;
        Debug.Log("Dentro");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerInRange = false;
        //Time.timeScale = 1f;
        Debug.Log("Fuera");
    }
}
