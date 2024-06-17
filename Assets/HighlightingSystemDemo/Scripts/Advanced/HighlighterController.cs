using UnityEngine;
using System.Collections;
using HighlightingSystem;

public class HighlighterController : MonoBehaviour
{
	public bool seeThrough = false;
	protected bool _seeThrough = true;

	protected Highlighter h;

	// 
	protected void Awake()
	{
		h = GetComponent<Highlighter>();
		if (h == null) { h = gameObject.AddComponent<Highlighter>(); }
	}

	// 
	void OnEnable()
	{
		_seeThrough = seeThrough;
		
		if (seeThrough) { h.SeeThroughOn(); }
		else { h.SeeThroughOff(); }
	}

	// 
	protected virtual void Start() { }

	// 
	protected void Update()
	{
		if (_seeThrough != seeThrough)
		{
			_seeThrough = seeThrough;
			if (_seeThrough) { h.SeeThroughOn(); }
			else { h.SeeThroughOff(); }
		}
	}

	// 
	public void MouseOver()
	{
		// Highlight object for one frame in case MouseOver event has arrived
		h.On(Color.red);
	}

	// 
	public void Fire1()
	{
		// Switch flashing
		h.FlashingSwitch();
	}

	// 
	public void Fire2()
	{
		// Stop flashing
		h.SeeThroughSwitch();
	}
}