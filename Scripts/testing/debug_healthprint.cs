using UnityEngine;
using System.Collections;

public class debug_healthprint : MonoBehaviour 
{
	public TextMesh text;

	private GlobalStats gs;

	void Start()
	{
		gs = GetComponent<GlobalStats> ();
	}

	void Update()
	{
		text.text = gs.health.ToString ("0.0");
	}
}