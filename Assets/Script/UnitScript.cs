using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitScript : MonoBehaviour
{
	public int hp = 0;
	public int hpMax = 0;
	public int force = 0;
	public int speed = 0;
	public int acumulateSpeed;
	private int damage;
	public Slider hpBar;

	public List<GameObject> skills;


	// Use this for initialization
	void Start()
	{
		acumulateSpeed= speed;

		hpBar.maxValue = hpMax;
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
		Debug.Log("unit use skill " + skillNo);
		GameObject usedSkill = Instantiate(skills[skillNo], this.gameObject.transform.Find("spellLauncher").transform);


		usedSkill.GetComponent<SkillScript>().useSkillOn(target);

	}

	public void getDamage(int damage)
	{
		this.hp -= damage;
		return;
	}
}
