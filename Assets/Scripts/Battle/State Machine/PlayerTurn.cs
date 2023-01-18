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
            _battleManager._textBoxText.SetText($"{_player}'s turn!");
            yield return new WaitForSeconds(1);
            _battleManager.PickTurn();
        }
    }
}