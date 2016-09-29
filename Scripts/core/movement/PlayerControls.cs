using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(RigidbodyFirstPersonController))]
public class PlayerControls : MonoBehaviour 
{
	//Currently using default movement system provided by Unity
	//A Debug.Log line with a meage [IN CAPS] means that feature is fully functional

	public RigidbodyFirstPersonController FPSController; //Temporary movement system

	private float castTimer1, castTimer2, castTimer3, castTimer4, castTimer5, castTimer6;

	public float waitForRecall; //Recall time
	public bool isRecalling; //Is the player recalling?
	public Transform recallPoint; //Where players will recall to (Single Player)
	public string recallPointTag; //RecallPoint has this tag
	public KeyCode[] numKeys; //Keys 1-4

	public int wardCount; //Number of wards placed
	public _OBJECT_STORAGE os; //Spawnable object list

	public GameObject handL, handR; //Player's hands

	private float recallTimer, wardCooldown, setTimer; //Time it takes to recall / cooldown between ward placements / Spell set change timer
	private bool alt, changingS; //Is the plaer holding shift? (For spells 3 && 4) / Are they changing spells?

	private const float lMax = 1f;
	private float cLerp;
	private bool isRotating;

	private int Index;


	void Start()
	{
		InitVariables ();
	}

	void InitVariables()
	{
		recallPointTag = "RECALL_POINT_SP";
		recallPoint = GameObject.FindGameObjectWithTag (recallPointTag).GetComponent<Transform>();
		FPSController = GetComponent<RigidbodyFirstPersonController> ();
		recallTimer = 0; castTimer1 = 0; castTimer2 = 0; castTimer3 = 0; castTimer4 = 0; castTimer5 = 0; castTimer6 = 0;
		isRecalling = false;
		alt = false;
		Debug.Log ("Variables Initialised.");
	}

	void Update()
	{
		CollectInputs ();

		if (isRecalling) 
		{
			FPSController.enabled = false;
			recallTimer += Time.deltaTime;
			if (recallTimer >= waitForRecall) 
			{
				transform.position = recallPoint.transform.position;
				transform.rotation = recallPoint.transform.rotation;
				recallTimer = 0;
				isRecalling = false;
				Debug.Log ("Player has Recalled!");
			}
		} 
		else 
		{
			FPSController.enabled = true;
		}

		cLerp += Time.deltaTime;
		if (cLerp > lMax && !isRotating) 
		{
			cLerp = 0;
		}
	}

	void CollectInputs()
	{
		if(Input.GetKeyDown("g"))
		{
			Recall ();
		}

		if(Input.GetMouseButton(0) && !changingS)
		{
			if (!alt) 
			{
				FIRE_PRIMARY ();
			} 
			else 
			{
				FIRE_ALT_PRIMARY ();
			}
		}

		if (Input.GetMouseButton (1) && !changingS) 
		{
			if (!alt) 
			{
				FIRE_SECONDARY ();
			} 
			else 
			{
				FIRE_ALT_SECONDARY ();
			}
				
		}

		if(Input.GetMouseButton(2) && !changingS)
		{
			FIRE_SPECIAL ();
		}

		if (Input.GetKeyDown (KeyCode.F)) 
		{
			MAP_PING ();
		}

		if (Input.GetKeyDown (KeyCode.C)) 
		{
			MINION_WAIT ();
		}

		if (Input.GetKeyDown (KeyCode.X)) 
		{
			MINION_FOLLOW ();
		}

		if (Input.GetKeyDown (KeyCode.Z)) 
		{
			MINION_GO ();
		}

		if (Input.GetKeyDown (KeyCode.R)) 
		{
			MINION_SUMMON ();
		}

		if(Input.GetKeyDown(KeyCode.E))
		{
			WARD ();
		}

		if (Input.GetKey (KeyCode.Q)) 
		{
			MANA_REGEN ();
		}

		if(Input.GetKey(KeyCode.LeftShift))
		{
			alt = true;
		}
		else if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			alt = false;
		}

		if (Input.GetKey (KeyCode.LeftControl)) 
		{
			FIRE_MOVEMENT_SPELL ();
		}

		for (int i = 0; i < numKeys.Length; i++) 
		{
			if(Input.GetKeyDown(numKeys[i]))
			{
				changingS = true;
				Index = i;
			}
		}

