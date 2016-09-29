using UnityEngine;
using System.Collections;

public class SpellObject : MonoBehaviour 
{
	public GameObject owner;

	protected void TakeDamage(float amt, GameObject target, GameObject attacker, string args, bool healing)
	{
		if (target.GetComponent<GlobalStats> () == null)
			return;
		else if (target.GetComponent<GlobalStats> ().flag == "Enemy" && args == "IgnoreEnemy") 
		{
			return;
		}
		else if (target.GetComponent<GlobalStats> ().flag == "Ally" && args == "IgnoreAlly")
			return;
		else 
		{
			GlobalStats t = target.GetComponent<GlobalStats> ();
			GlobalStats a = attacker.GetComponent<GlobalStats> ();

			amt -= t.defense;
			if (amt < 0.1f && !healing) 
			{
				amt = 0.1f;
			}
		}
	}
}