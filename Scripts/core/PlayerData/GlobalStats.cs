using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GlobalStats : MonoBehaviour 
{
	//The generic stats for every player/ai
	public float attack, defense, health, maxHealth, fModifier, mana;
	public int level;
	/* Attack: Damage output
	 * Defense: Damage resistance
	 * Health: Current HP
	 * MaxHealth: Maximum HP
	 * FModieir: A special modifier exclusive to flame spells
	 * Mana: The energy used for casting spells
	 */

	public List<SpellObject> spellSet;

	public List<bool> statusEffects;
	/*
	 * [0]: Alight
	 * [1]: Doused
	 * [2]: Slag Coat
	 */

	public string flag;
	//A flag is used to detirmine whether a player/minion is an "Ally" or an "Enemy"
}