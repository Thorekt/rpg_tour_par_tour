using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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



	[SerializeField] private Material defaultMaterial;
	[SerializeField] private Material highlightMaterial;

	void Start()
	{

		player = Instantiate<GameObject>(playerPref);
		playerScript = player.GetComponent<PlayerScript>();
		ai = Instantiate<GameObject>(aiPref);
		aiScript = ai.GetComponent<AIScript>();
		currentTurn = 0;
		currentPhase = "start";
		currentPlayer = null;
		currentUnit = null;
	}

	// Update is called once per frame
	void Update()
	{
		if (currentTurn == 0)
		{

			setAllUnits();
		}

		Debug.Log(currentPhase);
		switch (currentPhase)
		{
			case "start":
				{
					upAllAS();

					if (currentUnit == null)
					{
						currentUnit = getCurrentUnit();
						currentUnit.GetComponent<UnitScript>().pos.GetComponent<MeshRenderer>().material = highlightMaterial;

					}
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
					currentUnit.GetComponent<UnitScript>().pos.GetComponent<MeshRenderer>().material = defaultMaterial;
					currentUnit.GetComponent<UnitScript>().resetAS();
					Debug.Log("reset value : " + currentUnit.GetComponent<UnitScript>().acumulateSpeed);
					currentUnit = null;



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

	public void changeTurn()
	{
		this.currentTurn++;
	}

	public void setAllUnits()
	{
		int j = 0;
		foreach (GameObject u in this.playerScript.units)
		{
			Slider hpBar = gameObject.transform.Find("HUD/Unit_stat/unit_panel_1").GetChild(j).gameObject.GetComponent<Slider>();
			u.GetComponent<UnitScript>().settupBars(hpBar);
			allUnits.Add(u);
			j++;

		}
		j = 0;
		foreach (GameObject u in this.aiScript.units)
		{
			Slider hpBar = gameObject.transform.Find("HUD/Unit_stat/unit_panel_2").GetChild(j).gameObject.GetComponent<Slider>();
			u.GetComponent<UnitScript>().settupBars(hpBar);
			allUnits.Add(u);
			j++;
		}
		if (this.playerScript.units != null && this.aiScript.units != null)
		{
			changeTurn();
		}

	}

	public GameObject getCurrentUnit()
	{

		GameObject unitTurn = allUnits[0];
		foreach (GameObject u in allUnits)
		{

			if (unitTurn == null)
			{

				unitTurn = u;
			}
			else if (unitTurn.GetComponent<UnitScript>().acumulateSpeed < u.GetComponent<UnitScript>().acumulateSpeed)
			{
				unitTurn = null;
				unitTurn = u;
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
				playerScript.turn = true;
				playerScript.prepareSkillButtons();
				currentPlayer = player;
			}
			else if (currentUnit.transform.IsChildOf(ai.transform))
			{
				aiScript.turn = true;
				currentPlayer = ai;
			}
		}
	}



	private void upAllAS()
	{
		foreach (GameObject u in allUnits)
		{

			u.GetComponent<UnitScript>().upAS();
		}
	}
}
