using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
	


	private GameManager gm;
	private bool turn;
	private GameObject currentUnit;
	private GameObject board;

	private List<GameObject> units;
	[SerializeField]
	private List<GameObject> unitsPref;

	

	
	private List<Button> skillButtons = new List<Button>();
	private GameObject canvas;
	private int btnNo;


	


	private bool selectMode = false;
	[SerializeField] private string selectableTag = "Unit";
	private Material defaultMaterial;
	[SerializeField] private Material highlightMaterial;
	private Transform _selection;

	public List<GameObject> Units { get => units; set => units = value; }
	public bool Turn { get => turn; set => turn = value; }
	public GameObject Board { get => board; set => board = value; }




	// Use this for initialization
	void Start()
	{

		Turn = false;
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
			RaycastHit[] hits = Physics.RaycastAll(ray);

			foreach (RaycastHit hit in hits)
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
		Units = new List<GameObject>();
		int j = 0;
		foreach (GameObject unit in unitsPref)
		{
			GameObject pos = Board.transform.GetChild(j).gameObject;
			GameObject uC = Instantiate<GameObject>(unit, pos.transform);
			uC.transform.parent = gameObject.transform;
			uC.GetComponent<UnitScript>().Pos = pos;
			this.Units.Add(uC);
			j++;
		}
	}




	public void prepareSkillButtons()
	{
		int i = 0;

		GameObject currentUnit = gm.getCurrentUnit();
		canvas.transform.GetChild(0).gameObject.SetActive(true);
		List<GameObject> skills = currentUnit.GetComponent<UnitScript>().Skills;
		foreach (GameObject skill in skills)
		{
			skillButtons[i].gameObject.SetActive(true);
			skillButtons[i].transform.GetChild(0).GetComponent<Text>().text = skill.name;
			int btnNo = i;
			skillButtons[i].onClick.AddListener(() => skillButtonsClicked(btnNo, currentUnit));

			Debug.Log("assign " + i);
			i++;
		}
	}


	void skillButtonsClicked(int btnNo, GameObject currentUnit)
	{
		selectMode = true;
		this.btnNo = btnNo;
		this.currentUnit = currentUnit;
		Debug.Log("bouton " + btnNo);
	}

	public void unsetButtons()
	{
		foreach (Button button in skillButtons)
		{

			button.onClick.RemoveAllListeners();
			button.transform.GetChild(0).GetComponent<Text>().text = null;
			button.gameObject.SetActive(false);
		}
		canvas.transform.GetChild(0).gameObject.SetActive(false);
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
