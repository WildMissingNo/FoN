using UnityEngine;
using System.Collections;

public class F_Flamethrower : SpellObject
{
	public float fModifier;

	void OnTriggerStay(Collider c)
	{
		TakeDamage ((owner.GetComponent<GlobalStats>().attack * 0.1f) * fModifier, c.gameObject, owner.gameObject, "IgnoreAllies", false);
	}
}