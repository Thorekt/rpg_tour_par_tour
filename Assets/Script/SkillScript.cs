using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScript : MonoBehaviour
{

	[SerializeField] private string style;

	[SerializeField] private float vSkill;

	[SerializeField] private GameObject effect;



	[SerializeField] private float manaCost;

	private float speed;

	[HideInInspector] private float unitPuissance;

	private Transform targetTransform;

	private UnitScript targetScript;

	private GameManager gm;

	private Transform dest;

	public float UnitPuissance { get => unitPuissance; set => unitPuissance = value; }



	// Use this for initialization
	void Start()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

	}

	private void Awake()
	{
		speed = 10;
		speed *= Time.deltaTime;

	}

	// Update is called once per frame
	void Update()
	{
		transform.LookAt(targetTransform);
		transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed);
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.transform.parent.gameObject == targetTransform.gameObject)
		{
			skillApply();
			gm.changePhase();
			Destroy(this.gameObject);
		}

	}

	public float cost()
	{
		return manaCost;
	}

	void skillApply()
	{
		vSkill *= UnitPuissance;


		if (style == "soin")
		{
			targetScript.Hp += vSkill;
		}
		else if (style == "damage")
		{
			targetScript.Hp -= vSkill;
		}
		else if (style == "mana")
		{
			targetScript.Mana += vSkill;
		}

		if (effect != null)
		{
			targetScript.addEffect(effect);
		}
	}

	public void useSkillOn(GameObject target)
	{
		targetScript = target.GetComponent<UnitScript>();
		targetTransform = target.transform;


	}

}
