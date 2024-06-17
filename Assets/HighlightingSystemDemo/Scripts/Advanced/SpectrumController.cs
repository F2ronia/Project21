using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class SpectrumController : HighlighterController
{
	public float speed = 200f;
	
	private readonly int period = 1530;
	private float counter = 0f;
	private Color col = Color.white;

	// 
	new void Update()
	{
		base.Update();
		
		int val = (int)counter;
		
		h.ConstantOnImmediate(col);
		
		counter += Time.deltaTime * speed;
		counter %= period;
	}
	
	public void ReturnColor(bool isSelected) {
		if (isSelected)
			col = Color.red;
		else	
			col = Color.white;
	}
}
