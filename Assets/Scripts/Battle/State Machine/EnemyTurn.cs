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
            _battleManager.SetBattleText($"* {_enemy.gameObject.name.ToUpper()}'s turn!", true);
            
            while (_battleManager.dialogueVertexAnimator.textAnimating)
            {
                yield return null;
            }
            
            PlayerBattle player;

            do
            {
                var target = _rand.Next(_battleManager._players.Count);
                player = _battleManager._players[target].gameObject.GetComponent<PlayerBattle>();

                if (player._HP > 0) break;
            } while (true);

            yield return new WaitForSeconds(1f);

            _battleManager.SetBattleText($"* {_enemy.gameObject.name.ToUpper()} attacks {player._name.ToUpper()}!");
            
            yield return new WaitForSeconds(1);

            int damage = Globals.DamageFormula(_enemy._pow, player._def, out bool crit);

            _battleManager._soundManager.Play("playerHit");
            Shake shake = player.gameObject.GetComponent<Shake>();
            shake.maxShakeDuration = 0.25f;
            shake.enabled = true;
            player._HP -= damage;
            
            yield return new WaitForSeconds(0.5f);
            player.InitSetRedSlider(player._HP);
            yield return new WaitForSeconds(0.5f);
                    
            _battleManager.SetBattleText($"* {player._name.ToUpper()} took <color=red>{damage}</color> damage!");
                    
            while (_battleManager.dialogueVertexAnimator.textAnimating)
            {
                yield return null;
            }
                        
            yield return new WaitForSeconds(0.5f);
            
            if (player._HP <= 0)
            {
                _battleManager.SetBattleText($"<color=red>* {player._name.ToUpper()} defeated!</color>");
                _battleManager._deadPlayers.Add(player.gameObject.GetComponent<Animator>());
                
                while (_battleManager.dialogueVertexAnimator.textAnimating)
                {
                    yield return null;
                }
                        
                yield return new WaitForSeconds(0.5f);
            }
            _battleManager.PickTurn();
        }
    }
}