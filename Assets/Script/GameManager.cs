using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject playerPref;
	private GameObject player;
	private PlayerScript playerScript;

	[SerializeField] private GameObject aiPref;
	private GameObject ai;
	[NonSerialized] public AIScript aiScript;

	public List<GameObject> allUnits = null;

	[SerializeField] private GameObject currentUnit;

	private GameObject currentPlayer;
	private int currentTurn;
	private string currentPhase;

	void Start()
	{

		player = Instantiate<GameObject>(playerPref);
		playerScript = player.GetComponent<PlayerScript>();
		ai = Instantiate<GameObject>(aiPref);
		aiScript = ai.GetComponent<AIScript>();
		currentTurn = 0;
		currentPhase = "start";
		currentPlayer = null;
	}

	// Update is called once per frame
	void Update()
	{
		if (currentTurn == 0)
		{

			setAllUnits();
		}

		Debug.Log("turn : "+currentTurn );
		Debug.Log( currentPhase);

		switch (currentPhase)
		{
			case "start":
				{
					currentUnit = getCurrentUnit();
					Debug.Log(currentUnit.name);
					setCurrentPlayer();
					if (currentPlayer != null)
					{
						changePhase();
					}
					return;
				}

			case "mid":
				{



					return;
				}
			case "end":
				{
					playerScript.unsetButtons();

					currentUnit.GetComponent<UnitScript>().resetAS();

					foreach (GameObject u in allUnits)
					{

						u.GetComponent<UnitScript>().upAS();
					}

					changePhase();
					return;
				}

		}

	}



	public void changePhase()
	{
		switch (currentPhase)
		{
			case "start":
				{

					currentPhase = "mid";
					return;
				}

			case "mid":
				{
					currentPhase = "end";
					return;
				}
			case "end":
				{

					currentPhase = "start";
					changeTurn();
					return;
				}

		}
	}


	public void setAllUnits()
	{
		foreach (GameObject u in this.playerScript.units)
		{
			allUnits.Add(u);
		}
		foreach (GameObject u in this.aiScript.units)
		{
			allUnits.Add(u);
		}
		if (this.playerScript.units != null && this.aiScript.units != null)
		{

			changeTurn();

		}
		
	}

	public GameObject getCurrentUnit()
	{
		int hightestSpeed = -1;
		GameObject unitTurn = null;
		foreach (GameObject u in allUnits)
		{
			if (u != null)
			{
				if (hightestSpeed < u.GetComponent<UnitScript>().acumulateSpeed)
				{
					unitTurn = u;
					hightestSpeed = u.GetComponent<UnitScript>().acumulateSpeed;

				}
			}
		}
		return unitTurn;

	}

	private void setCurrentPlayer()
	{
		if (currentUnit != null)
		{
			if (currentUnit.transform.IsChildOf(player.transform))
			{
				Debug.Log("player");
				playerScript.turn = true;
				playerScript.prepareSkillButtons();
				currentPlayer = player;

			}
			else if (currentUnit.transform.IsChildOf(ai.transform))
			{
				Debug.Log("ai");
				aiScript.turn = true;
				currentPlayer = ai;


			}
			else
			{
				Debug.Log("pas de parent");
			}
		}
	}


	public void changeTurn()
	{
		this.currentTurn++;
	}
}