		if (!FPSController.Grounded) 
		{
			Quaternion newRot = new Quaternion();
			Quaternion oldRot = new Quaternion ();

			if (alt) 
			{
				FPSController.enabled = false;
				if (Input.GetKey ("w")) {	
					float perc = cLerp / lMax;
					newRot.eulerAngles = new Vector3 (90, transform.rotation.y, transform.rotation.z);
					transform.rotation = Quaternion.Slerp (transform.rotation, newRot, 0.5f);
				} 
				else if(Input.GetKeyUp("w")) 
				{
					transform.rotation = Quaternion.Slerp (transform.rotation, oldRot, 0.5f);
				}

				if (Input.GetKey ("a")) 
				{
					float perc = cLerp / lMax;
					newRot.eulerAngles = new Vector3 (transform.rotation.x, transform.rotation.y, 90);
					transform.rotation = Quaternion.Slerp (transform.rotation, newRot, 0.5f);
				}
				else if(Input.GetKeyUp("a")) 
				{
					transform.rotation = Quaternion.Slerp (transform.rotation, oldRot, 0.5f);
				}

				if (Input.GetKey ("s")) 
				{
					float perc = cLerp / lMax;
					newRot.eulerAngles = new Vector3 (-90, transform.rotation.y, transform.rotation.z);
					transform.rotation = Quaternion.Slerp (transform.rotation, newRot, 0.5f);
				}
				else if(Input.GetKeyUp("s")) 
				{
					transform.rotation = Quaternion.Slerp (transform.rotation, oldRot, 0.5f);
				}

				if (Input.GetKey ("d")) 
				{
					float perc = cLerp / lMax;
					newRot.eulerAngles = new Vector3 (transform.rotation.x, transform.rotation.y, -90);
					transform.rotation = Quaternion.Slerp (transform.rotation, newRot, 0.5f);
				}
				else if(Input.GetKeyUp("d")) 
				{
					transform.rotation = Quaternion.Slerp (transform.rotation, oldRot, 0.5f);
				}
			} 
			else 
			{
				FPSController.enabled = true;
				oldRot = transform.rotation;
			}
		}
	}

	void Recall()
	{
		isRecalling = true;
		Debug.Log ("BEGINNING RECALL...");
	}

	public void MANA_REGEN()
	{
		Debug.Log ("Regenerating Mana");
	}

	public void FIRE_PRIMARY()
	{
		//Left click
		Debug.Log("Spell 1");
		castTimer1 += Time.deltaTime;

		if(Input.GetMouseButtonUp(1))
		{
			castTimer1 = 0;
		}
	}

	public void FIRE_SECONDARY()
	{
		//Right click
		Debug.Log("Spell 2");
		castTimer2 += Time.deltaTime;

		if(Input.GetMouseButtonUp(1))
		{
			castTimer2 = 0;
		}
	}

	public void FIRE_SPECIAL()
	{
		//Middle click
		Debug.Log("Charge");

		castTimer3 += Time.deltaTime;

		if(Input.GetMouseButtonUp(1))
		{
			castTimer3 = 0;
		}
	}

	public void FIRE_ALT_PRIMARY()
	{
		//Shift + Left click
		Debug.Log("Spell 3");

		castTimer3 += Time.deltaTime;

		if(Input.GetMouseButtonUp(1))
		{
			castTimer3 = 0;
		}
	}

	public void FIRE_ALT_SECONDARY()
	{
		//Shift + Right click
		Debug.Log("Spell 4");

		castTimer3 += Time.deltaTime;

		if(Input.GetMouseButtonUp(1))
		{
			castTimer5 = 0;
		}
	}

	public void FIRE_MOVEMENT_SPELL()
	{
		//Control
		Debug.Log("Movement");

		castTimer3 += Time.deltaTime;

		if(Input.GetKeyUp(KeyCode.LeftControl))
		{
			castTimer6 = 0;
		}
	}

	public void MINION_SUMMON()
	{
		Debug.Log ("Minions summoned");
	}

	public void MINION_FOLLOW()
	{
		Debug.Log ("Minions will follow you");
	}

	public void MINION_WAIT()
	{
		Debug.Log ("Minions will stay");
	}

	public void MINION_GO()
	{
		Debug.Log ("Minions will charge in that direction");
	}

	public void MAP_PING()
	{
		Debug.Log ("Ping Pong!");
	}

	public void WARD()
	{
		GameObject w;
		if (wardCount == 2) 
		{
			Debug.LogWarning ("ward limit reached!");
			return;
		} 
		else 
		{
			w = (GameObject)Instantiate (os.OBJECT_LIST[0], transform.position + transform.TransformDirection(Vector3.forward) + Vector3.up/2, Quaternion.identity);
			wardCount++;
			Debug.Log ("ward constructed");
		}
	}

	public void PAUSE()
	{
		Debug.Log ("Paused");
	}
}