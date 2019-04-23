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
	[SerializeField] private float speed;

	private bool hit = false;

	private Transform startPos;

	private Transform targetTransform;

	private UnitScript targetScript;

	private GameManager gm;

	private Transform dest;

	// Use this for initialization
	void Start()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		startPos = this.transform.parent.parent;
		dest = startPos;
	}

	private void Awake()
	{
		speed *= Time.deltaTime;

	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log(hit);
		switch (this.style)
		{
			case "heal":
				transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed);

				return;
			case "cac":


				if (hit == false)
				{

					GameObject interactPos = targetScript.pos.transform.Find("interactPos").gameObject;
					dest = interactPos.transform;
				}
				else if (hit == true)
				{
					dest = startPos.transform;
				}


				if (this.transform.parent.parent.position != dest.position)
				{
					this.transform.parent.parent.position = Vector3.MoveTowards(this.transform.parent.parent.position, dest.position, speed);
				}
				else if (this.transform.parent.parent.position == dest.position)
				{
					transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed);
				}
				/*else if (this.transform.parent.parent.position != startPos.position && hit)
				{
					this.transform.parent.parent.position = Vector3.MoveTowards(this.transform.parent.parent.position, startPos.position, speed);
				}
				else if (this.transform.parent.parent.position == startPos.position && hit)
				{
					end();
				}*/

				return;
			case ( "cast" ):
				transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed);
				if (hit)
				{
					end();
				}


				return;
		}



	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject == targetTransform.gameObject)
		{
			hit = true;
			skillApply();
		}
	}

	void end()
	{
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
