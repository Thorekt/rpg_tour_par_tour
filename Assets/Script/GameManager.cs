using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	[SerializeField] private GameObject playerPref;
	private GameObject player1;
	private GameObject player2;
	private List<GameObject> allUnits = null;

	private GameObject currentUnit;
	private GameObject currentPlayer;
	private PlayerScript currentPlayerScript;
	private int currentTurn;
	private string currentPhase;

	

	private GameObject canvas;
	private bool end;
	private string looser;

	[SerializeField] private Material defaultMaterial;
	[SerializeField] private Material highlightMaterial;

	public List<GameObject> AllUnits { get => allUnits; set => allUnits = value; }

	void Start()
	{
		end = false;
		setupCanvas();
		player1 = Instantiate<GameObject>(playerPref);
		player1.GetComponent<PlayerScript>().Board = transform.Find("Ground/board_1").gameObject;
		player2 = Instantiate<GameObject>(playerPref);
		player2.GetComponent<PlayerScript>().Board = transform.Find("Ground/board_2").gameObject;

		AllUnits = new List<GameObject>();

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

		checkUnits();

		if(end == true)
		{
			endGame();	
		}
		else if(end == false)
		{
			execTurn();
		} 
	}

	private void setupCanvas()
	{
		canvas = transform.Find("Canvas").gameObject;
		foreach(Transform child in canvas.transform)
		{
			child.gameObject.SetActive(false);
		}

	}

	private void endGame()
	{
		GameObject panel = canvas.transform.Find("LoosePanel").gameObject;
		panel.SetActive(true);
		panel.transform.Find("Text").gameObject.GetComponent<Text>().text = looser + " a perdu!";
		panel.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(() => Application.Quit());

	}


	private void execTurn()
	{
		if (currentPhase == "start")
		{
			upAllAS();
			if (currentUnit == null)
			{
				currentUnit = getCurrentUnit();
				currentUnit.GetComponent<UnitScript>().Pos.GetComponent<MeshRenderer>().material = highlightMaterial;
				currentUnit.GetComponent<UnitScript>().startTurn();
			}
			setCurrentPlayer();
			if (currentPlayer != null)
			{
				changePhase();
			}

		}
		else if (currentPhase == "mid")
		{

		}
		else if (currentPhase == "end")
		{
			currentPlayer.GetComponent<PlayerScript>().unsetButtons();
			currentUnit.GetComponent<UnitScript>().Pos.GetComponent<MeshRenderer>().material = defaultMaterial;
			currentUnit.GetComponent<UnitScript>().resetAS();
			currentUnit = null;
			currentPlayer = null;
			changePhase();
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

	private void changeTurn()
	{
		this.currentTurn++;
	}

	private void setAllUnits()
	{
		for (int i = 0 ; i < 5 ; i++)
		{
			if (player1.GetComponent<PlayerScript>().Units[i] != null)
			{
				AllUnits.Add(player1.GetComponent<PlayerScript>().Units[i]);
			}
			if (player2.GetComponent<PlayerScript>().Units[i] != null)
			{
				AllUnits.Add(player2.GetComponent<PlayerScript>().Units[i]);
			}
		}
		if (player1.GetComponent<PlayerScript>().Units != null && player2.GetComponent<PlayerScript>().Units != null)
		{
			int totalUnits = player1.GetComponent<PlayerScript>().Units.Count + player2.GetComponent<PlayerScript>().Units.Count;
			if (totalUnits == AllUnits.Count)
			{
				changeTurn();
			}
			else
			{
				Debug.Log(totalUnits + " : " + AllUnits.Count);
			}
		}

	}

	private void checkUnits()
	{
		if (player1.GetComponent<PlayerScript>().Units.Count == 0)
		{
			looser = "Player 1";
			end = true;
		}
		else if (player2.GetComponent<PlayerScript>().Units.Count == 0)
		{
			looser = "Player 2";
			end = true;
		}
	}

	public GameObject getCurrentUnit()
	{

		GameObject unitTurn = AllUnits[0];
		foreach (GameObject u in AllUnits)
		{

			if (unitTurn == null)
			{
				unitTurn = u;
			}
			else if (unitTurn.GetComponent<UnitScript>().AcumulateSpeed < u.GetComponent<UnitScript>().AcumulateSpeed)
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
			if (currentUnit.transform.IsChildOf(player1.transform))
			{
				currentPlayer = player1;
			}
			else if (currentUnit.transform.IsChildOf(player2.transform))
			{
				currentPlayer = player2;
			}
			currentPlayer.GetComponent<PlayerScript>().Turn = true;
			currentPlayer.GetComponent<PlayerScript>().prepareSkillButtons();
		}
	}



	private void upAllAS()
	{
		foreach (GameObject u in AllUnits)
		{

			u.GetComponent<UnitScript>().upAS();
		}
	}

}
