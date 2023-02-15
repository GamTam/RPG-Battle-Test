﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

namespace Battle.State_Machine
{
    public class PlayerTurn : State
    {
        private Player _player;
        private Random _rand = new Random();
        
        public PlayerTurn(BattleManager bm, Player player) : base(bm)
        {
            _player = player;
        }

        public override IEnumerator EnterState()
        {
            _battleManager._playerInput.SwitchCurrentActionMap("Null");
            _battleManager._cancelMovement = true;
            
            _battleManager._nameTagText.color = Color.white;
            _battleManager._nameTagImage.color = Color.white;
                
            foreach (Image img in _battleManager._buttonImages)
            {
                img.color = Color.white;
                img.GetComponentInChildren<TMP_Text>().color = Color.white;
            }
            
            if ((_battleManager._turnIndex == 0 && _battleManager._currentPlayerIndex == 0) || _battleManager._players.Count == 1)
            {
                _battleManager.SetBattleText($"* {_player._name.ToUpper()}'s turn!", true);
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

            _battleManager.SetBattleText($"* {_player._name.ToUpper()}'s turn!", true);
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
            if (_player._HP <= 0)
            {
                _battleManager.SetBattleText($"* {_player._name.ToUpper()} is unable to participate!");
                yield return new WaitForSeconds(2f);
                _battleManager.PickTurn();
                yield break;
            }
            
            _battleManager._playerInput.SwitchCurrentActionMap("Menu");
            _battleManager.SelectButton();

            foreach (Button button in _battleManager._buttons)
            {
                button.interactable = true;
            }
        }

        public override IEnumerator OnClick()
        {
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "Fight":
                case "Check":
                    _battleManager._soundManager.Play("confirm");
                    _battleManager._selectionBoxes[0].gameObject.SetActive(true);
                    _battleManager._selectionBoxes[0].ResetButtons();
                    
                    Dictionary<String, int> names = new Dictionary<string, int>();
                    
                    for (int i = _battleManager._enemies.Count - 1; i >= 0; i--)
                    {
                        string text = _battleManager._enemies[i]._name;
                        
                        if (!names.ContainsKey(text)) names.Add(text, 0);
                        names[text] += 1;
                        if (names[text] > 1)
                        {
                            text += $" {Globals.NumberToChar(names[text], true)}";
                        }

                        _battleManager._enemies[i].gameObject.name = text;
                        
                        if (_battleManager._enemies[i]._HP > 0) _battleManager._selectionBoxes[0].AddButton(text);
                    }
                    
                    _battleManager.DisableButtons();
                    _battleManager._soundManager.Play("confirm");

                    EventSystem.current.SetSelectedGameObject(_battleManager._selectionBoxes[0]._buttons[0]
                        .gameObject);
                    yield break;
                case "Special":
                case "Item":
                    _battleManager._soundManager.Play("confirm");
                    _battleManager.DisableButtons();
                    _battleManager.PickTurn();
                    yield break;
                case "Flee":
                    _battleManager._soundManager.Play("confirm");
                    _battleManager.DisableButtons();

                    #region Cannot Flee
                    if (!_battleManager._canFlee)
                    {
                        _battleManager._soundManager.Play("confirm");
                        _battleManager.SetBattleText("* Cannot flee!", true);
                        
                        while (_battleManager.dialogueVertexAnimator.textAnimating)
                        {
                            yield return null;
                        }

                        yield return new WaitForSeconds(1);

                        _battleManager.SetBattleText($"* {_player._name.ToUpper()}'s turn!", true);
                        _battleManager.EnableButtons();
                        yield break;
                    }
                    #endregion

                    #region Can Flee

                    float partySpeed = 0;
                    float enemySpeed = 0;
                    foreach (Battleable bat in _battleManager._fighters)
                    {
                        if (bat.GetType() == typeof(Player)) partySpeed += bat._speed;
                        else enemySpeed += bat._speed;
                    }

                    partySpeed /= _battleManager._players.Count;
                    enemySpeed /= _battleManager._enemies.Count;

                    int threshold = (int) (((enemySpeed / partySpeed) * 100) / 2);

                    int chance = _rand.Next(101);
                    
                    _battleManager.SetBattleText("* You tried to escape...");
                    
                    while (_battleManager.dialogueVertexAnimator.textAnimating)
                    {
                        yield return null;
                    }
                    
                    yield return new WaitForSeconds(1);

                    if (chance > threshold)
                    {
                        _battleManager.SetBattleText("* Escaped!");
                        
                        while (_battleManager.dialogueVertexAnimator.textAnimating)
                        {
                            yield return null;
                        }
                        
                        yield return new WaitForSeconds(1);
                        Application.Quit();
                    }
                    else
                    {
                        _battleManager.SetBattleText("* ...but failed.");
                        
                        while (_battleManager.dialogueVertexAnimator.textAnimating)
                        {
                            yield return null;
                        }
                        
                        yield return new WaitForSeconds(1);
                    }
                    
                    _battleManager.PickTurn();

                    #endregion
                    
                yield break;
            }
            
            Enemy enemy = GameObject.Find(EventSystem.current.currentSelectedGameObject.name).GetComponent<Enemy>();
            _battleManager._selectionBoxes[0].ResetButtons();
            _battleManager._selectionBoxes[0].gameObject.SetActive(false);
            _battleManager.ClearBattleText();

            switch (_battleManager._buttons[_battleManager._currentButton].gameObject.name)
            {
                case "Fight":
                    _battleManager.SetBattleText($"* {_player._name.ToUpper()} attacked {enemy.gameObject.name.ToUpper()}!", true);
                    
                    while (_battleManager.dialogueVertexAnimator.textAnimating)
                    {
                        yield return null;
                    }

                    int damage = Globals.DamageFormula(_player._pow, enemy._def, out bool crit, _player._luck);
                    
                    enemy._slider.gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.2f);
                    
                    Shake shake = enemy.gameObject.GetComponent<Shake>();
                    enemy._HP = crit ? enemy._HP - damage * 2 : enemy._HP - damage;
                    
                    _battleManager._soundManager.Play("hit");
                    shake.maxShakeDuration = 0.25f;
                    shake.enabled = true;
                    
                    yield return new WaitForSeconds(0.5f);
                    enemy.InitSetRedSlider(enemy._HP);
                    yield return new WaitForSeconds(0.5f);

                    if (crit)
                    {
                        _battleManager.SetBattleText("<anim:shake>* A critical hit!</anim>");

                        while (_battleManager.dialogueVertexAnimator.textAnimating)
                        {
                            yield return null;
                        }
                        yield return new WaitForSeconds(0.5f);
                    }

                    _battleManager.SetBattleText($"* {enemy.gameObject.name.ToUpper()} took <color=red>{damage}</color> damage!");
                    
                    while (_battleManager.dialogueVertexAnimator.textAnimating)
                    {
                        yield return null;
                    }
                        
                    yield return new WaitForSeconds(0.5f);
                    enemy._slider.gameObject.SetActive(false);

                    if (enemy._HP <= 0)
                    {
                        _battleManager.SetBattleText($"* {_player._name.ToUpper()} <color=red>defeated {enemy.gameObject.name.ToUpper()}</color>!");
                        _battleManager._deadEnemies.Add(enemy);
                        enemy._killable = true;
                        _battleManager._soundManager.Play("enemyDie");
                        while (_battleManager.dialogueVertexAnimator.textAnimating)
                        {
                            yield return null;
                        }
                        
                        yield return new WaitForSeconds(0.5f);
                    }
                    
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "Check":
                    
                    for(int i=0; i < enemy._description.Length; i++)
                    {
                        yield return null;
                        _battleManager.SetBattleText(enemy._description[i], true);
                        
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
                    }
                    
                    break;
            }
            
            _battleManager.PickTurn();
        }
    }
}