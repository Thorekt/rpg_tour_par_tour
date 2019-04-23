using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScript : MonoBehaviour
{

	[SerializeField] private string style;
	[SerializeField] private string type;
	[SerializeField] private int vSkill;
	[SerializeField] private string effect;
	[SerializeField] private int vEffect;
	[SerializeField] private float speed ;

	private Transform targetTransform;

	private UnitScript targetScript;

	private GameManager gm;


	// Use this for initialization
	void Start()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

	}

	private void Awake()
	{
		speed *= Time.deltaTime;
	}

	// Update is called once per frame
	void Update()
	{
		switch (this.style)
		{
			case "heal":
				transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed);
				return;
			case "cac":
				GameObject interactPos = targetScript.pos.transform.Find("interactPos").gameObject;
				this.transform.parent.position = interactPos.transform.position;
				return;
			case ("cast"):
				transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed);

				return;
		}

		
		
	}

	void OnTriggerEnter(Collider c)
	{
		Debug.Log("hit");
		skillApply();

		gm.changePhase();

		Destroy(this.gameObject);
		

	}

	void skillApply()
	{
		switch (this.style)
		{
			case "heal":
				targetScript.hp += vSkill;
				return;
			case "cac":

				targetScript.getDamage(vSkill);
				return;
			case "cast":
				targetScript.getDamage(vSkill);
				return;
		}
	}

	public void useSkillOn(GameObject target)
	{
		targetScript = target.GetComponent<UnitScript>();
		targetTransform = target.transform;
		
	}

}
