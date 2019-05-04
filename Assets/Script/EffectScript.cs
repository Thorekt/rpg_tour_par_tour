using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
	[SerializeField] private int toursRestant;
	[SerializeField] private string type;
	[SerializeField] private float valeur;

	public void effectApply(GameObject unit)
	{
		UnitScript unitScript = unit.GetComponent<UnitScript>();

		toursRestant -= 1;

		if (type == "soin")
		{
			unitScript.Hp += valeur;
		}
		else if (type == "damage")
		{
			unitScript.Hp -= valeur;
		}
		else if (type == "mana")
		{
			unitScript.Mana += valeur;
		}

		if (toursRestant <= 0)
		{
			unitScript.removeEffect(this.gameObject);
		}
	}
}
