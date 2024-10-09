using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
	public float intensity,
		red, green, blue,
		posX, posY, posZ,
		rotX, rotY, rotZ,
		strobeSpeed, rotSpeed, strobeTimer;

	public bool isAnimating, strobeOn;

	// Start is called before the first frame update
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
	}
}