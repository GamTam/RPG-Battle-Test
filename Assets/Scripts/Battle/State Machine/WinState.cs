﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

namespace Battle.State_Machine
{
    public class WinState : State
    {
        public WinState(BattleManager bm) : base(bm)
        {}

        public override IEnumerator EnterState()
        {
            _battleManager._musicManager.fadeOut(0.25f);
            yield return new WaitForSeconds(0.5f);
            _battleManager._musicManager.Play("Victory");
            
            foreach (Animator anim in _battleManager._players)
            {
                if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Wide")) anim.Play("Shrink From Wide");
                else anim.Play("Shrink From Tall");
            }
            
            float timeElapsed = 0;
            float movementDuration = 1;
            while (timeElapsed < movementDuration)
            {
                timeElapsed += Time.deltaTime;
                _battleManager._nameTagText.color = Color.Lerp(Color.white, Color.clear, timeElapsed/movementDuration);
                _battleManager._nameTagImage.color = Color.Lerp(Color.white, Color.clear, timeElapsed/movementDuration);
                
                foreach (Image img in _battleManager._buttonImages)
                {
                    img.color = Color.Lerp(Color.white, Color.clear, timeElapsed/movementDuration);
                    img.GetComponentInChildren<TMP_Text>().color = Color.Lerp(Color.white, Color.clear, timeElapsed/movementDuration);
                }

                yield return null;
            }

            _battleManager.SetBattleText("* You defeated the enemies!", true);
            
            while (_battleManager.dialogueVertexAnimator.textAnimating)
            {
                if (_battleManager._confirm.triggered)
                {
                    _battleManager.dialogueVertexAnimator.QuickEnd();
                }
                yield return null;
            }

            while (true)
            {
                if (_battleManager._confirm.triggered)
                {
                    break;
                }
                            
                yield return null;
            }
            _battleManager._soundManager.Play("confirm");

            GameObject textBox = _battleManager._textBox.gameObject.transform.parent.gameObject;
            Vector3 textBoxPos = textBox.transform.localPosition;
            textBoxPos.y -= 300f;
            
            _battleManager.InitFinalSlide(textBox.gameObject, textBoxPos, textBox.transform.localScale, 3);
            _battleManager.InitFinalSlide(_battleManager.gameObject, _battleManager.transform.localPosition, new Vector3(1, 2, 1), 3);

            yield return new WaitForSeconds(0.25f);
            
            foreach (Battleable obj in _battleManager._fighters)
            {
                _battleManager.InitFinalSlide(obj.gameObject, new Vector3(obj.transform.localPosition.x - 500, obj.transform.localPosition.y, obj.transform.localPosition.z), 3);
            }
            
            Globals.MusicManager.fadeOut(2);
            
            _battleManager._inBattle = false;
            
            movementDuration = 2;
            timeElapsed = 0;
            
            while (timeElapsed < movementDuration)
            {
                timeElapsed += Time.deltaTime;
                _battleManager._mat.SetFloat("_alpha", Mathf.Lerp(1, 0, timeElapsed / movementDuration));
                yield return null;
            }
        }
    }
}