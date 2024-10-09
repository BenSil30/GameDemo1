using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
	public GameObject SelectedObject;
	public LightsAnimationController LightsAnimationController;// Variable to store the currently selected object
	public Camera mainCamera; // Reference to the camera

	public Material SelectedObjectMaterial;
	public Material IndicatorMaterial;

	public bool IsCopyPaste;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			SelectObject();
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			ClearSelection();
		}
	}

	private void ClearSelection()
	{
		if (SelectedObject == null) return;

		LightsAnimationController.RemoveListeners();
		//set material of selectedobject to selectedobjectmaterial
		SelectedObject.GetComponent<Renderer>().material = SelectedObjectMaterial;
		SelectedObject = null;
		IsCopyPaste = false;
	}

	private void SelectObject()
	{
		// Create a ray from the center of the screen
		Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
		RaycastHit hit;

		// Check if the ray hits any object
		if (!Physics.Raycast(ray, out hit)) return;
		// Check if the hit object is valid (e.g., has a certain tag)
		if (hit.collider == null) return;
		// do not allow user to select multiple objects at once

		if (SelectedObject != null)
		{
			Debug.Log("Deselect object first");
			return;
		}

		//LightsAnimationController.RemoveListeners();
		// if not copy pasting
		if (hit.collider.CompareTag("LightInScene"))
		{
			IsCopyPaste = false;
			SelectedObject = hit.collider.gameObject;

			SelectedObjectMaterial = SelectedObject.GetComponent<Renderer>().material;
			SelectedObject.GetComponent<Renderer>().material = IndicatorMaterial;

			UpdateSelectedLightProperties();

			Debug.Log("Selected Object: " + SelectedObject.name);
		}
		// if its a copy paste object
		else if (hit.collider.CompareTag("SelectableLightingPrefab"))
		{
			IsCopyPaste = true;
			SelectedObject = hit.collider.gameObject;

			SelectedObjectMaterial = SelectedObject.GetComponent<Material>();
			SelectedObject.GetComponent<Renderer>().material = IndicatorMaterial;
			Debug.Log("Selected Object: " + SelectedObject.name);
		}
		else
		{
			Debug.Log("Clicked on a non-selectable object: " + hit.collider.gameObject.name);
		}
	}

	private void UpdateSelectedLightProperties()
	{
		LightsAnimationController = SelectedObject.GetComponent<LightsAnimationController>();
		LightsAnimationController.GrabLightPanelComponents();
		LightsAnimationController.UpdateSliderValues();
		LightsAnimationController.AddListeners();
	}
}