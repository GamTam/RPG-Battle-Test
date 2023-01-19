using System.Collections;
using UnityEngine;

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

            _battleManager._textBoxText.SetText($"* {_player._name}'s turn!");
            yield return new WaitForSeconds(1.5f);
            
            float movementDuration = 1;
            float timeElapsed = 0;

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
            
            foreach (Animator anim in _battleManager._players)
            {
                if (anim.gameObject == currentPlayer.gameObject) anim.Play("Wide From Shrink");
                else anim.Play("Tall From Shrink");
            }
            
            yield return new WaitForSeconds(5);

            _battleManager.PickTurn();
        }
    }
}