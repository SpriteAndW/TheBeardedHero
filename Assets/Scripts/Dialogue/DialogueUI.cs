using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialogueBox;
    public Text dialogueText;
    public Image faceLeft, faceRight;
    public Text nameLeft, nameRight;
    public GameObject ContinueBox;

    private void Awake()
    {
        ContinueBox.SetActive(false);
    }

    private void OnEnable()
    {
        EventHandler.ShowDialogueEvent += OnShowDialogueEvent;
    }

    private void OnDisable()
    {
        EventHandler.ShowDialogueEvent -= OnShowDialogueEvent;
    }

    private void OnShowDialogueEvent(DialoguePiece piece)
    {
        StartCoroutine(ShowDialogue(piece));
    }

    private IEnumerator ShowDialogue(DialoguePiece piece)
    {
        if (piece != null)
        {
            piece.isDone = false;

            dialogueBox.SetActive(true);
            dialogueBox.GetComponent<CanvasGroup>().DOFade(1, 1);
            ContinueBox.SetActive(false);

            dialogueText.text = string.Empty; //清空对话栏文字

            if (piece.name != string.Empty)
            {
                if (piece.onLeft)
                {
                    faceLeft.gameObject.SetActive(true);
                    faceRight.gameObject.SetActive(false);
                    
                    faceLeft.sprite = piece.faceImage;
                    nameLeft.text = piece.name;
                }
                else
                {
                    faceLeft.gameObject.SetActive(false);
                    faceRight.gameObject.SetActive(true);

                    faceRight.sprite = piece.faceImage;
                    nameRight.text = piece.name;
                }
            }
            else
            {
                faceLeft.gameObject.SetActive(false);
                faceRight.gameObject.SetActive(false);
                nameLeft.gameObject.SetActive(false);
                nameRight.gameObject.SetActive(false);
            }

            yield return dialogueText.DOText(piece.dialogueText, 1f).WaitForCompletion();

            piece.isDone = true;

            if (piece.hasToPause && piece.isDone)
            {
                ContinueBox.SetActive(true);
            }
        }
        else
        {
            dialogueBox.GetComponent<CanvasGroup>().alpha = 0;
            dialogueBox.SetActive(false);
            yield break; //跳出协程
        }
    }
}