using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
	private GameManager gm;
	[SerializeField]
	private GameObject unit;
	public List<GameObject> units;
	public bool turn;


	// Use this for initialization
	void Start()
	{

		turn = false;
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		createUnits();
	}

	// Update is called once per frame
	void Update()
	{


	}

	void createUnits()
	{
		units = new List<GameObject>();
		for (int j = 0 ; j < 5 ; j++)
		{
			GameObject uC = Instantiate<GameObject>(unit, gm.transform.Find("Ground/board_2").GetChild(j).transform);
			uC.transform.parent = gameObject.transform;
			this.units.Add(uC);
		}
	}

	void endTurn()
	{
		gm.changePhase();
	}

}

