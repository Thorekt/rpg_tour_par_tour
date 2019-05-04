using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitScript : MonoBehaviour
{
	[Header("Stats")]
	private float hp;
	[SerializeField] private float hpMax;
	private float mana;
	[SerializeField] private float manaMax;
	private float puissance;
	[SerializeField] private float startPuissance;
	private float regenMana;
	[SerializeField] private float startRegenMana;
	private float speed;
	[SerializeField] private float startSpeed;
	private float acumulateSpeed = 0;
	[SerializeField] private List<GameObject> skills;

	[Header("Utilitaire")]
	[SerializeField] private Image hpBar;
	[SerializeField] private Image manaBar;
	private GameObject pos;
	private List<GameObject> effects = new List<GameObject>();

	public GameObject Pos { get => pos; set => pos = value; }
	public float Hp { get => hp; set => hp = value; }
	public float Mana { get => mana; set => mana = value; }
	public float Puissance { get => puissance; set => puissance = value; }
	public float RegenMana { get => regenMana; set => regenMana = value; }
	public float Speed { get => speed; set => speed = value; }
	public float AcumulateSpeed { get => acumulateSpeed; set => acumulateSpeed = value; }
	public List<GameObject> Skills { get => skills; set => skills = value; }

	// Use this for initialization
	void Start()
	{

		hp = hpMax;
		mana = manaMax;
		puissance = startPuissance;
		regenMana = startRegenMana;
	}

	private void Awake()
	{
		speed = startSpeed;
	}

	// Update is called once per frame

	void Update()
	{

		if (Hp <= 0)
		{
			GameObject.Find("GameManager").GetComponent<GameManager>().AllUnits.Remove(this.gameObject);
			transform.parent.GetComponent<PlayerScript>().Units.Remove(this.gameObject);
			Destroy(this.gameObject);
		}
		if ( Hp > hpMax)
		{
			Hp = hpMax;
		}
		if ( Mana > manaMax)
		{
			Mana = manaMax;
		}
		hpBar.fillAmount = Hp / hpMax;
		manaBar.fillAmount = Mana / manaMax;
	}

	public void startTurn()
	{
		Mana += RegenMana;
		foreach (GameObject e in effects)
		{
			e.GetComponent<EffectScript>().effectApply(gameObject);
		}
	}

	public void addEffect(GameObject effect)
	{
		GameObject e = Instantiate<GameObject>(effect, gameObject.transform.Find("Capsule"));
		effects.Add(e);
	}

	public void removeEffect(GameObject effect)
	{
		effects.Remove(effect);
		Destroy(effect);
	}


	public void upAS()
	{
		AcumulateSpeed += Speed;
	}

	public void resetAS()
	{
		AcumulateSpeed = 0;
	}


	public void useSkill(int skillNo, GameObject target)
	{
		Debug.Log(skillNo);
		GameObject usedSkill = Instantiate(Skills[skillNo], this.gameObject.transform.Find("spellLauncher").transform);
		usedSkill.GetComponent<SkillScript>().useSkillOn(target);
		Mana -= usedSkill.GetComponent<SkillScript>().cost();
		usedSkill.GetComponent<SkillScript>().UnitPuissance = Puissance;
	}


}
