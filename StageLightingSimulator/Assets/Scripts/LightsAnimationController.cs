using Ookii.Dialogs;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LightsAnimationController : MonoBehaviour
{
	// Light component reference
	public Light controlledLight;

	// UI elements for controlling light properties
	public Slider intensitySlider;

	public Slider redSlider, greenSlider, blueSlider;

	public Slider positionXSlider, positionYSlider, positionZSlider;

	public Slider rotationXSlider, rotationYSlider, rotationZSlider;

	public Slider strobeSpeedSlider;

	public Slider rotSpeedSlider;

	public float RotationSpeed = 0;

	// Variables for strobe effect
	private float strobeTimer = 0;

	public float StrobeSpeed = 0;

	private bool strobeOn = false;

	public bool isAnimating = false;

	public Button StartStopButton;

	private Coroutine RotationCoroutine;

	private void Start()
	{
		controlledLight = GetComponentInChildren<Light>();
	}

	public void Update()
	{
		// Handle strobe effect
		HandleStrobe();
	}

	public void AddListeners()
	{
		intensitySlider.onValueChanged.AddListener(UpdateLightIntensity);

		redSlider.onValueChanged.AddListener(UpdateLightColor);
		greenSlider.onValueChanged.AddListener(UpdateLightColor);
		blueSlider.onValueChanged.AddListener(UpdateLightColor);

		positionXSlider.onValueChanged.AddListener(UpdateLightPosition);
		positionYSlider.onValueChanged.AddListener(UpdateLightPosition);
		positionZSlider.onValueChanged.AddListener(UpdateLightPosition);

		rotationXSlider.onValueChanged.AddListener(UpdateLightRotation);
		rotationYSlider.onValueChanged.AddListener(UpdateLightRotation);
		rotationZSlider.onValueChanged.AddListener(UpdateLightRotation);

		rotSpeedSlider.onValueChanged.AddListener(UpdateLightRotationSpeed);
		strobeSpeedSlider.onValueChanged.AddListener(UpdateStrobeSpeed);

		StartStopButton.onClick.AddListener(ToggleAnimation);
	}

	public void RemoveListeners()
	{
		intensitySlider.onValueChanged.RemoveAllListeners();

		redSlider.onValueChanged.RemoveAllListeners();
		greenSlider.onValueChanged.RemoveAllListeners();
		blueSlider.onValueChanged.RemoveAllListeners();

		positionXSlider.onValueChanged.RemoveAllListeners();
		positionYSlider.onValueChanged.RemoveAllListeners();
		positionZSlider.onValueChanged.RemoveAllListeners();

		rotationXSlider.onValueChanged.RemoveAllListeners();
		rotationYSlider.onValueChanged.RemoveAllListeners();
		rotationZSlider.onValueChanged.RemoveAllListeners();

		rotSpeedSlider.onValueChanged.RemoveAllListeners();

		strobeSpeedSlider.onValueChanged.RemoveAllListeners();

		StartStopButton.onClick.RemoveAllListeners();
	}

	public void GrabLightPanelComponents()
	{
		// Set default values for sliders to match initial light properties
		rotSpeedSlider = GameObject.Find("RotSpeedSlider").GetComponent<Slider>();

		intensitySlider = GameObject.Find("LightIntensitySlider").GetComponent<Slider>();

		redSlider = GameObject.Find("RedColorSlider").GetComponent<Slider>();
		greenSlider = GameObject.Find("GreenColorSlider").GetComponent<Slider>();
		blueSlider = GameObject.Find("BlueColorSlider").GetComponent<Slider>();

		positionXSlider = GameObject.Find("XPosSlider").GetComponent<Slider>();
		positionYSlider = GameObject.Find("YPosSlider").GetComponent<Slider>();
		positionZSlider = GameObject.Find("ZPosSlider").GetComponent<Slider>();

		rotationXSlider = GameObject.Find("XRotSlider").GetComponent<Slider>();
		rotationYSlider = GameObject.Find("YRotSlider").GetComponent<Slider>();
		rotationZSlider = GameObject.Find("ZRotSlider").GetComponent<Slider>();

		strobeSpeedSlider = GameObject.Find("FlashRateSlider").GetComponent<Slider>();

		StartStopButton = GameObject.Find("Start/StopButton").GetComponent<Button>();
	}

	public void UpdateSliderValues()
	{
		intensitySlider.value = controlledLight.intensity;

		redSlider.value = controlledLight.color.r;
		greenSlider.value = controlledLight.color.g;
		blueSlider.value = controlledLight.color.b;

		positionXSlider.value = controlledLight.transform.position.x;
		positionYSlider.value = controlledLight.transform.position.y;
		positionZSlider.value = controlledLight.transform.position.z;

		rotationXSlider.value = controlledLight.transform.rotation.eulerAngles.x;
		rotationYSlider.value = controlledLight.transform.rotation.eulerAngles.y;
		rotationZSlider.value = controlledLight.transform.rotation.eulerAngles.z;

		rotSpeedSlider.value = RotationSpeed;

		strobeSpeedSlider.value = StrobeSpeed; ; // Start with no strobe effect
												 //todo update strobe values after every method call
	}

	public void UpdateLightIntensity(float value)
	{
		controlledLight.intensity = value;
	}

	public void UpdateLightRotationSpeed(float value)
	{
		RotationSpeed = value;
	}

	public void UpdateLightRotation(float value)
	{
		Vector3 newRotation = new Vector3(rotationXSlider.value, rotationYSlider.value, rotationZSlider.value);
		controlledLight.transform.rotation = Quaternion.Euler(newRotation);
		//transform.rotation = Quaternion.Euler(newRotation);
	}

	public void UpdateLightPosition(float value)
	{
		Vector3 newPosition = new Vector3(positionXSlider.value, positionYSlider.value, positionZSlider.value);
		controlledLight.transform.position = newPosition;
		transform.position = newPosition;
	}

	public void UpdateLightColor(float value)
	{
		Color lightColor = new Color(redSlider.value, greenSlider.value, blueSlider.value);
		controlledLight.color = lightColor;
	}

	public void UpdateStrobeSpeed(float value)
	{
		StrobeSpeed = value;
	}

	public void HandleStrobe()
	{
		if (StrobeSpeed > 0)
		{
			strobeTimer += Time.deltaTime;
			if (strobeTimer >= 1f / StrobeSpeed)
			{
				strobeOn = !strobeOn;
				controlledLight.enabled = strobeOn;
				strobeTimer = 0;
			}
		}
		else
		{
			// Ensure the light stays on when the strobe is off
			controlledLight.enabled = true;
		}
	}

	public void ToggleAnimation()
	{
		isAnimating = !isAnimating;
		if (isAnimating)
		{
			StartStopButton.GetComponentInChildren<Text>().text = "Stop";
			RotationCoroutine = StartCoroutine(RotateLightContinually());
		}
		else
		{
			StartStopButton.GetComponentInChildren<Text>().text = "Start";
			StopCoroutine(RotationCoroutine);
		}
	}

	public IEnumerator RotateLightContinually()
	{
		while (isAnimating)
		{
			controlledLight.transform.Rotate(Vector3.up, RotationSpeed * 2 * Time.deltaTime);
			yield return null;
		}
	}
}