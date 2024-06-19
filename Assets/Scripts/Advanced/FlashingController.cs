using UnityEngine;
using System.Collections;

public class FlashingController : HighlighterController
{
	public Color flashingStartColor = Color.clear;
    public Color flashingEndColor = Color.clear;
	public float flashingDelay = 0f;
	public float flashingFrequency = 0f;

	// 
	protected override void Start()
	{
		base.Start();

		StartCoroutine(DelayFlashing());
	}

	// 
	protected IEnumerator DelayFlashing()
	{
		yield return new WaitForSeconds(flashingDelay);
		
		// Start object flashing after delay
		h.FlashingOn(flashingStartColor, flashingEndColor, flashingFrequency);
	}
}
