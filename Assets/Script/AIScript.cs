using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
	private GameManager gm;

	public List<GameObject> units;
	[SerializeField]
	private List<GameObject> unitsPref;
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
		int j = 0;
		foreach (GameObject unit in unitsPref)
		{
			GameObject pos = gm.transform.Find("Ground/board_2").GetChild(j).gameObject;
			GameObject uC = Instantiate<GameObject>(unit, pos.transform);
			uC.transform.parent = gameObject.transform;
			uC.GetComponent<UnitScript>().pos = pos;
			this.units.Add(uC);
			j++;
		}
	}

	void endTurn()
	{
		gm.changePhase();
	}

}

