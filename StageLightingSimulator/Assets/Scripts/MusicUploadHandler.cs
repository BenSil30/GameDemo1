using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SFB;  // Standalone File Browser namespace

public class MusicUploadHandler : MonoBehaviour
{
	public Button uploadButton;         // Reference to the Upload Music button

	//public Button stopButton;           // Button to stop music
	public Text musicInfoText;          // Reference to the Music Info Text UI element

	public AudioSource audioSource;     // Reference to the AudioSource

	public AudioClip audioClip;

	private Coroutine musicLoadingCoroutine; // Coroutine reference to manage the loading

	private void Start()
	{
		// Attach the UploadMusic method to the button click
		uploadButton.onClick.AddListener(UploadMusic);
		//stopButton.onClick.AddListener(StopMusic);
	}

	private void UploadMusic()
	{
		// Open file browser and let the user select an audio file
		string[] paths = StandaloneFileBrowser.OpenFilePanel("Select Music", "", "mp3", false);

		if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
		{
			// If a coroutine is already running, stop it first
			if (musicLoadingCoroutine != null)
			{
				StopCoroutine(musicLoadingCoroutine);
			}

			// Start a new coroutine to load and play the music
			musicLoadingCoroutine = StartCoroutine(LoadMusic(paths[0]));
		}
		else
		{
			musicInfoText.text = "No Music File Selected!";
		}
	}

	private IEnumerator LoadMusic(string path)
	{
		// Load the music file asynchronously
		using (WWW www = new WWW("file:///" + path))
		{
			yield return www;

			if (www.error == null)
			{
				AudioClip clip = www.GetAudioClip(false, true);
				if (clip != null)
				{
					audioSource.clip = clip;
					audioClip = clip;
					audioSource.Play();
					musicInfoText.text = "Selected: " + Path.GetFileName(path);
				}
				else
				{
					musicInfoText.text = "Failed to load audio file!";
				}
			}
			else
			{
				musicInfoText.text = "Error: " + www.error;
			}
		}
	}

	public void StopMusic()
	{
		// Stop the current audio if it's playing
		if (audioSource.isPlaying)
		{
			audioSource.Stop();
			musicInfoText.text = "Music Stopped";
		}

		// Stop the coroutine if it's still running
		if (musicLoadingCoroutine != null)
		{
			StopCoroutine(musicLoadingCoroutine);
			musicLoadingCoroutine = null; // Reset the coroutine reference
		}
	}
}