using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.State_Machine
{
    public class Opening : State
    {
        public Opening(BattleManager bm) : base(bm)
        {
        }

        public override IEnumerator EnterState()
        {
            Color nameColor = _battleManager._nameTagText.color;
            Color[] buttonColor = new Color[_battleManager._buttonImages.Count];
            Player firstPlayer = null;
            
            _battleManager._nameTagText.color = Color.clear;
            _battleManager._nameTagImage.color = Color.clear;
            foreach (Image img in _battleManager._buttonImages)
            {
                img.color = Color.clear;
                img.GetComponentInChildren<TMP_Text>().color = Color.clear;
            }
            
            yield return new WaitForSeconds(2);

            float movementDuration = 1;
            float timeElapsed = 0;

            while (timeElapsed < movementDuration)
            {
                timeElapsed += Time.deltaTime;
                foreach (Battleable obj in _battleManager._fighters)
                {
                    if(obj.GetType() != typeof(Player)) continue;
                    if (firstPlayer == null) firstPlayer = (Player) obj;
                    obj.gameObject.transform.localPosition = Vector3.Lerp(obj.gameObject.transform.localPosition, ((Player) obj).StartingLocation, Time.deltaTime * 5);
                }
                yield return null;
            }
            
            firstPlayer.SetNameText();
            
            foreach (Battleable obj in _battleManager._fighters)
            {
                if(obj.GetType() != typeof(Player)) continue;
                _battleManager.InitFinalSlide((Player) obj);
            }

            foreach (Animator anim in _battleManager._players)
            {
                if (anim.gameObject == firstPlayer.gameObject) anim.Play("Wide From Shrink");
                else anim.Play("Tall From Shrink");
            }

            timeElapsed = 0;
            movementDuration = 1;
            while (timeElapsed < movementDuration)
            {
                timeElapsed += Time.deltaTime;
                _battleManager._nameTagText.color = Color.Lerp(Color.clear, nameColor, timeElapsed/movementDuration);
                _battleManager._nameTagImage.color = Color.Lerp(Color.clear, Color.white, timeElapsed/movementDuration);
                
                foreach (Image img in _battleManager._buttonImages)
                {
                    img.color = Color.Lerp(Color.clear, Color.white, timeElapsed/movementDuration);
                    img.GetComponentInChildren<TMP_Text>().color = Color.Lerp(Color.clear, Color.white, timeElapsed/movementDuration);
                }

                yield return null;
            }

            _battleManager.PickTurn();
        }
    }
}