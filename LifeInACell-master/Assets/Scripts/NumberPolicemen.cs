using UnityEngine;
using UnityEngine.UI;

public class NumberPolicemen : MonoBehaviour
{
	Text numberPolicemen; 

	private void Awake()
	{
		numberPolicemen = GetComponent<Text>();
	}

    private void Update()
	{
		GameObject[] police = GameObject.FindGameObjectsWithTag ("Policeman");
		int Total = police.Length;
		
		numberPolicemen.text = "Total police: " + Total;
	}	
}
