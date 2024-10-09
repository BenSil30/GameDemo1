using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public Canvas MusicUploaderCanvas;

	public Canvas LightsCanvas;

	public CameraController CameraController;

	public bool IsPaused;

	public bool MusicUploaderVisible;

	public bool LightsPanelVisible;

	// Start is called before the first frame update
	private void Start()
	{
		IsPaused = false;
		MusicUploaderVisible = false;
		LightsPanelVisible = false;
		Cursor.visible = false;

		MusicUploaderCanvas.enabled = MusicUploaderVisible;
		LightsCanvas.enabled = LightsPanelVisible;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			TogglePause();
		}

		if (Input.GetKeyDown(KeyCode.M))
		{
			ToggleMusicUploader();
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			ToggleLightsPanel();
		}
	}

	private void ToggleLightsPanel()
	{
		LightsPanelVisible = !LightsPanelVisible;
		if (LightsPanelVisible)
		{
			LightsCanvas.enabled = true;
			Cursor.visible = true;
			CameraController.ToggleFreezeCamera();
		}
		else
		{
			LightsCanvas.enabled = false;
			Cursor.visible = false;
			CameraController.ToggleFreezeCamera();
		}
	}

	private void ToggleMusicUploader()
	{
		MusicUploaderVisible = !MusicUploaderVisible;
		if (MusicUploaderVisible)
		{
			TogglePause();
			MusicUploaderCanvas.enabled = true;
			CameraController.ToggleFreezeCamera();
		}
		else
		{
			TogglePause();
			MusicUploaderCanvas.enabled = false;
			CameraController.ToggleFreezeCamera();
		}
	}

	private void TogglePause()
	{
		IsPaused = !IsPaused;
		if (IsPaused)
		{
			Time.timeScale = 0f;
			Cursor.visible = true;
		}
		else
		{
			Time.timeScale = 1f;
			Cursor.visible = false;
		}
	}
}