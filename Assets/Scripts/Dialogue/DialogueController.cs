using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MFarm.Dialogue
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DialogueController : MonoBehaviour
    {
        public UnityEvent OnFinishEvent; //事件

        public List<DialoguePiece> dialogueList = new List<DialoguePiece>(); //对话列表

        private Stack<DialoguePiece> dialogueStack;

        private GameObject signSprite;
        private bool canTalk;
        // private bool isTalking;
        private bool isFirst;
        

        private void Awake()
        {
            FillDiaiogueStack();
            signSprite = transform.GetChild(1).gameObject;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                canTalk = true;
                signSprite.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                canTalk = false;
                signSprite.SetActive(false);
            }
        }

        private void Update()
        {
            if (canTalk && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(DialoueRotoutine());
            }
        }

        /// <summary>
        /// 将对话列表反向压入栈中,构建对话
        /// </summary>
        private void FillDiaiogueStack()
        {
            dialogueStack = new Stack<DialoguePiece>();
            for (int i = dialogueList.Count - 1; i > -1; i--)
            {
                dialogueList[i].isDone = false;
                dialogueStack.Push(dialogueList[i]); //Push()向 Stack 的顶部添加一个对象
            }
        }

        private IEnumerator DialoueRotoutine()
        {
            // isTalking = true;
            PlayerMovement.Instance.DisableInput = true;
            //TyrPop():尝试移除并返回在 Stack 的顶部的对象. TeyPeek():尝试寻找并返回Stack的顶部对象,不做其他任何操作
            //尝试找到参数,并删除
            if (dialogueStack.TryPop(out DialoguePiece result))
            {
                EventHandler.CallCameraLookEvent(this.gameObject,true);
                //传递UI显示对话
                EventHandler.CallDialogueEvent(result);
                // EventHandler.CallUpdateGameStateEvent(GameState.Pause);

                yield return new WaitUntil(() => result.isDone);
                // isTalking = false;
            }
            else
            {
                EventHandler.CallCameraLookEvent(GameObject.FindWithTag("CameraFlower"),false);
                //如果DialoPiece中没有参数后,传入null,使得关闭对话
                EventHandler.CallDialogueEvent(null);
                // EventHandler.CallUpdateGameStateEvent(GameState.Gameplay);

                //将对话列表重新压入栈中
                FillDiaiogueStack();
                // isTalking = false;
                PlayerMovement.Instance.DisableInput = false;

                if (OnFinishEvent != null)
                {
                    OnFinishEvent.Invoke();
                    canTalk = false;
                }
            }
        }
    }
}