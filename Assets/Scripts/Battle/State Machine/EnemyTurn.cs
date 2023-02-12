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
            _battleManager.SetBattleText($"* {_enemy._name}'s turn!", true);
            Player player;

            do
            {
                var target = _rand.Next(_battleManager._players.Count);
                player = _battleManager._players[target].gameObject.GetComponent<Player>();

                if (player._HP > 0) break;
            } while (true);

            yield return new WaitForSeconds(1f);

            _battleManager.SetBattleText($"* {_enemy._name} attacks {player._name}!");
            
            yield return new WaitForSeconds(1);

            int damage = Globals.DamageFormula(_enemy._pow, player._def);

            _battleManager._soundManager.Play("playerHit");
            Shake shake = player.gameObject.GetComponent<Shake>();
            shake.maxShakeDuration = 0.25f;
            shake.enabled = true;
            player._HP -= damage;
            
            yield return new WaitForSeconds(0.5f);
            player.InitSetRedSlider(player._HP);
            yield return new WaitForSeconds(0.5f);
                    
            _battleManager.SetBattleText($"* {player._name} took {damage} damage!");
                    
            yield return new WaitForSeconds(1f);
            
            if (player._HP <= 0)
            {
                _battleManager.SetBattleText($"* {player._name} defeated!");
                _battleManager._deadPlayers.Add(player.gameObject.GetComponent<Animator>());
                yield return new WaitForSeconds(2f);
            }
            _battleManager.PickTurn();
        }
    }
}