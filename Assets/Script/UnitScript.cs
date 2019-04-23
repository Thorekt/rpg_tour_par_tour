using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitScript : MonoBehaviour
{
	public int hp ;
	public int hpMax ;
	public int force ;
	public int speed ;
	public int acumulateSpeed = 0;
	private int damage;
	public Slider hpBar;
	public GameObject pos;

	public List<GameObject> skills;


	// Use this for initialization
	void Start()
	{
	

	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log(hp);
		if (hp <= 0)
		{
			GameObject.Find("GameManager").GetComponent<GameManager>().allUnits.Remove(this.gameObject);
			Destroy(this.gameObject);
		}
		hpBar.value = hp;
		Debug.Log(acumulateSpeed);
	}

	public void settupBars(Slider oneHpBar)
	{
		hpBar = oneHpBar;
		hpBar.maxValue = hpMax;
		hpBar.value = hp;

	}

	public void upAS()
	{
		acumulateSpeed += speed;
	}

	public void resetAS()
	{
		acumulateSpeed = 0;
	}


	public void useSkill(int skillNo, GameObject target)
	{
		GameObject usedSkill = Instantiate(skills[skillNo], this.gameObject.transform.Find("spellLauncher").transform);
		usedSkill.GetComponent<SkillScript>().useSkillOn(target);

	}

	public void getDamage(int damage)
	{
		this.hp -= damage;
		return;
	}
}
