using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace Battle.State_Machine
{
    public class EnemyTurn : State
    {
        private Enemy _enemy;
        private Random _rand = new Random();
        
        public EnemyTurn(BattleManager bm, Enemy enemy) : base(bm)
        {
            _enemy = enemy;
        }

        public override IEnumerator EnterState()
        {
            _battleManager._textBoxText.SetText($"* {_enemy._name}'s turn!");
            int target = _rand.Next(_battleManager._players.Count - 1);
            Player player = _battleManager._players[target].gameObject.GetComponent<Player>();
            
            yield return new WaitForSeconds(2);
            
            _battleManager._textBoxText.SetText($"* {_enemy._name} attacks {player._name}!");
            
            yield return new WaitForSeconds(1);
           
            player._HP -= 100;
            yield return new WaitForSeconds(1);
            _battleManager.PickTurn();
        }
    }
}