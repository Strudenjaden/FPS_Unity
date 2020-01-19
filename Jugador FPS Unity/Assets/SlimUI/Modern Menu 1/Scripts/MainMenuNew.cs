using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuNew : MonoBehaviour {

	Animator CameraObject;

	[Header("Loaded Scene")]
	[Tooltip("The name of the scene in the build settings that will load")]
	public string sceneName = ""; 

	[Header("Panels")]
	[Tooltip("The UI Panel parenting all sub menus")]
	public GameObject mainCanvas;
	[Tooltip("The UI Panel that holds the VIDEO window tab")]
	public GameObject PanelVideo;
	[Tooltip("The UI Panel that holds the GAME window tab")]
	public GameObject PanelGame;

	[Header("SFX")]
	[Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
	public GameObject hoverSound;
	[Tooltip("The GameObject holding the Audio Source component for the AUDIO SLIDER")]
	public GameObject sliderSound;
	[Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
	public GameObject swooshSound;

	// campaign button sub menu
	[Header("Menus")]
	[Tooltip("The Menu for when the MAIN menu buttons")]
	public GameObject mainMenu;
	//[Tooltip("The Menu for when the PLAY button is clicked")]
	//public GameObject playMenu;
	[Tooltip("The Menu for when the EXIT button is clicked")]
	public GameObject exitMenu;

	// highlights
	[Header("Highlight Effects")]
	[Tooltip("Highlight Image for when GAME Tab is selected in Settings")]
	public GameObject lineGame;
	[Tooltip("Highlight Image for when VIDEO Tab is selected in Settings")]
	public GameObject lineVideo;

	[Header("LOADING SCREEN")]
	public GameObject loadingMenu;
	public Slider loadBar;
	public TMP_Text finishedLoadingText;

	void Start(){
		CameraObject = transform.GetComponent<Animator>();
	}

	public void  ReturnMenu (){
		
		exitMenu.gameObject.SetActive(false);
		mainMenu.gameObject.SetActive(true);
	}

	public void NewGame(){
		if(sceneName != ""){
			StartCoroutine(LoadAsynchronously(sceneName));
			//SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		}
	}


	public void  Position2 (){

		CameraObject.SetFloat("Animate",1);
	}

	public void  Position1 (){
		CameraObject.SetFloat("Animate",0);
	}

	public void  GamePanel (){
		PanelVideo.gameObject.SetActive(false);
		PanelGame.gameObject.SetActive(true);

		lineGame.gameObject.SetActive(true);
		lineVideo.gameObject.SetActive(false);
	}

	public void  VideoPanel (){
		PanelVideo.gameObject.SetActive(true);
		PanelGame.gameObject.SetActive(false);

		lineGame.gameObject.SetActive(false);
		lineVideo.gameObject.SetActive(true);
	}

	public void PlayHover (){
		hoverSound.GetComponent<AudioSource>().Play();
	}

	public void PlaySFXHover (){
		sliderSound.GetComponent<AudioSource>().Play();
	}

	public void PlaySwoosh (){
		swooshSound.GetComponent<AudioSource>().Play();
	}

	// Are You Sure - Quit Panel Pop Up
	public void  AreYouSure (){
		exitMenu.gameObject.SetActive(true);
	} 

	public void  Yes (){
		Application.Quit();
	}

	/**
	 * Método creado para que al pulsar el botón NO desaparezca el menú
	 */
	public void No()
	{
		exitMenu.gameObject.SetActive(false);
	}

	IEnumerator LoadAsynchronously (string sceneName){ // scene name is just the name of the current scene being loaded
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
			Debug.Log(sceneName);
			operation.allowSceneActivation = false;
			mainCanvas.SetActive(false);
			loadingMenu.SetActive(true);

			while (!operation.isDone){
				float progress = Mathf.Clamp01(operation.progress / .9f);
				loadBar.value = progress;

				if(operation.progress >= 0.9f){
					finishedLoadingText.gameObject.SetActive(true);

					if(Input.anyKeyDown){
						operation.allowSceneActivation = true;
					}
				}
				
				yield return null;
			}
		}
}