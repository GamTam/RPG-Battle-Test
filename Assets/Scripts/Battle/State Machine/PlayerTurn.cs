using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battle.State_Machine
{
    public class PlayerTurn : State
    {
        private Player _player;
        
        public PlayerTurn(BattleManager bm, Player player) : base(bm)
        {
            _player = player;
        }

        public override IEnumerator EnterState()
        {
            _battleManager._playerInput.SwitchCurrentActionMap("Null");
            if ((_battleManager._turnIndex == 0 && _battleManager._currentPlayerIndex == 0) || _battleManager._players.Count == 1)
            {
                _battleManager._textBoxText.SetText($"* {_player._name}'s turn!");
                goto movedIcons;
            }

            Player currentPlayer = null;
            Player startingPlayer = null;
            int i = 0;
            foreach (Battleable obj in _battleManager._fighters)
            {
                if(obj.GetType() != typeof(Player)) continue;
                int point = Globals.Mod(i + _battleManager._currentPlayerIndex, _battleManager._players.Count);
                
                if (point == 0) currentPlayer = (Player) obj;
                if (point == 1) startingPlayer = (Player) obj;

                ((Player) obj).StartingLocation = new Vector2(obj.transform.localPosition.x, _battleManager._profilePoints[(_battleManager._players.Count - point) % _battleManager._players.Count].y);
                i--;
            }
            
            foreach (Animator anim in _battleManager._players)
            {
                if (anim.gameObject == startingPlayer.gameObject) anim.Play("Shrink From Wide");
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

            _battleManager._textBoxText.SetText($"* {_player._name}'s turn!");
            yield return new WaitForSeconds(0.5f);
            
            movementDuration = 1;
            timeElapsed = 0;

            while (timeElapsed < movementDuration)
            {
                timeElapsed += Time.deltaTime;
                foreach (Battleable obj in _battleManager._fighters)
                {
                    if(obj.GetType() != typeof(Player)) continue;
                    obj.gameObject.transform.localPosition = Vector3.Lerp(obj.gameObject.transform.localPosition, ((Player) obj).StartingLocation, Time.deltaTime * 5);
                }
                yield return null;
            }
            
            currentPlayer.SetNameText();
            
            foreach (Battleable obj in _battleManager._fighters)
            {
                if(obj.GetType() != typeof(Player)) continue;
                _battleManager.InitFinalSlide((Player) obj);
            }
            
            foreach (Animator anim in _battleManager._players)
            {
                if (anim.gameObject == currentPlayer.gameObject) anim.Play("Wide From Shrink");
                else anim.Play("Tall From Shrink");
            }
            
            timeElapsed = 0;
            movementDuration = 1;
            while (timeElapsed < movementDuration)
            {
                timeElapsed += Time.deltaTime;
                _battleManager._nameTagText.color = Color.Lerp(Color.clear, Color.white, timeElapsed/movementDuration);
                _battleManager._nameTagImage.color = Color.Lerp(Color.clear, Color.white, timeElapsed/movementDuration);
                
                foreach (Image img in _battleManager._buttonImages)
                {
                    img.color = Color.Lerp(Color.clear, Color.white, timeElapsed/movementDuration);
                    img.GetComponentInChildren<TMP_Text>().color = Color.Lerp(Color.clear, Color.white, timeElapsed/movementDuration);
                }

                yield return null;
            }
            
            movedIcons:
            _battleManager._playerInput.SwitchCurrentActionMap("Menu");
            EventSystem.current.SetSelectedGameObject(_battleManager._buttons[0].gameObject);

            foreach (Button button in _battleManager._buttons)
            {
                button.interactable = true;
            }
        }

        public override IEnumerator OnClick()
        {
            foreach (Button button in _battleManager._buttons)
            {
                button.interactable = false;
            }
            _battleManager.PickTurn();
            yield break;
        }
    }
}