using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{


	private GameManager gm;

	
	public List<GameObject> units;
	[SerializeField]
	private List<GameObject> unitsPref;

	public bool turn;

	[SerializeField]
	private List<Button> skillButtons = new List<Button>();
	[SerializeField]
	GameObject canvas;


	private int btnNo;
	private GameObject currentUnit;


	private bool selectMode = false;
	[SerializeField] private string selectableTag = "Unit";
	[SerializeField] private Material defaultMaterial;
	[SerializeField] private Material highlightMaterial;
	private Transform _selection;


	// Use this for initialization
	void Start()
	{

		turn = false;
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		createUnits();

		canvas = gameObject.transform.Find("Ui/Canvas").gameObject;
		setSkillButtons();
	}

	// Update is called once per frame
	void Update()
	{
		if (_selection != null)
		{
			_selection.GetComponent<MeshRenderer>().material = defaultMaterial;
			_selection = null;

		}

		if (selectMode == true)
		{
			

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Transform selection = hit.transform;
				if (selection.CompareTag(selectableTag))
				{
					MeshRenderer selectionRenderer = selection.GetComponent<MeshRenderer>();
					if (selectionRenderer != null)
					{
						defaultMaterial = selectionRenderer.material;
						selectionRenderer.material = highlightMaterial;
						if (Input.GetMouseButtonDown(0))
						{
							currentUnit.GetComponent<UnitScript>().useSkill(btnNo, selection.transform.parent.gameObject);
							unsetButtons();
							selectMode = false;
						}

					}
					_selection = selection;
				}
			}

		}

	}



	void createUnits()
	{
		units = new List<GameObject>();
		int j = 0;
		foreach (GameObject unit in unitsPref)
		{
			GameObject pos = gm.transform.Find("Ground/board_1").GetChild(j).gameObject;
			GameObject uC = Instantiate<GameObject>(unit, pos.transform);
			uC.transform.parent = gameObject.transform;
			uC.GetComponent<UnitScript>().pos = pos;
			this.units.Add(uC);
			j++;
		}
	}




	public void prepareSkillButtons()
	{



		int b = 0;

		GameObject currentUnit = gm.getCurrentUnit();

		foreach (GameObject skill in currentUnit.GetComponent<UnitScript>().skills)
		{
			skillButtons[b].gameObject.SetActive(true);
			skillButtons[b].transform.GetChild(0).GetComponent<Text>().text = skill.name;
			skillButtons[b].onClick.AddListener(() => skillButtonsClicked(b - 1, currentUnit));
			b++;
		}


	}


	void skillButtonsClicked(int btnNo, GameObject currentUnit)
	{
		this.selectMode = true;
		this.btnNo = btnNo;
		this.currentUnit = currentUnit;
	}

	public void unsetButtons()
	{
		foreach (Button button in skillButtons)
		{

			button.onClick.RemoveAllListeners();
			button.gameObject.SetActive(false);
		}
	}

	void setSkillButtons()
	{

		for (int i = 0 ; i < 4 ; i++)
		{
			skillButtons.Add(canvas.transform.GetChild(0).GetChild(i).GetComponent<Button>());
		}
		unsetButtons();
	}
}
