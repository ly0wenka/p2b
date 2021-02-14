using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using FPLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(EventSystem))]
public class MainScript : MonoBehaviour, MainScriptInterface
{
	#region public instance enum
	public enum MultiplayerMode{
		Lan,
		Online,
		Bluetooth,
	}
	#endregion

	#region public instance properties
    [FormerlySerializedAs("Main_Config")] public GlobalInfo Main_Config;
	#endregion

	#region public event definitions
	public delegate void MeterHandler(float newFloat, CharacterInfo player);
	public static event MeterHandler OnLifePointsChange;

	public delegate void IntHandler(int newInt);
	public static event IntHandler OnRoundBegins;

	public delegate void StringHandler(string newString, CharacterInfo player);
	public static event StringHandler OnNewAlert;
	
	public delegate void HitHandler(HitBox strokeHitBox, MoveInfo move, CharacterInfo player);
    public static event HitHandler OnHit;
    public static event HitHandler OnBlock;
    public static event HitHandler OnParry;

	public delegate void MoveHandler(MoveInfo move, CharacterInfo player);
	public static event MoveHandler OnMove;

    public delegate void ButtonHandler(ButtonPress button, CharacterInfo player);
    public static event ButtonHandler OnButton;

    public delegate void BasicMoveHandler(BasicMoveReference basicMove, CharacterInfo player);
    public static event BasicMoveHandler OnBasicMove;

    public delegate void BodyVisibilityHandler(MoveInfo move, CharacterInfo player, BodyPartVisibilityChange bodyPartVisibilityChange, HitBox hitBox);
    public static event BodyVisibilityHandler OnBodyVisibilityChange;

    public delegate void ParticleEffectsHandler(MoveInfo move, CharacterInfo player, MoveParticleEffect particleEffects);
    public static event ParticleEffectsHandler OnParticleEffects;

    public delegate void SideSwitchHandler(int side, CharacterInfo player);
    public static event SideSwitchHandler OnSideSwitch;

	public delegate void GameBeginHandler(CharacterInfo player1, CharacterInfo player2, StageOptions stage);
	public static event GameBeginHandler OnGameBegin;

	public delegate void GameEndsHandler(CharacterInfo winner, CharacterInfo loser);
	public static event GameEndsHandler OnGameEnds;
	public static event GameEndsHandler OnRoundEnds;

	public delegate void GamePausedHandler(bool isPaused);
	public static event GamePausedHandler OnGamePaused;

	public delegate void ScreenChangedHandler(CombatScreen previousScreen, CombatScreen newScreen);
	public static event ScreenChangedHandler OnScreenChanged;

	public delegate void StoryModeHandler(CharacterInfo character);
	public static event StoryModeHandler OnStoryModeStarted;
	public static event StoryModeHandler OnStoryModeCompleted;

	public delegate void TimerHandler(Fix64 time);
	public static event TimerHandler OnTimer;

	public delegate void TimeOverHandler();
	public static event TimeOverHandler OnTimeOver;

	public delegate void InputHandler(InputReferences[] inputReferences, int player);
	public static event InputHandler OnInput;
	#endregion

	#region network definitions
    //-----------------------------------------------------------------------------------------------------------------
	// Network
	//-----------------------------------------------------------------------------------------------------------------
    public static MultiplayerAPI multiplayerAPI{
		get{
			if (MainScript.multiplayerMode == MultiplayerMode.Bluetooth){
				return MainScript.bluetoothMultiplayerAPI;
			}else if (MainScript.multiplayerMode == MultiplayerMode.Lan){
				return MainScript.lanMultiplayerAPI;
			}else{
				return MainScript.onlineMultiplayerAPI;
			}
		}
	}

	public static MultiplayerMode multiplayerMode{
		get{
			return MainScript._multiplayerMode;
		}
		set{
			MainScript._multiplayerMode = value;

			if (value == MultiplayerMode.Bluetooth){
				MainScript.bluetoothMultiplayerAPI.enabled = true;
				MainScript.lanMultiplayerAPI.enabled = false;
				MainScript.onlineMultiplayerAPI.enabled = false;
			}else if (value == MultiplayerMode.Lan){
				MainScript.bluetoothMultiplayerAPI.enabled = false;
				MainScript.lanMultiplayerAPI.enabled = true;
				MainScript.onlineMultiplayerAPI.enabled = false;
			}else{
				MainScript.bluetoothMultiplayerAPI.enabled = false;
				MainScript.lanMultiplayerAPI.enabled = false;
				MainScript.onlineMultiplayerAPI.enabled = true;
			}
		}
	}

	private static MultiplayerAPI bluetoothMultiplayerAPI;
	private static MultiplayerAPI lanMultiplayerAPI;
	private static MultiplayerAPI onlineMultiplayerAPI;

	private static MultiplayerMode _multiplayerMode = MultiplayerMode.Lan;
    #endregion
    
	#region gui definitions
    //-----------------------------------------------------------------------------------------------------------------
	// GUI
	//-----------------------------------------------------------------------------------------------------------------
	public static Canvas canvas{get; protected set;}
	public static CanvasGroup canvasGroup{get; protected set;}
	public static EventSystem eventSystem{get; protected set;}
	public static GraphicRaycaster graphicRaycaster{get; protected set;}
	public static StandaloneInputModule standaloneInputModule{get; protected set;}
	//-----------------------------------------------------------------------------------------------------------------
	protected static readonly string MusicEnabledKey = "MusicEnabled";
	protected static readonly string MusicVolumeKey = "MusicVolume";
	protected static readonly string SoundsEnabledKey = "SoundsEnabled";
	protected static readonly string SoundsVolumeKey = "SoundsVolume";
	protected static readonly string DifficultyLevelKey = "DifficultyLevel";
	protected static readonly string DebugModeKey = "DebugMode";
	//-----------------------------------------------------------------------------------------------------------------

	public static CameraScript cameraScript{get; set;}
    public static FluxCapacitor fluxCapacitor;
	public static GameMode gameMode = GameMode.None;
	public static GlobalInfo config;
	public static MainScript MainScriptInstance;
	public static bool debug = true;
	public static Text debugger1;
	public static Text debugger2;
    #endregion

    #region addons definitions
    public static bool isAiAddonInstalled {get; set;}
    public static bool isCInputInstalled { get; set; }
    public static bool isControlFreakInstalled { get; set; }
    public static bool isControlFreak1Installed { get; set; }
    public static bool isControlFreak2Installed { get; set; }
    public static bool isRewiredInstalled { get; set; }
    public static bool isNetworkAddonInstalled {get; set; }
    public static bool isPhotonInstalled { get; set; }
    public static bool isBluetoothAddonInstalled { get; set; }
    public static GameObject controlFreakPrefab;
    public static InputTouchControllerBridge touchControllerBridge;
    #endregion
    
    #region screen definitions
    public static CombatScreen currentScreen{get; protected set;}
	public static CombatScreen battleGUI{get; protected set;}
	public static GameObject gameEngine{get; protected set;}
    #endregion

    #region trackable definitions
    public static bool freeCamera;
    public static bool freezePhysics;
    public static bool newRoundCasted;
    public static bool normalizedCam = true;
    public static bool pauseTimer;
    public static Fix64 timer;
    public static Fix64 timeScale;
    public static ControlsScript p1ControlsScript;
    public static ControlsScript p2ControlsScript;
    public static List<DelayedAction> delayedLocalActions = new List<DelayedAction>();
    public static List<DelayedAction> delayedSynchronizedActions = new List<DelayedAction>();
    public static List<InstantiatedGameObject> instantiatedObjects = new List<InstantiatedGameObject>();
    #endregion

    #region story mode definitions
    //-----------------------------------------------------------------------------------------------------------------
    // Required for the Story Mode: if the player lost its previous battle, 
    // he needs to fight the same opponent again, not the next opponent.
    //-----------------------------------------------------------------------------------------------------------------
    private static StoryModeInfo storyMode = new StoryModeInfo();
    private static List<string> unlockedCharactersInStoryMode = new List<string>();
    private static List<string> unlockedCharactersInVersusMode = new List<string>();
    private static bool player1WonLastBattle;
    private static int lastStageIndex;
    #endregion

    #region public definitions
    public static Fix64 fixedDeltaTime { get { return _fixedDeltaTime * timeScale; } set { _fixedDeltaTime = value; } }
    public static int intTimer;
    public static int fps = 60;
    public static long currentFrame { get; set; }
    public static bool gameRunning { get; protected set; }

    public static CombatController localPlayerController;
    public static CombatController remotePlayerController;
    #endregion

    #region private definitions
    private static Fix64 _fixedDeltaTime;
    private static AudioSource musicAudioSource;
	private static AudioSource soundsAudioSource;

	private static CombatController p1Controller;
	private static CombatController p2Controller;

	private static RandomAI p1RandomAI;
	private static RandomAI p2RandomAI;
	private static AbstractInputController p1FuzzyAI;
	private static AbstractInputController p2FuzzyAI;
	private static SimpleAI p1SimpleAI;
	private static SimpleAI p2SimpleAI;
    
    private static bool closing = false;
	private static bool disconnecting = false;
    private static List<object> memoryDump = new List<object>();
    #endregion


    #region public class methods: Delay the execution of a method maintaining synchronization between clients
    public static void DelayLocalAction(Action action, Fix64 seconds) {
        if (MainScript.fixedDeltaTime > 0) {
            MainScript.DelayLocalAction(action, (int)FPMath.Floor(seconds * config.fps));
		}else{
			MainScript.DelayLocalAction(action, 1);
		}
	}

	public static void DelayLocalAction(Action action, int steps){
		MainScript.DelayLocalAction(new DelayedAction(action, steps));
	}

	public static void DelayLocalAction(DelayedAction delayedAction){
		MainScript.delayedLocalActions.Add(delayedAction);
	}

	public static void DelaySynchronizedAction(Action action, Fix64 seconds){
        if (MainScript.fixedDeltaTime > 0) {
            MainScript.DelaySynchronizedAction(action, (int)FPMath.Floor(seconds * config.fps));
		}else{
			MainScript.DelaySynchronizedAction(action, 1);
		}
	}

	public static void DelaySynchronizedAction(Action action, int steps){
		MainScript.DelaySynchronizedAction(new DelayedAction(action, steps));
	}

	public static void DelaySynchronizedAction(DelayedAction delayedAction){
		MainScript.delayedSynchronizedActions.Add(delayedAction);
	}
	
	
	public static bool FindDelaySynchronizedAction(Action action){
		foreach (DelayedAction delayedAction in MainScript.delayedSynchronizedActions){
			if (action == delayedAction.action) return true;
		}
		return false;
	}

    public static bool FindAndUpdateDelaySynchronizedAction(Action action, Fix64 seconds) {
		foreach (DelayedAction delayedAction in MainScript.delayedSynchronizedActions){
			if (action == delayedAction.action) {
				delayedAction.steps = (int)FPMath.Floor(seconds * config.fps);
				return true;
			}
		}
		return false;
	}

    public static void FindAndRemoveDelaySynchronizedAction(Action action) {
        foreach (DelayedAction delayedAction in MainScript.delayedSynchronizedActions) {
            if (action == delayedAction.action) {
                MainScript.delayedSynchronizedActions.Remove(delayedAction);
                return;
            }
        }
    }

    public static void FindAndRemoveDelayLocalAction(Action action) {
        foreach (DelayedAction delayedAction in MainScript.delayedLocalActions) {
            if (action == delayedAction.action) {
                MainScript.delayedLocalActions.Remove(delayedAction);
                return;
            }
        }
    }

    public static GameObject SpawnGameObject(GameObject gameObject, Vector3 position, Quaternion rotation, long? destroyTimer = null) {
        if (gameObject == null) return null;

        GameObject goInstance = UnityEngine.Object.Instantiate(gameObject, position, rotation);
        goInstance.transform.SetParent(MainScript.gameEngine.transform);
        MrFusion mrFusion = (MrFusion) goInstance.GetComponent(typeof(MrFusion));
        if (mrFusion == null) mrFusion = goInstance.AddComponent<MrFusion>();

        MainScript.instantiatedObjects.Add(new InstantiatedGameObject(goInstance, mrFusion, MainScript.currentFrame, MainScript.currentFrame + destroyTimer));

        return goInstance;
    }

    public static void DestroyGameObject(GameObject gameObject, long? destroyTimer = null) {
        for (int i = 0; i < MainScript.instantiatedObjects.Count; ++i) {
            if (MainScript.instantiatedObjects[i].gameObject == gameObject) {
                MainScript.instantiatedObjects[i].destructionFrame = destroyTimer == null? MainScript.currentFrame : destroyTimer;
                break;
            }
        }
    }

	#endregion

	#region public class methods: Audio related methods
	public static bool GetMusic(){
		return config.music;
	}

	public static AudioClip GetMusicClip(){
		return MainScript.musicAudioSource.clip;
	}
	
	public static bool GetSoundFX(){
		return config.soundfx;
	}

	public static float GetMusicVolume(){
		if (MainScript.config != null) return config.musicVolume;
		return 1f;
	}

	public static float GetSoundFXVolume(){
		if (MainScript.config != null) return MainScript.config.soundfxVolume;
		return 1f;
	}

	public static void InitializeAudioSystem(){
		Camera cam = Camera.main;

		// Create the AudioSources required for the music and sound effects
		MainScript.musicAudioSource = cam.GetComponent<AudioSource>();
		if (MainScript.musicAudioSource == null){
			MainScript.musicAudioSource = cam.gameObject.AddComponent<AudioSource>();
		}

		MainScript.musicAudioSource.loop = true;
		MainScript.musicAudioSource.playOnAwake = false;
		MainScript.musicAudioSource.volume = config.musicVolume;


		MainScript.soundsAudioSource = cam.gameObject.AddComponent<AudioSource>();
		MainScript.soundsAudioSource.loop = false;
		MainScript.soundsAudioSource.playOnAwake = false;
		MainScript.soundsAudioSource.volume = 1f;
	}

	public static bool IsPlayingMusic(){
		if (MainScript.musicAudioSource.clip != null) return MainScript.musicAudioSource.isPlaying;
		return false;
	}

	public static bool IsMusicLooped(){
		return MainScript.musicAudioSource.loop;
	}

	public static bool IsPlayingSoundFX(){
		return false;
	}

	public static void LoopMusic(bool loop){
		MainScript.musicAudioSource.loop = loop;
	}

	public static void PlayMusic(){
		if (config.music && !MainScript.IsPlayingMusic() && MainScript.musicAudioSource.clip != null){
			MainScript.musicAudioSource.Play();
		}
	}

	public static void PlayMusic(AudioClip music){
		if (music != null){
			AudioClip oldMusic = MainScript.GetMusicClip();

			if (music != oldMusic){
				MainScript.musicAudioSource.clip = music;
			}

			if (config.music && (music != oldMusic || !MainScript.IsPlayingMusic())){
				MainScript.musicAudioSource.Play();
			}
		}
	}

	public static void PlaySound(IList<AudioClip> sounds){
		if (sounds.Count > 0){
			MainScript.PlaySound(sounds[UnityEngine.Random.Range(0, sounds.Count)]);
		}
	}
	
	public static void PlaySound(AudioClip soundFX){
		MainScript.PlaySound(soundFX, MainScript.GetSoundFXVolume());
	}

	public static void PlaySound(AudioClip soundFX, float volume){
		if (config.soundfx && soundFX != null && MainScript.soundsAudioSource != null){
			MainScript.soundsAudioSource.PlayOneShot(soundFX, volume);
		}
	}
	
	public static void SetMusic(bool on){
		bool isPlayingMusic = MainScript.IsPlayingMusic();
		MainScript.config.music = on;

		if (on && !isPlayingMusic)		MainScript.PlayMusic();
		else if (!on && isPlayingMusic)	MainScript.StopMusic();

		PlayerPrefs.SetInt(MainScript.MusicEnabledKey, on ? 1 : 0);
		PlayerPrefs.Save();
	}
	
	public static void SetSoundFX(bool on){
		MainScript.config.soundfx = on;
		PlayerPrefs.SetInt(MainScript.SoundsEnabledKey, on ? 1 : 0);
		PlayerPrefs.Save();
	}
	
	public static void SetMusicVolume(float volume){
		if (MainScript.config != null) MainScript.config.musicVolume = volume;
		if (MainScript.musicAudioSource != null) MainScript.musicAudioSource.volume = volume;

		PlayerPrefs.SetFloat(MainScript.MusicVolumeKey, volume);
		PlayerPrefs.Save();
	}

	public static void SetSoundFXVolume(float volume){
		if (MainScript.config != null) MainScript.config.soundfxVolume = volume;
		PlayerPrefs.SetFloat(MainScript.SoundsVolumeKey, volume);
		PlayerPrefs.Save();
	}
    
    public static void StopMusic()
    {
        if (MainScript.musicAudioSource.clip != null) MainScript.musicAudioSource.Stop();
    }

    public static void StopSounds()
    {
        MainScript.soundsAudioSource.Stop();
    }
    #endregion

    #region public class methods: AI related methods
    public static void SetAIEngine(AIEngine engine){
		MainScript.config.aiOptions.engine = engine;
	}
	
	public static AIEngine GetAIEngine(){
		return MainScript.config.aiOptions.engine;
	}

    public static ChallengeModeOptions GetChallenge(int challengeNum) {
        return MainScript.config.challengeModeOptions[challengeNum];
    }
	
	public static void SetDebugMode(bool flag){
		MainScript.config.debugOptions.debugMode = flag;
		if (debugger1 != null) debugger1.enabled = flag;
        if (debugger2 != null) debugger2.enabled = flag;
	}

	public static void SetAIDifficulty(AIDifficultyLevel difficulty){
		foreach(AIDifficultySettings difficultySettings in MainScript.config.aiOptions.difficultySettings){
			if (difficultySettings.difficultyLevel == difficulty) {
				MainScript.SetAIDifficulty(difficultySettings);
				break;
			}
		}
	}

	public static void SetAIDifficulty(AIDifficultySettings difficulty){
		MainScript.config.aiOptions.selectedDifficulty = difficulty;
		MainScript.config.aiOptions.selectedDifficultyLevel = difficulty.difficultyLevel;

		for (int i = 0; i < MainScript.config.aiOptions.difficultySettings.Length; ++i){
			if (difficulty == MainScript.config.aiOptions.difficultySettings[i]){
				PlayerPrefs.SetInt(MainScript.DifficultyLevelKey, i);
				PlayerPrefs.Save();
				break;
			}
		}
	}

	public static void SetSimpleAI(int player, SimpleAIBehaviour behaviour){
		if (player == 1){
			MainScript.p1SimpleAI.behaviour = behaviour;
			MainScript.p1Controller.cpuController = MainScript.p1SimpleAI;
		}else if (player == 2){
			MainScript.p2SimpleAI.behaviour = behaviour;
			MainScript.p2Controller.cpuController = MainScript.p2SimpleAI;
		}
	}

	public static void SetRandomAI(int player){
		if (player == 1){
			MainScript.p1Controller.cpuController = MainScript.p1RandomAI;
		}else if (player == 2){
			MainScript.p2Controller.cpuController = MainScript.p2RandomAI;
		}
	}

	public static void SetFuzzyAI(int player, CharacterInfo character){
		MainScript.SetFuzzyAI(player, character, MainScript.config.aiOptions.selectedDifficulty);
	}

	public static void SetFuzzyAI(int player, CharacterInfo character, AIDifficultySettings difficulty){
		if (MainScript.isAiAddonInstalled){
			if (player == 1){
				MainScript.p1Controller.cpuController = MainScript.p1FuzzyAI;
			}else if (player == 2){
				MainScript.p2Controller.cpuController = MainScript.p2FuzzyAI;
			}

			CombatController controller = MainScript.GetController(player);
			if (controller != null && controller.isCPU){
				AbstractInputController cpu = controller.cpuController;

				if (cpu != null){
					MethodInfo method = cpu.GetType().GetMethod(
						"SetAIInformation", 
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy,
						null,
						new Type[]{typeof(ScriptableObject)},
						null
					);

					if (method != null){
						if (character != null && character.aiInstructionsSet != null && character.aiInstructionsSet.Length > 0){
							if (difficulty.startupBehavior == AIBehavior.Any){
								method.Invoke(cpu, new object[]{character.aiInstructionsSet[0].aiInfo});
							}else{
								ScriptableObject selectedAIInfo = character.aiInstructionsSet[0].aiInfo;
								foreach(AIInstructionsSet instructionSet in character.aiInstructionsSet){
									if (instructionSet.behavior == difficulty.startupBehavior){
										selectedAIInfo = instructionSet.aiInfo;
										break;
									}
								}
								method.Invoke(cpu, new object[]{selectedAIInfo});
							}
						}else{
							method.Invoke(cpu, new object[]{null});
						}
					}
				}
			}
		}
	}
	#endregion

	#region public class methods: Story Mode related methods
	public static CharacterStory GetCharacterStory(CharacterInfo character){
		if (!MainScript.config.storyMode.useSameStoryForAllCharacters){
			StoryMode storyMode = MainScript.config.storyMode;

			for (int i = 0; i < MainScript.config.characters.Length; ++i){
				if (MainScript.config.characters[i] == character && storyMode.selectableCharactersInStoryMode.Contains(i)){
					CharacterStory characterStory = null;

					if (storyMode.characterStories.TryGetValue(i, out characterStory) && characterStory != null){
						return characterStory;
					}
				}
			}
		}
		
		return MainScript.config.storyMode.defaultStory;
	}
	

	public static AIDifficultySettings GetAIDifficulty(){
		return MainScript.config.aiOptions.selectedDifficulty;
	}
	#endregion

	#region public class methods: GUI Related methods
	public static BattleGUI GetBattleGUI(){
		return MainScript.config.gameGUI.battleGUI;
	}

	public static BluetoothGameScreen GetBluetoothGameScreen(){
		return MainScript.config.gameGUI.bluetoothGameScreen;
	}

	public static CharacterSelectionScreen GetCharacterSelectionScreen(){
		return MainScript.config.gameGUI.characterSelectionScreen;
	}

	public static ConnectionLostScreen GetConnectionLostScreen(){
		return MainScript.config.gameGUI.connectionLostScreen;
	}

	public static CreditsScreen GetCreditsScreen(){
		return MainScript.config.gameGUI.creditsScreen;
	}

	public static HostGameScreen GetHostGameScreen(){
		return MainScript.config.gameGUI.hostGameScreen;
	}

	public static IntroScreen GetIntroScreen(){
		return MainScript.config.gameGUI.introScreen;
	}

	public static JoinGameScreen GetJoinGameScreen(){
		return MainScript.config.gameGUI.joinGameScreen;
	}

	public static LoadingBattleScreen GetLoadingBattleScreen(){
		return MainScript.config.gameGUI.loadingBattleScreen;
	}

	public static MainMenuScreen GetMainMenuScreen(){
		return MainScript.config.gameGUI.mainMenuScreen;
	}

	public static NetworkGameScreen GetNetworkGameScreen(){
		return MainScript.config.gameGUI.networkGameScreen;
	}

	public static OptionsScreen GetOptionsScreen(){
		return MainScript.config.gameGUI.optionsScreen;
	}

	public static StageSelectionScreen GetStageSelectionScreen(){
		return MainScript.config.gameGUI.stageSelectionScreen;
	}

	public static StoryModeScreen GetStoryModeCongratulationsScreen(){
		return MainScript.config.gameGUI.storyModeCongratulationsScreen;
	}

	public static StoryModeContinueScreen GetStoryModeContinueScreen(){
		return MainScript.config.gameGUI.storyModeContinueScreen;
	}

	public static StoryModeScreen GetStoryModeGameOverScreen(){
		return MainScript.config.gameGUI.storyModeGameOverScreen;
	}

	public static VersusModeAfterBattleScreen GetVersusModeAfterBattleScreen(){
		return MainScript.config.gameGUI.versusModeAfterBattleScreen;
	}

	public static VersusModeScreen GetVersusModeScreen(){
		return MainScript.config.gameGUI.versusModeScreen;
	}

	public static void HideScreen(CombatScreen screen){
		if (screen != null){
			screen.OnHide();
			GameObject.Destroy(screen.gameObject);
            if (!gameRunning && gameEngine != null) MainScript.EndGame();
		}
	}
	
	public static void ShowScreen(CombatScreen screen, Action nextScreenAction = null){
		if (screen != null){
			if (MainScript.OnScreenChanged != null){
				MainScript.OnScreenChanged(MainScript.currentScreen, screen);
			}

			MainScript.currentScreen = (CombatScreen) GameObject.Instantiate(screen);
			MainScript.currentScreen.transform.SetParent(MainScript.canvas != null ? MainScript.canvas.transform : null, false);

			StoryModeScreen storyModeScreen = MainScript.currentScreen as StoryModeScreen;
			if (storyModeScreen != null){
				storyModeScreen.nextScreenAction = nextScreenAction;
			}

			MainScript.currentScreen.OnShow ();
		}
	}

	public static void Quit(){
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

	public static void StartBluetoothGameScreen(){
		MainScript.StartBluetoothGameScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}
	
	public static void StartBluetoothGameScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartBluetoothGameScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartBluetoothGameScreen(fadeTime / 2f);
        }
	}

	public static void StartCharacterSelectionScreen(){
		MainScript.StartCharacterSelectionScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartCharacterSelectionScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartCharacterSelectionScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartCharacterSelectionScreen(fadeTime / 2f);
        }
	}

	public static void StartCpuVersusCpu(){
		MainScript.StartCpuVersusCpu((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartCpuVersusCpu(float fadeTime){
		MainScript.gameMode = GameMode.VersusMode;
		MainScript.SetCPU(1, true);
		MainScript.SetCPU(2, true);
		MainScript.StartCharacterSelectionScreen(fadeTime);
	}

	public static void StartConnectionLostScreenIfMainMenuNotLoaded(){
		MainScript.StartConnectionLostScreenIfMainMenuNotLoaded((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartConnectionLostScreenIfMainMenuNotLoaded(float fadeTime){
		if ((MainScript.currentScreen as MainMenuScreen) == null){
			MainScript.StartConnectionLostScreen(fadeTime);
		}
	}

	public static void StartConnectionLostScreen(){
		MainScript.StartConnectionLostScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartConnectionLostScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartConnectionLostScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartConnectionLostScreen(fadeTime / 2f);
        }
	}

	public static void StartCreditsScreen(){
		MainScript.StartCreditsScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartCreditsScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartCreditsScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartCreditsScreen(fadeTime / 2f);
        }
	}

	public static void StartGame(){
		MainScript.StartGame((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartGame(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.gameFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartGame(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartGame(fadeTime / 2f);
        }
	}

	public static void StartHostGameScreen(){
		MainScript.StartHostGameScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartHostGameScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartHostGameScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartHostGameScreen(fadeTime / 2f);
        }
	}

	public static void StartIntroScreen(){
		MainScript.StartIntroScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartIntroScreen(float fadeTime){
        if (MainScript.currentScreen != null && MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartIntroScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartIntroScreen(fadeTime / 2f);
        }
	}

	public static void StartJoinGameScreen(){
		MainScript.StartJoinGameScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartJoinGameScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartJoinGameScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartJoinGameScreen(fadeTime / 2f);
        }
	}

	public static void StartLoadingBattleScreen(){
		MainScript.StartLoadingBattleScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartLoadingBattleScreen(float fadeTime){
        if (MainScript.currentScreen != null && MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartLoadingBattleScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartLoadingBattleScreen(fadeTime / 2f);
        }
	}

	public static void StartMainMenuScreen(){
		MainScript.StartMainMenuScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartMainMenuScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartMainMenuScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartMainMenuScreen(fadeTime / 2f);
        }
	}

	public static void StartSearchMatchScreen(){
		MainScript.StartSearchMatchScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartSearchMatchScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartSearchMatchScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartSearchMatchScreen(fadeTime / 2f);
        }
	}

	public static void StartNetworkGameScreen(){
		MainScript.StartNetworkGameScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartNetworkGameScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartNetworkGameScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartNetworkGameScreen(fadeTime / 2f);
        }
	}

	public static void StartOptionsScreen(){
		MainScript.StartOptionsScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartOptionsScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartOptionsScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartOptionsScreen(fadeTime / 2f);
        }
	}

	public static void StartPlayerVersusPlayer(){
		MainScript.StartPlayerVersusPlayer((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartPlayerVersusPlayer(float fadeTime){
		MainScript.gameMode = GameMode.VersusMode;
		MainScript.SetCPU(1, false);
		MainScript.SetCPU(2, false);
		MainScript.StartCharacterSelectionScreen(fadeTime);
	}

	public static void StartPlayerVersusCpu(){
		MainScript.StartPlayerVersusCpu((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartPlayerVersusCpu(float fadeTime){
		MainScript.gameMode = GameMode.VersusMode;
		MainScript.SetCPU(1, false);
		MainScript.SetCPU(2, true);
		MainScript.StartCharacterSelectionScreen(fadeTime);
	}

	public static void StartNetworkGame(float fadeTime, int localPlayer, bool startImmediately){
		MainScript.disconnecting = false;
		Application.runInBackground = true;

		MainScript.localPlayerController.Initialize(MainScript.p1Controller.inputReferences);
		MainScript.localPlayerController.humanController = MainScript.p1Controller.humanController;
		MainScript.localPlayerController.cpuController = MainScript.p1Controller.cpuController;
		MainScript.remotePlayerController.Initialize(MainScript.p2Controller.inputReferences);

		if (localPlayer == 1){
			MainScript.localPlayerController.player = 1;
			MainScript.remotePlayerController.player = 2;
		}else{
			MainScript.localPlayerController.player = 2;
			MainScript.remotePlayerController.player = 1;
		}

		MainScript.fluxCapacitor.Initialize();
		MainScript.gameMode = GameMode.NetworkGame;
		MainScript.SetCPU(1, false);
		MainScript.SetCPU(2, false);

        if (startImmediately) {
            MainScript.StartLoadingBattleScreen(fadeTime);
            //MainScript.StartGame();
        } else {
            MainScript.StartCharacterSelectionScreen(fadeTime);
        }
	}

	public static void StartStageSelectionScreen(){
		MainScript.StartStageSelectionScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartStageSelectionScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartStageSelectionScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartStageSelectionScreen(fadeTime / 2f);
        }
	}

	public static void StartStoryMode(){
		MainScript.StartStoryMode((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartStoryMode(float fadeTime){
		//-------------------------------------------------------------------------------------------------------------
		// Required for loading the first combat correctly.
		MainScript.player1WonLastBattle = true; 
		//-------------------------------------------------------------------------------------------------------------
		MainScript.gameMode = GameMode.StoryMode;
		MainScript.SetCPU(1, false);
		MainScript.SetCPU(2, true);
		MainScript.storyMode.characterStory = null;
		MainScript.storyMode.canFightAgainstHimself = MainScript.config.storyMode.canCharactersFightAgainstThemselves;
		MainScript.storyMode.currentGroup = -1;
		MainScript.storyMode.currentBattle = -1;
		MainScript.storyMode.currentBattleInformation = null;
		MainScript.storyMode.defeatedOpponents.Clear();
		MainScript.StartCharacterSelectionScreen(fadeTime);
	}

	public static void StartStoryModeBattle(){
		MainScript.StartStoryModeBattle((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartStoryModeBattle(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartStoryModeBattle(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartStoryModeBattle(fadeTime / 2f);
        }
	}

	public static void StartStoryModeCongratulationsScreen(){
		MainScript.StartStoryModeCongratulationsScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartStoryModeCongratulationsScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartStoryModeCongratulationsScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartStoryModeCongratulationsScreen(fadeTime / 2f);
        }
	}

	public static void StartStoryModeContinueScreen(){
		MainScript.StartStoryModeContinueScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartStoryModeContinueScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartStoryModeContinueScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartStoryModeContinueScreen(fadeTime / 2f);
        }
	}

	public static void StartStoryModeConversationAfterBattleScreen(CombatScreen conversationScreen){
		MainScript.StartStoryModeConversationAfterBattleScreen(conversationScreen, (float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartStoryModeConversationAfterBattleScreen(CombatScreen conversationScreen, float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartStoryModeConversationAfterBattleScreen(conversationScreen, fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartStoryModeConversationAfterBattleScreen(conversationScreen, fadeTime / 2f);
        }
	}

	public static void StartStoryModeConversationBeforeBattleScreen(CombatScreen conversationScreen){
		MainScript.StartStoryModeConversationBeforeBattleScreen(conversationScreen, (float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartStoryModeConversationBeforeBattleScreen(CombatScreen conversationScreen, float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartStoryModeConversationBeforeBattleScreen(conversationScreen, fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartStoryModeConversationBeforeBattleScreen(conversationScreen, fadeTime / 2f);
        }
	}

	public static void StartStoryModeEndingScreen(){
		MainScript.StartStoryModeEndingScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartStoryModeEndingScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartStoryModeEndingScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartStoryModeEndingScreen(fadeTime / 2f);
        }
	}

	public static void StartStoryModeGameOverScreen(){
		MainScript.StartStoryModeGameOverScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartStoryModeGameOverScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartStoryModeGameOverScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartStoryModeGameOverScreen(fadeTime / 2f);
        }
	}

	public static void StartStoryModeOpeningScreen(){
		MainScript.StartStoryModeOpeningScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartStoryModeOpeningScreen(float fadeTime){
		// First, retrieve the character story, so we can find the opening associated to this player
		MainScript.storyMode.characterStory = MainScript.GetCharacterStory(MainScript.GetPlayer1());

        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartStoryModeOpeningScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartStoryModeOpeningScreen(fadeTime / 2f);
        }
	}

	public static void StartTrainingMode(){
		MainScript.StartTrainingMode((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartTrainingMode(float fadeTime){
		MainScript.gameMode = GameMode.TrainingRoom;
		MainScript.SetCPU(1, false);
		MainScript.SetCPU(2, false);
		MainScript.StartCharacterSelectionScreen(fadeTime);
	}

	public static void StartVersusModeAfterBattleScreen(){
		MainScript.StartVersusModeAfterBattleScreen(0f);
	}

	public static void StartVersusModeAfterBattleScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartVersusModeAfterBattleScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartVersusModeAfterBattleScreen(fadeTime / 2f);
        }
	}

	public static void StartVersusModeScreen(){
		MainScript.StartVersusModeScreen((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void StartVersusModeScreen(float fadeTime){
        if (MainScript.currentScreen.hasFadeOut) {
            MainScript.eventSystem.enabled = false;
            CameraFade.StartAlphaFade(
                MainScript.config.gameGUI.screenFadeColor,
                false,
                fadeTime / 2f,
                0f
            );
            MainScript.DelayLocalAction(() => { MainScript.eventSystem.enabled = true; MainScript._StartVersusModeScreen(fadeTime / 2f); }, (Fix64)fadeTime / 2);
        } else {
            MainScript._StartVersusModeScreen(fadeTime / 2f);
        }
	}

	public static void WonStoryModeBattle(){
		MainScript.WonStoryModeBattle((float)MainScript.config.gameGUI.screenFadeDuration);
	}

	public static void WonStoryModeBattle(float fadeTime){
		MainScript.storyMode.defeatedOpponents.Add(MainScript.storyMode.currentBattleInformation.opponentCharacterIndex);
		MainScript.StartStoryModeConversationAfterBattleScreen(MainScript.storyMode.currentBattleInformation.conversationAfterBattle, fadeTime);
	}
	#endregion

	#region public class methods: Language
	public static void SetLanguage(){
		foreach(LanguageOptions languageOption in config.languages){
			if (languageOption.defaultSelection){
				config.selectedLanguage = languageOption;
				return;
			}
		}
	}

	public static void SetLanguage(string language){
		foreach(LanguageOptions languageOption in config.languages){
			if (language == languageOption.languageName){
				config.selectedLanguage = languageOption;
				return;
			}
		}
	}
	#endregion

	#region public class methods: Input Related methods
	public static bool GetCPU(int player){
		CombatController controller = MainScript.GetController(player);
		if (controller != null){
			return controller.isCPU;
		}
		return false;
	}

	public static string GetInputReference(ButtonPress button, InputReferences[] inputReferences){
		foreach(InputReferences inputReference in inputReferences){
			if (inputReference.engineRelatedButton == button) return inputReference.inputButtonName;
		}
		return null;
	}
	
	public static string GetInputReference(InputType inputType, InputReferences[] inputReferences){
		foreach(InputReferences inputReference in inputReferences){
			if (inputReference.inputType == inputType) return inputReference.inputButtonName;
		}
		return null;
	}

	public static CombatController GetPlayer1Controller(){
		if (MainScript.isNetworkAddonInstalled && MainScript.isConnected){
			if (MainScript.multiplayerAPI.IsServer()){
				return MainScript.localPlayerController;
			}else{
				return MainScript.remotePlayerController;
			}
		}
		return MainScript.p1Controller;
	}
	
	public static CombatController GetPlayer2Controller(){
		if (MainScript.isNetworkAddonInstalled && MainScript.isConnected){
			if (MainScript.multiplayerAPI.IsServer()){
				return MainScript.remotePlayerController;
			}else{
				return MainScript.localPlayerController;
			}
		}
		return MainScript.p2Controller;
	}
	
	public static CombatController GetController(int player){
		if		(player == 1)	return MainScript.GetPlayer1Controller();
		else if (player == 2)	return MainScript.GetPlayer2Controller();
		else					return null;
	}
	
	public static int GetLocalPlayer(){
		if		(MainScript.localPlayerController == MainScript.GetPlayer1Controller())	return 1;
		else if	(MainScript.localPlayerController == MainScript.GetPlayer2Controller())	return 2;
		else																return -1;
	}
	
	public static int GetRemotePlayer(){
		if		(MainScript.remotePlayerController == MainScript.GetPlayer1Controller())	return 1;
		else if	(MainScript.remotePlayerController == MainScript.GetPlayer2Controller())	return 2;
		else																return -1;
	}

	public static void SetAI(int player, CharacterInfo character){
		if (MainScript.isAiAddonInstalled){
			CombatController controller = MainScript.GetController(player);
			
			if (controller != null && controller.isCPU){
				AbstractInputController cpu = controller.cpuController;
				
				if (cpu != null){
					MethodInfo method = cpu.GetType().GetMethod(
						"SetAIInformation", 
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy,
						null,
						new Type[]{typeof(ScriptableObject)},
					null
					);
					
					if (method != null){
						if (character != null && character.aiInstructionsSet != null && character.aiInstructionsSet.Length > 0){
							method.Invoke(cpu, new object[]{character.aiInstructionsSet[0].aiInfo});
						}else{
							method.Invoke(cpu, new object[]{null});
						}
					}
				}
			}
		}
	}

	public static void SetCPU(int player, bool cpuToggle){
		CombatController controller = MainScript.GetController(player);
		if (controller != null){
			controller.isCPU = cpuToggle;
		}
	}
	#endregion

	#region public class methods: methods related to the character selection
	public static CharacterInfo GetPlayer(int player){
		if (player == 1){
			return MainScript.GetPlayer1();
		}else if (player == 2){
			return MainScript.GetPlayer2();
		}
		return null;
	}
	
	public static CharacterInfo GetPlayer1(){
		return config.player1Character;
	}
	
	public static CharacterInfo GetPlayer2(){
		return config.player2Character;
	}

	public static CharacterInfo[] GetStoryModeSelectableCharacters(){
		List<CharacterInfo> characters = new List<CharacterInfo>();

		for (int i = 0; i < MainScript.config.characters.Length; ++i){
			if(
				MainScript.config.characters[i] != null 
				&& 
				(
					MainScript.config.storyMode.selectableCharactersInStoryMode.Contains(i) || 
					MainScript.unlockedCharactersInStoryMode.Contains(MainScript.config.characters[i].characterName)
				)
			){
				characters.Add(MainScript.config.characters[i]);
			}
		}
		
		return characters.ToArray();
	}

	public static CharacterInfo[] GetTrainingRoomSelectableCharacters(){
		List<CharacterInfo> characters = new List<CharacterInfo>();
		
		for (int i = 0; i < MainScript.config.characters.Length; ++i){
			// If the character is selectable on Story Mode or Versus Mode,
			// then the character should be selectable on Training Room...
			if(
				MainScript.config.characters[i] != null 
				&& 
				(
					MainScript.config.storyMode.selectableCharactersInStoryMode.Contains(i) || 
					MainScript.config.storyMode.selectableCharactersInVersusMode.Contains(i) || 
					MainScript.unlockedCharactersInStoryMode.Contains(MainScript.config.characters[i].characterName) ||
					MainScript.unlockedCharactersInVersusMode.Contains(MainScript.config.characters[i].characterName)
				)
			){
				characters.Add(MainScript.config.characters[i]);
			}
		}
		
		return characters.ToArray();
	}
	
	public static CharacterInfo[] GetVersusModeSelectableCharacters(){
		List<CharacterInfo> characters = new List<CharacterInfo>();
		
		for (int i = 0; i < MainScript.config.characters.Length; ++i){
			if(
				MainScript.config.characters[i] != null && 
				(
					MainScript.config.storyMode.selectableCharactersInVersusMode.Contains(i) || 
					MainScript.unlockedCharactersInVersusMode.Contains(MainScript.config.characters[i].characterName)
				)
			){
				characters.Add(MainScript.config.characters[i]);
			}
		}
		
		return characters.ToArray();
	}

	public static void SetPlayer(int player, CharacterInfo info){
		if (player == 1){
			config.player1Character = info;
		}else if (player == 2){
			config.player2Character = info;
		}
	}

	public static void SetPlayer1(CharacterInfo player1){
		config.player1Character = player1;
	}

	public static void SetPlayer2(CharacterInfo player2){
		config.player2Character = player2;
	}

	public static void LoadUnlockedCharacters(){
		MainScript.unlockedCharactersInStoryMode.Clear();
		string value = PlayerPrefs.GetString("UCSM", null);

		if (!string.IsNullOrEmpty(value)){
			string[] characters = value.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);
			foreach (string character in characters){
				unlockedCharactersInStoryMode.Add(character);
			}
		}


		MainScript.unlockedCharactersInVersusMode.Clear();
		value = PlayerPrefs.GetString("UCVM", null);
		
		if (!string.IsNullOrEmpty(value)){
			string[] characters = value.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);
			foreach (string character in characters){
				unlockedCharactersInVersusMode.Add(character);
			}
		}
	}

	public static void SaveUnlockedCharacters(){
		StringBuilder sb = new StringBuilder();
		foreach (string characterName in MainScript.unlockedCharactersInStoryMode){
			if (!string.IsNullOrEmpty(characterName)){
				if (sb.Length > 0){
					sb.AppendLine();
				}
				sb.Append(characterName);
			}
		}
		PlayerPrefs.SetString("UCSM", sb.ToString());

		sb = new StringBuilder();
		foreach (string characterName in MainScript.unlockedCharactersInVersusMode){
			if (!string.IsNullOrEmpty(characterName)){
				if (sb.Length > 0){
					sb.AppendLine();
				}
				sb.Append(characterName);
			}
		}
		PlayerPrefs.SetString("UCVM", sb.ToString());
		PlayerPrefs.Save();
	}

	public static void RemoveUnlockedCharacterInStoryMode(CharacterInfo character){
		if (character != null && !string.IsNullOrEmpty(character.characterName)){
			MainScript.unlockedCharactersInStoryMode.Remove(character.characterName);
		}
		
		MainScript.SaveUnlockedCharacters();
	}

	public static void RemoveUnlockedCharacterInVersusMode(CharacterInfo character){
		if (character != null && !string.IsNullOrEmpty(character.characterName)){
			MainScript.unlockedCharactersInVersusMode.Remove(character.characterName);
		}
		
		MainScript.SaveUnlockedCharacters();
	}

	public static void RemoveUnlockedCharactersInStoryMode(){
		MainScript.unlockedCharactersInStoryMode.Clear();
		MainScript.SaveUnlockedCharacters();
	}
	
	public static void RemoveUnlockedCharactersInVersusMode(){
		MainScript.unlockedCharactersInVersusMode.Clear();
		MainScript.SaveUnlockedCharacters();
	}

	public static void UnlockCharacterInStoryMode(CharacterInfo character){
		if(
			character != null && 
			!string.IsNullOrEmpty(character.characterName) &&
			!MainScript.unlockedCharactersInStoryMode.Contains(character.characterName)
		){
			MainScript.unlockedCharactersInStoryMode.Add(character.characterName);
		}

		MainScript.SaveUnlockedCharacters();
	}

	public static void UnlockCharacterInVersusMode(CharacterInfo character){
		if(
			character != null && 
			!string.IsNullOrEmpty(character.characterName) &&
			!MainScript.unlockedCharactersInVersusMode.Contains(character.characterName)
		){
			MainScript.unlockedCharactersInVersusMode.Add(character.characterName);
		}

		MainScript.SaveUnlockedCharacters();
	}
	#endregion

	#region public class methods: methods related to the stage selection
	public static void SetStage(StageOptions stage){
		config.selectedStage = stage;
	}

	public static void SetStage(string stageName){
		foreach(StageOptions stage in config.stages){
			if (stageName == stage.stageName){
				MainScript.SetStage(stage);
				return;
			}
		}
	}
	
	public static StageOptions GetStage(){
		return config.selectedStage;
	}
	#endregion


	#region public class methods: methods related to the behaviour of the characters during the battle
	public static ControlsScript GetControlsScript(int player){
		if (player == 1){
			return MainScript.GetPlayer1ControlsScript();
		}else if (player == 2){
			return MainScript.GetPlayer2ControlsScript();
		}
		return null;
	}

	public static ControlsScript GetPlayer1ControlsScript(){
		return p1ControlsScript;
	}
	
	public static ControlsScript GetPlayer2ControlsScript(){
		return p2ControlsScript;
	}
	#endregion

	#region public class methods: methods that are used for raising events
	public static void SetLifePoints(Fix64 newValue, CharacterInfo player){
		if (MainScript.OnLifePointsChange != null) MainScript.OnLifePointsChange((float)newValue, player);
	}

	public static void FireAlert(string alertMessage, CharacterInfo player){
		if (MainScript.OnNewAlert != null) MainScript.OnNewAlert(alertMessage, player);
	}

	public static void FireHit(HitBox strokeHitBox, MoveInfo move, CharacterInfo player){
		if (MainScript.OnHit != null) MainScript.OnHit(strokeHitBox, move, player);
	}

    public static void FireBlock(HitBox strokeHitBox, MoveInfo move, CharacterInfo player) {
        if (MainScript.OnBlock != null) MainScript.OnBlock(strokeHitBox, move, player);
    }

    public static void FireParry(HitBox strokeHitBox, MoveInfo move, CharacterInfo player) {
        if (MainScript.OnParry != null) MainScript.OnParry(strokeHitBox, move, player);
    }
	
	public static void FireMove(MoveInfo move, CharacterInfo player){
		if (MainScript.OnMove != null) MainScript.OnMove(move, player);
	}

    public static void FireButton(ButtonPress button, CharacterInfo player) {
        if (MainScript.OnButton != null) MainScript.OnButton(button, player);
    }

    public static void FireBasicMove(BasicMoveReference basicMove, CharacterInfo player) {
        if (MainScript.OnBasicMove != null) MainScript.OnBasicMove(basicMove, player);
    }

    public static void FireBodyVisibilityChange(MoveInfo move, CharacterInfo player, BodyPartVisibilityChange bodyPartVisibilityChange, HitBox hitBox) {
        if (MainScript.OnBodyVisibilityChange != null) MainScript.OnBodyVisibilityChange(move, player, bodyPartVisibilityChange, hitBox);
    }

    public static void FireParticleEffects(MoveInfo move, CharacterInfo player, MoveParticleEffect particleEffects) {
        if (MainScript.OnParticleEffects != null) MainScript.OnParticleEffects(move, player, particleEffects);
    }

    public static void FireSideSwitch(int side, CharacterInfo player) {
        if (MainScript.OnSideSwitch != null) MainScript.OnSideSwitch(side, player);
    }

	public static void FireGameBegins(){
		if (MainScript.OnGameBegin != null) {
			gameRunning = true;
			MainScript.OnGameBegin(config.player1Character, config.player2Character, config.selectedStage);
		}
	}
	
	public static void FireGameEnds(CharacterInfo winner, CharacterInfo loser){
		// I've commented the next line because it worked with the old GUI, but not with the new one.
		//MainScript.EndGame();

        MainScript.timeScale = MainScript.config._gameSpeed;
		MainScript.gameRunning = false;
		MainScript.newRoundCasted = false;
		MainScript.player1WonLastBattle = (winner == MainScript.GetPlayer1());

		/*if (MainScript.fluxGameManager != null){
			MainScript.fluxGameManager.Initialize();
		}*/

		if (MainScript.OnGameEnds != null) {
			MainScript.OnGameEnds(winner, loser);
		}
	}
	
	public static void FireRoundBegins(int currentRound){
		if (MainScript.OnRoundBegins != null) MainScript.OnRoundBegins(currentRound);
	}

	public static void FireRoundEnds(CharacterInfo winner, CharacterInfo loser){
		if (MainScript.OnRoundEnds != null) MainScript.OnRoundEnds(winner, loser);
	}

	public static void FireTimer(float timer){
		if (MainScript.OnTimer != null) MainScript.OnTimer(timer);
	}

	public static void FireTimeOver(){
		if (MainScript.OnTimeOver != null) MainScript.OnTimeOver();
	}
	#endregion

    
	#region public class methods: CORE methods
	public static void PauseGame(bool pause){
        if (pause && MainScript.timeScale == 0) return;

		if (pause){
            MainScript.timeScale = 0;
		}else{
            MainScript.timeScale = MainScript.config._gameSpeed;
		}

		if (MainScript.OnGamePaused != null){
			MainScript.OnGamePaused(pause);
		}
	}

	public static bool IsInstalled(string theClass){
		return MainScript.SearchClass(theClass) != null;
	}
	
	public static bool isPaused(){
        return MainScript.timeScale <= 0;
	}
	
	public static Fix64 GetTimer(){
		return timer;
	}
	
	public static void ResetTimer(){
		timer = config.roundOptions._timer;
		intTimer = (int)FPMath.Round(config.roundOptions._timer);
		if (MainScript.OnTimer != null) OnTimer((float)timer);
	}
	
	public static Type SearchClass(string theClass){
		Type type = null;
		
		foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()){
			type = assembly.GetType(theClass);
			if (type != null){break;}
		}
		
		return type;
	}
	
	public static void SetTimer(Fix64 time){
		timer = time;
		intTimer = (int)FPMath.Round(time);
		if (MainScript.OnTimer != null) OnTimer(timer);
	}
	
	public static void PlayTimer(){
		pauseTimer = false;
	}
	
	public static void PauseTimer(){
		pauseTimer = true;
	}
	
	public static bool IsTimerPaused(){
		return pauseTimer;
	}

    public static void EndGame(){
        /*MainScript.timeScale = MainScript.ToDouble(MainScript.config.gameSpeed);
		MainScript.gameRunning = false;
		MainScript.newRoundCasted = false;*/

		if (MainScript.battleGUI != null){
			MainScript.battleGUI.OnHide();
            GameObject.Destroy(MainScript.battleGUI.gameObject);
            MainScript.battleGUI = null;
		}
		
		if (gameEngine != null) {
			GameObject.Destroy(gameEngine);
			gameEngine = null;
		}
	}

	public static void ResetRoundCast(){
		newRoundCasted = false;
	}
	
	public static void CastNewRound(){
		if (newRoundCasted) return;
		if (p1ControlsScript.introPlayed && p2ControlsScript.introPlayed){
            MainScript.FireRoundBegins(config.currentRound);
			MainScript.DelaySynchronizedAction(StartFight, (Fix64)2);
			newRoundCasted = true;
		}
	}

    public static void StartFight() {
        if (MainScript.gameMode != GameMode.ChallengeMode) 
            MainScript.FireAlert(MainScript.config.selectedLanguage.fight, null);
        MainScript.config.lockInputs = false;
        MainScript.config.lockMovements = false;
        MainScript.PlayTimer();
    }

	public static void CastInput(InputReferences[] inputReferences, int player){
		if (MainScript.OnInput != null) OnInput(inputReferences, player);
	}
	#endregion

	#region public class methods: Network Related methods
	public static void HostBluetoothGame(){
		if (MainScript.isNetworkAddonInstalled){
			MainScript.multiplayerMode = MultiplayerMode.Bluetooth;
			MainScript.AddNetworkEventListeners();
			MainScript.multiplayerAPI.CreateMatch(new MultiplayerAPI.MatchCreationRequest(MainScript.config.networkOptions.port, null, 1, false, null));
		}
	}

	public static void HostGame(){
		if (MainScript.isNetworkAddonInstalled){
			MainScript.multiplayerMode = MultiplayerMode.Lan;

			MainScript.AddNetworkEventListeners();
			MainScript.multiplayerAPI.CreateMatch(new MultiplayerAPI.MatchCreationRequest(MainScript.config.networkOptions.port, null, 1, false, null));
		}
	}

	public static void JoinBluetoothGame(){
		if (MainScript.isNetworkAddonInstalled){
			MainScript.multiplayerMode = MultiplayerMode.Bluetooth;

			MainScript.multiplayerAPI.OnMatchesDiscovered += MainScript.OnMatchesDiscovered;
			MainScript.multiplayerAPI.OnMatchDiscoveryError += MainScript.OnMatchDiscoveryError;
			MainScript.multiplayerAPI.StartSearchingMatches();
		}
	}

	protected static void OnMatchesDiscovered(ReadOnlyCollection<MultiplayerAPI.MatchInformation> matches){
		MainScript.multiplayerAPI.OnMatchesDiscovered -= MainScript.OnMatchesDiscovered;
		MainScript.multiplayerAPI.OnMatchDiscoveryError -= MainScript.OnMatchDiscoveryError;
		MainScript.AddNetworkEventListeners();

		if (matches != null && matches.Count > 0){
			// TODO: let the player choose the desired game
			MainScript.multiplayerAPI.JoinMatch(matches[0]);
		}else{
			MainScript.StartConnectionLostScreen();
		}
    }
    
	protected static void OnMatchDiscoveryError(){
		MainScript.multiplayerAPI.OnMatchesDiscovered -= MainScript.OnMatchesDiscovered;
		MainScript.multiplayerAPI.OnMatchDiscoveryError -= MainScript.OnMatchDiscoveryError;
		MainScript.StartConnectionLostScreen();
    }

	public static void JoinGame(MultiplayerAPI.MatchInformation match){
		if (MainScript.isNetworkAddonInstalled){
			MainScript.multiplayerMode = MultiplayerMode.Lan;

			MainScript.AddNetworkEventListeners();
			MainScript.multiplayerAPI.JoinMatch(match);
		}
	}

	public static void DisconnectFromGame(){
		if (MainScript.isNetworkAddonInstalled){
			NetworkState state = MainScript.multiplayerAPI.GetConnectionState();
			if (state == NetworkState.Client){
				MainScript.multiplayerAPI.DisconnectFromMatch();
			}else if (state == NetworkState.Server){
				MainScript.multiplayerAPI.DestroyMatch();
			}
		}
	}
	#endregion
    

	#region protected instance methods: MonoBehaviour methods
	protected void Awake(){
        MainScript.config = Main_Config;
        MainScript.MainScriptInstance = this;
        MainScript.fixedDeltaTime = 1 / (Fix64)MainScript.config.fps;

        FPRandom.Init();

        // Check which characters have been unlocked
        MainScript.LoadUnlockedCharacters();

        // Check the installed Addons and supported 3rd party products
        MainScript.isCInputInstalled = MainScript.IsInstalled("cInput");
#if UFE_LITE
        MainScript.isAiAddonInstalled = false;
#else
        MainScript.isAiAddonInstalled = MainScript.IsInstalled("RuleBasedAI");
#endif

#if UFE_LITE || UFE_BASIC
		MainScript.isNetworkAddonInstalled = false;
		MainScript.isPhotonInstalled = false;
        MainScript.isBluetoothAddonInstalled = false;
#else
        MainScript.isNetworkAddonInstalled = MainScript.IsInstalled("UnetHighLevelMultiplayerAPI") && MainScript.config.networkOptions.networkService != NetworkService.Disabled;
        MainScript.isPhotonInstalled = MainScript.IsInstalled("PhotonMultiplayerAPI") && MainScript.config.networkOptions.networkService != NetworkService.Disabled;
        MainScript.isBluetoothAddonInstalled = MainScript.IsInstalled("BluetoothMultiplayerAPI") && MainScript.config.networkOptions.networkService != NetworkService.Disabled;
#endif

        MainScript.isControlFreak1Installed = MainScript.IsInstalled("TouchController");				// [DGT]
        MainScript.isControlFreak2Installed = MainScript.IsInstalled("ControlFreak2.UFEBridge");
        MainScript.isControlFreakInstalled = MainScript.isControlFreak1Installed || MainScript.isControlFreak2Installed;
        MainScript.isRewiredInstalled = MainScript.IsInstalled("Rewired.Integration.UniversalFightingEngine.RewiredUFEInputManager");

        // Check if we should run the application in background
        Application.runInBackground = MainScript.config.runInBackground;

        // Check if cInput is installed and initialize the cInput GUI
		if (MainScript.isCInputInstalled){
			Type t = MainScript.SearchClass("cGUI");
			if (t != null) t.GetField("cSkin").SetValue(null, MainScript.config.inputOptions.cInputSkin);
		}

        //-------------------------------------------------------------------------------------------------------------
        // Initialize the GUI
        //-------------------------------------------------------------------------------------------------------------
        GameObject goGroup = new GameObject("CanvasGroup");
        MainScript.canvasGroup = goGroup.AddComponent<CanvasGroup>();


        GameObject go = new GameObject("Canvas");
        go.transform.SetParent(goGroup.transform);

        MainScript.canvas = go.AddComponent<Canvas>();
        MainScript.canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        if(EventSystem.current != null) {
            // Use the current event system if one exists
            MainScript.eventSystem = EventSystem.current;
        } else {
            MainScript.eventSystem = go.AddComponent<EventSystem>();
        }
        //MainScript.eventSystem = GameObject.FindObjectOfType<EventSystem>();
        //if (MainScript.eventSystem == null) MainScript.eventSystem = go.AddComponent<EventSystem>();

        MainScript.graphicRaycaster = go.AddComponent<GraphicRaycaster>();

        MainScript.standaloneInputModule = go.AddComponent<StandaloneInputModule>();
        MainScript.standaloneInputModule.verticalAxis = "Mouse Wheel";
        MainScript.standaloneInputModule.horizontalAxis = "Mouse Wheel";
        MainScript.standaloneInputModule.forceModuleActive = true;

        if (MainScript.config.gameGUI.useCanvasScaler) {
            CanvasScaler cs = go.AddComponent<CanvasScaler>();
            cs.defaultSpriteDPI = MainScript.config.gameGUI.canvasScaler.defaultSpriteDPI;
            cs.fallbackScreenDPI = MainScript.config.gameGUI.canvasScaler.fallbackScreenDPI;
            cs.matchWidthOrHeight = MainScript.config.gameGUI.canvasScaler.matchWidthOrHeight;
            cs.physicalUnit = MainScript.config.gameGUI.canvasScaler.physicalUnit;
            cs.referencePixelsPerUnit = MainScript.config.gameGUI.canvasScaler.referencePixelsPerUnit;
            cs.referenceResolution = MainScript.config.gameGUI.canvasScaler.referenceResolution;
            cs.scaleFactor = MainScript.config.gameGUI.canvasScaler.scaleFactor;
            cs.screenMatchMode = MainScript.config.gameGUI.canvasScaler.screenMatchMode;
            cs.uiScaleMode = MainScript.config.gameGUI.canvasScaler.scaleMode;
            
            //Line commented because we use "Screen Space - Overlay" canvas and the "dynaicPixelsPerUnit" property is only used in "World Space" Canvas.
            //cs.dynamicPixelsPerUnit = MainScript.config.gameGUI.canvasScaler.dynamicPixelsPerUnit; 
        }

        // Check if "Control Freak Virtual Controller" is installed and instantiate the prefab
        if (MainScript.isControlFreakInstalled && MainScript.config.inputOptions.inputManagerType == InputManagerType.ControlFreak)
        {
            if (MainScript.isControlFreak2Installed && (MainScript.config.inputOptions.controlFreak2Prefab != null)) {
                // Try to instantiate Control Freak 2 rig prefab...
                MainScript.controlFreakPrefab = (GameObject)Instantiate(MainScript.config.inputOptions.controlFreak2Prefab.gameObject);
                MainScript.touchControllerBridge = (MainScript.controlFreakPrefab != null) ? MainScript.controlFreakPrefab.GetComponent<InputTouchControllerBridge>() : null;
                MainScript.touchControllerBridge.Init();

            }
            else if (MainScript.isControlFreak1Installed && (MainScript.config.inputOptions.controlFreakPrefab != null)) {
                // ...or try to instantiate Control Freak 1.x controller prefab...
                MainScript.controlFreakPrefab = (GameObject)Instantiate(MainScript.config.inputOptions.controlFreakPrefab);
            }
        }

		// Check if the "network addon" is installed
		string uuid = (MainScript.config.gameName ?? "UFE") /*+ "_" + Application.version*/;
        if (MainScript.isNetworkAddonInstalled)
        {
            GameObject networkManager = new GameObject("Network Manager");
            networkManager.transform.SetParent(this.gameObject.transform);

            MainScript.lanMultiplayerAPI = networkManager.AddComponent(MainScript.SearchClass("UnetLanMultiplayerAPI")) as MultiplayerAPI;
			MainScript.lanMultiplayerAPI.Initialize(uuid);

			if (MainScript.config.networkOptions.networkService == NetworkService.Unity) {
				MainScript.onlineMultiplayerAPI = networkManager.AddComponent(MainScript.SearchClass("UnetOnlineMultiplayerAPI")) as MultiplayerAPI;
			} else if (MainScript.config.networkOptions.networkService == NetworkService.Photon && MainScript.isPhotonInstalled) {
				MainScript.onlineMultiplayerAPI = networkManager.AddComponent(MainScript.SearchClass("PhotonMultiplayerAPI")) as MultiplayerAPI;
			}else if (MainScript.config.networkOptions.networkService == NetworkService.Photon && !MainScript.isPhotonInstalled){
                Debug.LogError("You need 'Photon Unity Networking' installed in order to use Photon as a Network Service.");
            }
			MainScript.onlineMultiplayerAPI.Initialize(uuid);

            if (Application.platform == RuntimePlatform.Android && MainScript.isBluetoothAddonInstalled) {
                MainScript.bluetoothMultiplayerAPI = networkManager.AddComponent(MainScript.SearchClass("BluetoothMultiplayerAPI")) as MultiplayerAPI;
            } else {
                MainScript.bluetoothMultiplayerAPI = networkManager.AddComponent<NullMultiplayerAPI>();
            }
            MainScript.bluetoothMultiplayerAPI.Initialize(uuid);
			

			MainScript.multiplayerAPI.SendRate = 1 / (float)MainScript.config.fps;

			MainScript.localPlayerController = gameObject.AddComponent<CombatController> ();
			MainScript.remotePlayerController = gameObject.AddComponent<DummyInputController>();

			MainScript.localPlayerController.isCPU = false;
			MainScript.remotePlayerController.isCPU = false;

            // TODO deprecated
            //NetworkView network = this.gameObject.AddComponent<NetworkView>();
            //network.stateSynchronization = NetworkStateSynchronization.Off;
            //network.observed = MainScript.remotePlayerController;
		}else{
			MainScript.lanMultiplayerAPI = this.gameObject.AddComponent<NullMultiplayerAPI>();
			MainScript.lanMultiplayerAPI.Initialize(uuid);

			MainScript.onlineMultiplayerAPI = this.gameObject.AddComponent<NullMultiplayerAPI>();
			MainScript.onlineMultiplayerAPI.Initialize(uuid);
			
			MainScript.bluetoothMultiplayerAPI = this.gameObject.AddComponent<NullMultiplayerAPI>();
			MainScript.bluetoothMultiplayerAPI.Initialize(uuid);
		}

		MainScript.fluxCapacitor = new FluxCapacitor(MainScript.currentFrame, MainScript.config.networkOptions.maxBufferSize);
		MainScript._multiplayerMode = MultiplayerMode.Lan;


		// Initialize the input systems
        p1Controller = gameObject.AddComponent<CombatController>();
        if (MainScript.config.inputOptions.inputManagerType == InputManagerType.ControlFreak) {
            p1Controller.humanController = gameObject.AddComponent<InputTouchController>();
        } else if (MainScript.config.inputOptions.inputManagerType == InputManagerType.Rewired) {
            p1Controller.humanController = gameObject.AddComponent<RewiredInputController>();
            (p1Controller.humanController as RewiredInputController).rewiredPlayerId = 0;
        } else {
			p1Controller.humanController = gameObject.AddComponent<InputController>();
		}

        // Initialize AI
        p1SimpleAI = gameObject.AddComponent<SimpleAI>();
		p1SimpleAI.player = 1;

		p1RandomAI = gameObject.AddComponent<RandomAI>();
		p1RandomAI.player = 1;

		p1FuzzyAI = null;
		if (MainScript.isAiAddonInstalled && MainScript.config.aiOptions.engine == AIEngine.FuzzyAI){
			p1FuzzyAI = gameObject.AddComponent(MainScript.SearchClass("RuleBasedAI")) as AbstractInputController;
			p1FuzzyAI.player = 1;
			p1Controller.cpuController = p1FuzzyAI;
		}else{
			p1Controller.cpuController = p1RandomAI;
		}

		p1Controller.isCPU = config.p1CPUControl;
		p1Controller.player = 1;

		p2Controller = gameObject.AddComponent<CombatController> ();
		p2Controller.humanController = gameObject.AddComponent<InputController>();

		p2SimpleAI = gameObject.AddComponent<SimpleAI>();
		p2SimpleAI.player = 2;

		p2RandomAI = gameObject.AddComponent<RandomAI>();
		p2RandomAI.player = 2;

		p2FuzzyAI = null;
		if (MainScript.isAiAddonInstalled && MainScript.config.aiOptions.engine == AIEngine.FuzzyAI){
			p2FuzzyAI = gameObject.AddComponent(MainScript.SearchClass("RuleBasedAI")) as AbstractInputController;
			p2FuzzyAI.player = 2;
			p2Controller.cpuController = p2FuzzyAI;
		}else{
			p2Controller.cpuController = p2RandomAI;
		}

		p2Controller.isCPU = config.p2CPUControl;
		p2Controller.player = 2;


		p1Controller.Initialize(config.player1_Inputs);
		p2Controller.Initialize(config.player2_Inputs);

		if (config.fps > 0) {
            MainScript.timeScale = MainScript.config._gameSpeed;
			Application.targetFrameRate = config.fps;
		}

        SetLanguage();
        MainScript.InitializeAudioSystem();
        MainScript.SetAIDifficulty(MainScript.config.aiOptions.selectedDifficultyLevel);
        MainScript.SetDebugMode(config.debugOptions.debugMode);

        // Load the player settings from disk
        MainScript.SetMusic(PlayerPrefs.GetInt(MainScript.MusicEnabledKey, 1) > 0);
		MainScript.SetMusicVolume(PlayerPrefs.GetFloat(MainScript.MusicVolumeKey, 1f));
		MainScript.SetSoundFX(PlayerPrefs.GetInt(MainScript.SoundsEnabledKey, 1) > 0);
		MainScript.SetSoundFXVolume(PlayerPrefs.GetFloat(MainScript.SoundsVolumeKey, 1f));
        
        // Load the intro screen or the combat, depending on the UFE Config settings
        if (MainScript.config.debugOptions.startGameImmediately){
            if (MainScript.config.debugOptions.matchType == MatchType.Training) {
                MainScript.gameMode = GameMode.TrainingRoom;
            } else if (MainScript.config.debugOptions.matchType == MatchType.Challenge) {
                MainScript.gameMode = GameMode.ChallengeMode;
            } else {
                MainScript.gameMode = GameMode.VersusMode;
            }
			MainScript.config.player1Character = config.p1CharStorage;
			MainScript.config.player2Character = config.p2CharStorage;
			MainScript.SetCPU(1, config.p1CPUControl);
			MainScript.SetCPU(2, config.p2CPUControl);

            if (MainScript.config.debugOptions.skipLoadingScreen)
            {
                MainScript._StartGame((float)MainScript.config.gameGUI.gameFadeDuration);
            }
            else
            {
                MainScript._StartLoadingBattleScreen((float)MainScript.config.gameGUI.screenFadeDuration);
            }
		}else{
			MainScript.StartIntroScreen(0f);
        }
    }

	protected void Update(){
		MainScript.GetPlayer1Controller().DoUpdate();
		MainScript.GetPlayer2Controller().DoUpdate();

#if UNITY_EDITOR
        // Save and Load State
        if (MainScript.fluxCapacitor != null && MainScript.config.debugOptions.stateTrackerTest) {
			if (Input.GetKeyDown(KeyCode.F2)) { // Save State
				Debug.Log("Save (" + MainScript.currentFrame + ")");
                MainScript.fluxCapacitor.savedState = FluxStateTracker.SaveGameState(MainScript.currentFrame);

                //dictionaryList.Add(RecordVar.SaveStateTrackers(this, new Dictionary<MemberInfo, object>()));
                //testRecording = !testRecording;
            }
			if (MainScript.fluxCapacitor.savedState != null && Input.GetKeyDown(KeyCode.F3)) { // Load State
				Debug.Log("Load (" + MainScript.fluxCapacitor.savedState.Value.networkFrame + ")");
                FluxStateTracker.LoadGameState(MainScript.fluxCapacitor.savedState.Value);
                MainScript.fluxCapacitor.PlayerManager.Initialize(MainScript.fluxCapacitor.savedState.Value.networkFrame);

                //UFE ufeInstance = this;
                //ufeInstance = RecordVar.LoadStateTrackers(ufeInstance, dictionaryList[dictionaryList.Count - 1]) as UFE;
                //p1ControlsScript.MoveSet.MecanimControl.Refresh();
                //p2ControlsScript.MoveSet.MecanimControl.Refresh();
            }
        }
#endif
    }

#if UNITY_EDITOR
    private void OnGUI() {
        if (MainScript.config.debugOptions.stateTrackerTest && MainScript.gameRunning)
        {
            if (GUI.Button(new Rect(10, 10, 160, 40), "Save State"))
            {
                Debug.Log("Save (" + MainScript.currentFrame + ")");
                MainScript.fluxCapacitor.savedState = FluxStateTracker.SaveGameState(MainScript.currentFrame);

                //Debug.Log(MainScript.GetPlayer1ControlsScript().MoveSet.GetCurrentClipFrame());
            }

            if (GUI.Button(new Rect(10, 60, 160, 40), "Load State"))
            {
                Debug.Log("Load (" + MainScript.fluxCapacitor.savedState.Value.networkFrame + ")");
                FluxStateTracker.LoadGameState(MainScript.fluxCapacitor.savedState.Value);
                MainScript.fluxCapacitor.PlayerManager.Initialize(MainScript.fluxCapacitor.savedState.Value.networkFrame);

                //Debug.Log(MainScript.GetPlayer1ControlsScript().MoveSet.GetCurrentClipFrame());
            }
        }
    }
#endif

    //public List<Dictionary<System.Reflection.MemberInfo, System.Object>> dictionaryList = new List<Dictionary<System.Reflection.MemberInfo, System.Object>>();
    //public bool testRecording = false;

    protected void FixedUpdate(){
		if (MainScript.fluxCapacitor != null){
			MainScript.fluxCapacitor.DoFixedUpdate();

            /*if (testRecording)
            {
                dictionaryList.Add(RecordVar.SaveStateTrackers(this, new Dictionary<MemberInfo, object>()));
                if (dictionaryList.Count > 30) dictionaryList.RemoveAt(0);
            }*/
        }
	}
    
	protected void OnApplicationQuit(){
		MainScript.closing = true;
		MainScript.EnsureNetworkDisconnection();
	}
#endregion

#region protected instance methods: Network Events
	public static bool isConnected{
		get{
			return MainScript.multiplayerAPI.IsConnected() && MainScript.multiplayerAPI.Connections > 0;
		}
	}

	public static void EnsureNetworkDisconnection(){
		if (!MainScript.disconnecting){
			NetworkState state = MainScript.multiplayerAPI.GetConnectionState();

			if (state == NetworkState.Client){
				MainScript.RemoveNetworkEventListeners();
				MainScript.multiplayerAPI.DisconnectFromMatch();
			}else if (state == NetworkState.Server){
				MainScript.RemoveNetworkEventListeners();
				MainScript.multiplayerAPI.DestroyMatch();
			}
		}
    }

	protected static void AddNetworkEventListeners(){
		MainScript.multiplayerAPI.OnDisconnection -= MainScript.OnDisconnectedFromServer;
		MainScript.multiplayerAPI.OnJoined -= MainScript.OnJoined;
		MainScript.multiplayerAPI.OnJoinError -= MainScript.OnJoinError;
		MainScript.multiplayerAPI.OnPlayerConnectedToMatch -= MainScript.OnPlayerConnectedToMatch;
		MainScript.multiplayerAPI.OnPlayerDisconnectedFromMatch -= MainScript.OnPlayerDisconnectedFromMatch;
		MainScript.multiplayerAPI.OnMatchesDiscovered -= MainScript.OnMatchesDiscovered;
		MainScript.multiplayerAPI.OnMatchDiscoveryError -= MainScript.OnMatchDiscoveryError;
		MainScript.multiplayerAPI.OnMatchCreated -= MainScript.OnMatchCreated;
		MainScript.multiplayerAPI.OnMatchDestroyed -= MainScript.OnMatchDestroyed;

		MainScript.multiplayerAPI.OnDisconnection += MainScript.OnDisconnectedFromServer;
		MainScript.multiplayerAPI.OnJoined += MainScript.OnJoined;
		MainScript.multiplayerAPI.OnJoinError += MainScript.OnJoinError;
		MainScript.multiplayerAPI.OnPlayerConnectedToMatch += MainScript.OnPlayerConnectedToMatch;
		MainScript.multiplayerAPI.OnPlayerDisconnectedFromMatch += MainScript.OnPlayerDisconnectedFromMatch;
		MainScript.multiplayerAPI.OnMatchesDiscovered += MainScript.OnMatchesDiscovered;
		MainScript.multiplayerAPI.OnMatchDiscoveryError += MainScript.OnMatchDiscoveryError;
		MainScript.multiplayerAPI.OnMatchCreated += MainScript.OnMatchCreated;
		MainScript.multiplayerAPI.OnMatchDestroyed += MainScript.OnMatchDestroyed;
	}

	protected static void RemoveNetworkEventListeners(){
		MainScript.multiplayerAPI.OnDisconnection -= MainScript.OnDisconnectedFromServer;
		MainScript.multiplayerAPI.OnJoined -= MainScript.OnJoined;
		MainScript.multiplayerAPI.OnJoinError -= MainScript.OnJoinError;
		MainScript.multiplayerAPI.OnPlayerConnectedToMatch -= MainScript.OnPlayerConnectedToMatch;
		MainScript.multiplayerAPI.OnPlayerDisconnectedFromMatch -= MainScript.OnPlayerDisconnectedFromMatch;
		MainScript.multiplayerAPI.OnMatchesDiscovered -= MainScript.OnMatchesDiscovered;
		MainScript.multiplayerAPI.OnMatchDiscoveryError -= MainScript.OnMatchDiscoveryError;
		MainScript.multiplayerAPI.OnMatchCreated -= MainScript.OnMatchCreated;
		MainScript.multiplayerAPI.OnMatchDestroyed -= MainScript.OnMatchDestroyed;
	}

	protected static void OnJoined(MultiplayerAPI.JoinedMatchInformation match){
		if (MainScript.config.debugOptions.connectionLog) Debug.Log("Connected to server");
		MainScript.StartNetworkGame(0.5f, 2, false);
	}

	protected static void OnDisconnectedFromServer() {
        if (MainScript.config.debugOptions.connectionLog) Debug.Log("Disconnected from server");
		MainScript.fluxCapacitor.Initialize(); // Return to single player controls

		if (!MainScript.closing){
			MainScript.disconnecting = true;
			Application.runInBackground = MainScript.config.runInBackground;

			if (MainScript.config.lockInputs && MainScript.currentScreen == null){
				MainScript.DelayLocalAction(MainScript.StartConnectionLostScreenIfMainMenuNotLoaded, 1f);
			}else{
				MainScript.StartConnectionLostScreen();
			}
		}
	}

	protected static void OnJoinError() {
        if (MainScript.config.debugOptions.connectionLog) Debug.Log("Could not connect to server");
		Application.runInBackground = MainScript.config.runInBackground;
		MainScript.StartConnectionLostScreen();
	}

	protected static void OnMatchCreated(MultiplayerAPI.CreatedMatchInformation match){}

	protected static void OnMatchDestroyed(){}

	protected static void OnMatchJoined(JoinMatchResponse response){}

	protected static void OnMatchDropped(){}

	protected static void OnPlayerConnectedToMatch(MultiplayerAPI.PlayerInformation player) {
		if (MainScript.config.debugOptions.connectionLog){
			if (player.networkIdentity != null){
				Debug.Log("Connection: " + player.networkIdentity.connectionToClient);
			}else{
				Debug.Log("Player connected: " + player.photonPlayer);
			}
		}

		MainScript.StartNetworkGame(0.5f, 1, false);
	}

	protected static void OnPlayerDisconnectedFromMatch(MultiplayerAPI.PlayerInformation player) {
        if (MainScript.config.debugOptions.connectionLog) Debug.Log("Clean up after player " + player);
		MainScript.fluxCapacitor.Initialize(); // Return to single player controls

		if (!MainScript.closing){
			MainScript.disconnecting = true;
			Application.runInBackground = MainScript.config.runInBackground;

			if (MainScript.config.lockInputs && MainScript.currentScreen == null){
				MainScript.DelayLocalAction(MainScript.StartConnectionLostScreenIfMainMenuNotLoaded, 1f);
			}else{
				MainScript.StartConnectionLostScreen();
			}
		}
	}

	protected static void OnServerInitialized() {
        if (MainScript.config.debugOptions.connectionLog) Debug.Log("Server initialized and ready");
		Application.runInBackground = true;
		MainScript.disconnecting = false;
	}
#endregion

#region private class methods: GUI Related methods
    public static Text DebuggerText(string dName, string dText, Vector2 position, TextAnchor alignment)
    {
        GameObject debugger = new GameObject(dName);
        debugger.transform.SetParent(MainScript.canvas.transform);

        RectTransform trans = debugger.AddComponent<RectTransform>();
        trans.anchoredPosition = position;

        Text debuggerText = debugger.AddComponent<Text>();
        debuggerText.text = dText;
        debuggerText.alignment = alignment;
        debuggerText.color = Color.black;
        debuggerText.fontStyle = FontStyle.Bold;

        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        debuggerText.font = ArialFont;
        debuggerText.fontSize = 24;
        debuggerText.verticalOverflow = VerticalWrapMode.Overflow;
        debuggerText.horizontalOverflow = HorizontalWrapMode.Overflow;
        debuggerText.material = ArialFont.material;

        //Outline debuggerTextOutline = debugger.AddComponent<Outline>();
        //debuggerTextOutline.effectColor = Color.white;

        return debuggerText;
    }

    public static void GoToNetworkGameScreen(){
		if (MainScript.multiplayerMode == MultiplayerMode.Bluetooth){
			MainScript.StartBluetoothGameScreen();
		}else{
			MainScript.StartNetworkGameScreen();
		}
	}

	public static void GoToNetworkGameScreen(float fadeTime){
		if (MainScript.multiplayerMode == MultiplayerMode.Bluetooth){
			MainScript.StartBluetoothGameScreen(fadeTime);
		}else{
			MainScript.StartNetworkGameScreen(fadeTime);
		}
    }

	private static void _StartBluetoothGameScreen(float fadeTime){
		MainScript.EnsureNetworkDisconnection();

		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.bluetoothGameScreen == null){
			Debug.LogError("Bluetooth Game Screen not found! Make sure you have set the prefab correctly in the Global Editor");
		}else if (MainScript.isNetworkAddonInstalled){
            MainScript.ShowScreen(MainScript.config.gameGUI.bluetoothGameScreen);
            if (!MainScript.config.gameGUI.bluetoothGameScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}else{
			Debug.LogWarning("Network Addon not found!");
		}
	}

	private static void _StartCharacterSelectionScreen(float fadeTime){
		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.characterSelectionScreen == null){
			Debug.LogError("Character Selection Screen not found! Make sure you have set the prefab correctly in the Global Editor");
		}else{
            MainScript.ShowScreen(MainScript.config.gameGUI.characterSelectionScreen);
            if (!MainScript.config.gameGUI.characterSelectionScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}

	private static void _StartIntroScreen(float fadeTime){
		MainScript.EnsureNetworkDisconnection();

		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.introScreen == null){
			//Debug.Log("Intro Screen not found! Make sure you have set the prefab correctly in the Global Editor");
			MainScript._StartMainMenuScreen(fadeTime);
		}else{
            MainScript.ShowScreen(MainScript.config.gameGUI.introScreen);
            if (!MainScript.config.gameGUI.introScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
        }
	}

	private static void _StartMainMenuScreen(float fadeTime){
		MainScript.EnsureNetworkDisconnection();

		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.mainMenuScreen == null){
			Debug.LogError("Main Menu Screen not found! Make sure you have set the prefab correctly in the Global Editor");
		}else{
            MainScript.ShowScreen(MainScript.config.gameGUI.mainMenuScreen);
            if (!MainScript.config.gameGUI.mainMenuScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}

	private static void _StartStageSelectionScreen(float fadeTime){
		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.stageSelectionScreen == null){
			Debug.LogError("Stage Selection Screen not found! Make sure you have set the prefab correctly in the Global Editor");
		}else{
            MainScript.ShowScreen(MainScript.config.gameGUI.stageSelectionScreen);
            if (!MainScript.config.gameGUI.stageSelectionScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}
	
	private static void _StartCreditsScreen(float fadeTime){
		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.creditsScreen == null){
			Debug.Log("Credits screen not found! Make sure you have set the prefab correctly in the Global Editor");
		}else{
            MainScript.ShowScreen(MainScript.config.gameGUI.creditsScreen);
            if (!MainScript.config.gameGUI.creditsScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}

	private static void _StartConnectionLostScreen(float fadeTime){
		MainScript.EnsureNetworkDisconnection();

		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.connectionLostScreen == null){
			Debug.LogError("Connection Lost Screen not found! Make sure you have set the prefab correctly in the Global Editor");
			MainScript._StartMainMenuScreen(fadeTime);
		}else if (MainScript.isNetworkAddonInstalled){
            MainScript.ShowScreen(MainScript.config.gameGUI.connectionLostScreen);
            if (!MainScript.config.gameGUI.connectionLostScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}else{
			Debug.LogWarning("Network Addon not found!");
			MainScript._StartMainMenuScreen(fadeTime);
		}
	}

	private static void _StartGame(float fadeTime){
		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.battleGUI == null){
			Debug.LogError("Battle GUI not found! Make sure you have set the prefab correctly in the Global Editor");
			MainScript.battleGUI = new GameObject("BattleGUI").AddComponent<CombatScreen>();
		}else{
			MainScript.battleGUI = (CombatScreen) GameObject.Instantiate(MainScript.config.gameGUI.battleGUI);
        }
        if (!MainScript.battleGUI.hasFadeIn) fadeTime = 0;
        CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);

		MainScript.battleGUI.transform.SetParent(MainScript.canvas != null ? MainScript.canvas.transform : null, false);
        MainScript.battleGUI.OnShow();
        MainScript.canvasGroup.alpha = 0;
		
		MainScript.gameEngine = new GameObject("Game");
		MainScript.cameraScript = MainScript.gameEngine.AddComponent<CameraScript>();

		if (MainScript.config.player1Character == null){
			Debug.LogError("No character selected for player 1.");
			return;
		}
		if (MainScript.config.player2Character == null){
			Debug.LogError("No character selected for player 2.");
			return;
		}
		if (MainScript.config.selectedStage == null){
			Debug.LogError("No stage selected.");
			return;
		}
		
		if (MainScript.config.aiOptions.engine == AIEngine.FuzzyAI){
			MainScript.SetFuzzyAI(1, MainScript.config.player1Character);
            MainScript.SetFuzzyAI(2, MainScript.config.player2Character);
        } else {
            MainScript.SetRandomAI(1);
            MainScript.SetRandomAI(2);
        }
		
		MainScript.config.player1Character.currentLifePoints = (Fix64)MainScript.config.player1Character.lifePoints;
		MainScript.config.player2Character.currentLifePoints = (Fix64)MainScript.config.player2Character.lifePoints;
		MainScript.config.player1Character.currentGaugePoints = 0;
		MainScript.config.player2Character.currentGaugePoints = 0;

        GameObject stageInstance = null;
        if (MainScript.config.stagePrefabStorage == StorageMode.Legacy) {
            if (MainScript.config.selectedStage.prefab != null) {
                stageInstance = (GameObject)Instantiate(config.selectedStage.prefab);
                stageInstance.transform.parent = gameEngine.transform;
            } else {
                Debug.LogError("Stage prefab not found! Make sure you have set the prefab correctly in the Global Editor.");
            }
        } else {
            GameObject prefab = Resources.Load<GameObject>(config.selectedStage.stageResourcePath);

            if (prefab != null) {
                stageInstance = (GameObject)GameObject.Instantiate(prefab);
                stageInstance.transform.parent = gameEngine.transform;
            } else {
                Debug.LogError("Stage prefab not found! Make sure the prefab is correctly located under the Resources folder and the path is written correctly.");
            }
        }


        MainScript.config.currentRound = 1;
		MainScript.config.lockInputs = true;
		MainScript.SetTimer(config.roundOptions._timer);
		MainScript.PauseTimer();

        // Initialize Player 1 Character
		GameObject p1 = new GameObject("Player1");
		p1.transform.parent = gameEngine.transform;
        MainScript.p1ControlsScript = p1.AddComponent<ControlsScript>();
        MainScript.p1ControlsScript.Physics = p1.AddComponent<PhysicsScript>();
        MainScript.p1ControlsScript.myInfo = (CharacterInfo)Instantiate(MainScript.config.player1Character);

        MainScript.config.player1Character = MainScript.p1ControlsScript.myInfo;
        MainScript.p1ControlsScript.myInfo.playerNum = 1;
        if (MainScript.isControlFreak2Installed && MainScript.p1ControlsScript.myInfo.customControls.overrideControlFreak && MainScript.p1ControlsScript.myInfo.customControls.controlFreak2Prefab != null) {
            MainScript.controlFreakPrefab = (GameObject)Instantiate(MainScript.p1ControlsScript.myInfo.customControls.controlFreak2Prefab.gameObject);
            MainScript.touchControllerBridge = (MainScript.controlFreakPrefab != null) ? MainScript.controlFreakPrefab.GetComponent<InputTouchControllerBridge>() : null;
            MainScript.touchControllerBridge.Init();
        }

        // Initialize Player 2 Character
		GameObject p2 = new GameObject("Player2");
		p2.transform.parent = gameEngine.transform;
        MainScript.p2ControlsScript = p2.AddComponent<ControlsScript>();
        MainScript.p2ControlsScript.Physics = p2.AddComponent<PhysicsScript>();
        MainScript.p2ControlsScript.myInfo = (CharacterInfo)Instantiate(MainScript.config.player2Character);
        MainScript.config.player2Character = MainScript.p2ControlsScript.myInfo;
        MainScript.p2ControlsScript.myInfo.playerNum = 2;


        // If the same character is selected, try loading the alt costume
        if (MainScript.config.player1Character.name == MainScript.config.player2Character.name) {
            if (MainScript.config.player2Character.alternativeCostumes.Length > 0) {
                MainScript.config.player2Character.isAlt = true;
                MainScript.config.player2Character.selectedCostume = 0;
                
                if (MainScript.config.player2Character.alternativeCostumes[0].characterPrefabStorage == StorageMode.Legacy) {
                    MainScript.p2ControlsScript.myInfo.characterPrefab = MainScript.config.player2Character.alternativeCostumes[0].prefab;
                } else {
                    MainScript.p2ControlsScript.myInfo.characterPrefab = Resources.Load<GameObject>(MainScript.config.player2Character.alternativeCostumes[0].prefabResourcePath);
                }
            }
        }

        // Initialize Debuggers
        MainScript.debugger1 = MainScript.DebuggerText("Debugger1", "", new Vector2(- Screen.width + 50, Screen.height - 180), TextAnchor.UpperLeft);
        MainScript.debugger2 = MainScript.DebuggerText("Debugger2", "", new Vector2(Screen.width - 50, Screen.height - 180), TextAnchor.UpperRight);
        MainScript.p1ControlsScript.debugger = MainScript.debugger1;
        MainScript.p2ControlsScript.debugger = MainScript.debugger2;
        MainScript.debugger1.enabled = MainScript.debugger2.enabled = config.debugOptions.debugMode;


        //MainScript.fluxGameManager.Initialize(MainScript.currentFrame);
        MainScript.fluxCapacitor.savedState = null;
        MainScript.PauseGame(false);
    }

    //Preloader
    public static void PreloadBattle() {
        PreloadBattle((float)MainScript.config._preloadingTime);
    }

    public static void PreloadBattle(float warmTimer) {
        if (MainScript.config.preloadHitEffects) {
            SearchAndCastGameObject(MainScript.config.hitOptions, warmTimer);
            SearchAndCastGameObject(MainScript.config.groundBounceOptions, warmTimer);
            SearchAndCastGameObject(MainScript.config.wallBounceOptions, warmTimer);
            if (MainScript.config.debugOptions.preloadedObjects) Debug.Log("Hit Effects Loaded");
        }
        if (MainScript.config.preloadStage) {
            SearchAndCastGameObject(MainScript.config.selectedStage, warmTimer);
            if (MainScript.config.debugOptions.preloadedObjects) Debug.Log("Stage Loaded");
        }
        if (MainScript.config.preloadCharacter1) {
            SearchAndCastGameObject(MainScript.config.player1Character, warmTimer);
            if (MainScript.config.debugOptions.preloadedObjects) Debug.Log("Character 1 Loaded");
        }
        if (MainScript.config.preloadCharacter2) {
            SearchAndCastGameObject(MainScript.config.player2Character, warmTimer);
            if (MainScript.config.debugOptions.preloadedObjects) Debug.Log("Character 2 Loaded");
        }
        if (MainScript.config.warmAllShaders) Shader.WarmupAllShaders();

        memoryDump.Clear();
    }

    public static void SearchAndCastGameObject(object target, float warmTimer) {
        if (target != null) {
            Type typeSource = target.GetType();
            FieldInfo[] fields = typeSource.GetFields();

            foreach (FieldInfo field in fields) {
                object fieldValue = field.GetValue(target);
                if (fieldValue == null || fieldValue.Equals(null)) continue;
                if (memoryDump.Contains(fieldValue)) continue;
                memoryDump.Add(fieldValue);

                if (field.FieldType.Equals(typeof(GameObject))) {
                    if (MainScript.config.debugOptions.preloadedObjects) Debug.Log(fieldValue + " preloaded");
                    GameObject tempGO = (GameObject)Instantiate((GameObject)fieldValue);
                    tempGO.transform.position = new Vector2(-999, -999);

                    //Light lightComponent = tempGO.GetComponent<Light>();
                    //if (lightComponent != null) lightComponent.enabled = false;

                    Destroy(tempGO, warmTimer);

                } else if (field.FieldType.IsArray && !field.FieldType.GetElementType().IsEnum) {
                    object[] fieldValueArray = (object[])fieldValue;
                    foreach (object obj in fieldValueArray) {
                        SearchAndCastGameObject(obj, warmTimer);
                    }
                }
            }
        }
    }

	private static void _StartHostGameScreen(float fadeTime){
		MainScript.EnsureNetworkDisconnection();

		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.hostGameScreen == null){
			Debug.LogError("Host Game Screen not found! Make sure you have set the prefab correctly in the Global Editor");
			MainScript._StartMainMenuScreen(fadeTime);
		}else if (MainScript.isNetworkAddonInstalled){
            MainScript.ShowScreen(MainScript.config.gameGUI.hostGameScreen);
            if (!MainScript.config.gameGUI.hostGameScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}else{
			Debug.LogWarning("Network Addon not found!");
			MainScript._StartMainMenuScreen(fadeTime);
		}
	}

	private static void _StartJoinGameScreen(float fadeTime){
		MainScript.EnsureNetworkDisconnection();

		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.joinGameScreen == null){
			Debug.LogError("Join To Game Screen not found! Make sure you have set the prefab correctly in the Global Editor");
			MainScript._StartMainMenuScreen(fadeTime);
		}else if (MainScript.isNetworkAddonInstalled){
            MainScript.ShowScreen(MainScript.config.gameGUI.joinGameScreen);
            if (!MainScript.config.gameGUI.joinGameScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}else{
			Debug.LogWarning("Network Addon not found!");
			MainScript._StartMainMenuScreen(fadeTime);
		}
	}
	
	private static void _StartLoadingBattleScreen(float fadeTime){
        MainScript.config.lockInputs = true;

		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.loadingBattleScreen == null){
			Debug.Log("Loading Battle Screen not found! Make sure you have set the prefab correctly in the Global Editor");
            MainScript._StartGame((float)MainScript.config.gameGUI.gameFadeDuration);
		}else{
            MainScript.ShowScreen(MainScript.config.gameGUI.loadingBattleScreen);
            if (!MainScript.config.gameGUI.loadingBattleScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}

	private static void _StartSearchMatchScreen(float fadeTime){
		//MainScript.EnsureNetworkDisconnection();

		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.searchMatchScreen == null){
			Debug.LogError("Random Match Screen not found! Make sure you have set the prefab correctly in the Global Editor");
			MainScript._StartMainMenuScreen(fadeTime);
		}else if (MainScript.isNetworkAddonInstalled){
			//MainScript.AddNetworkEventListeners();
            MainScript.ShowScreen(MainScript.config.gameGUI.searchMatchScreen);
            if (!MainScript.config.gameGUI.searchMatchScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}else{
			Debug.LogWarning("Network Addon not found!");
			MainScript._StartMainMenuScreen(fadeTime);
		}
	}

	private static void _StartNetworkGameScreen(float fadeTime){
		MainScript.EnsureNetworkDisconnection();

		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.networkGameScreen == null){
			Debug.LogError("Network Game Screen not found! Make sure you have set the prefab correctly in the Global Editor");
			MainScript._StartMainMenuScreen(fadeTime);
		}else if (MainScript.isNetworkAddonInstalled){
            MainScript.ShowScreen(MainScript.config.gameGUI.networkGameScreen);
            if (!MainScript.config.gameGUI.networkGameScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}else{
			Debug.LogWarning("Network Addon not found!");
			MainScript._StartMainMenuScreen(fadeTime);
		}
	}

	private static void _StartOptionsScreen(float fadeTime){

		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.optionsScreen == null){
			Debug.LogError("Options Screen not found! Make sure you have set the prefab correctly in the Global Editor");
		}else{
            MainScript.ShowScreen(MainScript.config.gameGUI.optionsScreen);
            if (!MainScript.config.gameGUI.optionsScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}

	public static void _StartStoryModeBattle(float fadeTime){
		// If the player 1 won the last battle, load the information of the next battle. 
		// Otherwise, repeat the last battle...
		CharacterInfo character = MainScript.GetPlayer(1);

		if (MainScript.player1WonLastBattle){
			// If the player 1 won the last battle...
			if (MainScript.storyMode.currentGroup < 0){
				// If we haven't fought any battle, raise the "Story Mode Started" event...
				if (MainScript.OnStoryModeStarted != null){
					MainScript.OnStoryModeStarted(character);
				}

				// And start with the first battle of the first group
				MainScript.storyMode.currentGroup = 0;
				MainScript.storyMode.currentBattle = 0;
			}else if (MainScript.storyMode.currentGroup >= 0 && MainScript.storyMode.currentGroup < MainScript.storyMode.characterStory.fightsGroups.Length){
				// Otherwise, check if there are more remaining battles in the current group
				FightsGroup currentGroup = MainScript.storyMode.characterStory.fightsGroups[MainScript.storyMode.currentGroup];
				int numberOfFights = currentGroup.maxFights;
				
				if (currentGroup.mode != FightsGroupMode.FightAgainstSeveralOpponentsInTheGroupInRandomOrder){
					numberOfFights = currentGroup.opponents.Length;
				}
				
				if (MainScript.storyMode.currentBattle < numberOfFights - 1){
					// If there are more battles in the current group, go to the next battle...
					++MainScript.storyMode.currentBattle;
				}else{
					// Otherwise, go to the next group of battles...
					++MainScript.storyMode.currentGroup;
					MainScript.storyMode.currentBattle = 0;
					MainScript.storyMode.defeatedOpponents.Clear();
				}
			}

			// If the player hasn't finished the game...
			MainScript.storyMode.currentBattleInformation = null;
			while (
				MainScript.storyMode.currentBattleInformation == null &&
				MainScript.storyMode.currentGroup >= 0 && 
				MainScript.storyMode.currentGroup < MainScript.storyMode.characterStory.fightsGroups.Length
			){
				// Try to retrieve the information of the next battle
				FightsGroup currentGroup = MainScript.storyMode.characterStory.fightsGroups[MainScript.storyMode.currentGroup];
				MainScript.storyMode.currentBattleInformation = null;
				
				if (currentGroup.mode == FightsGroupMode.FightAgainstAllOpponentsInTheGroupInTheDefinedOrder){
					StoryModeBattle b = currentGroup.opponents[MainScript.storyMode.currentBattle];
					CharacterInfo opponent = MainScript.config.characters[b.opponentCharacterIndex];

					if (MainScript.storyMode.canFightAgainstHimself || !character.characterName.Equals(opponent.characterName)){
						MainScript.storyMode.currentBattleInformation = b;
					}else{
						// Otherwise, check if there are more remaining battles in the current group
						int numberOfFights = currentGroup.maxFights;
						
						if (currentGroup.mode != FightsGroupMode.FightAgainstSeveralOpponentsInTheGroupInRandomOrder){
							numberOfFights = currentGroup.opponents.Length;
						}
						
						if (MainScript.storyMode.currentBattle < numberOfFights - 1){
							// If there are more battles in the current group, go to the next battle...
							++MainScript.storyMode.currentBattle;
						}else{
							// Otherwise, go to the next group of battles...
							++MainScript.storyMode.currentGroup;
							MainScript.storyMode.currentBattle = 0;
							MainScript.storyMode.defeatedOpponents.Clear();
						}
					}
				}else{
					List<StoryModeBattle> possibleBattles = new List<StoryModeBattle>();
					
					foreach (StoryModeBattle b in currentGroup.opponents){
						if (!MainScript.storyMode.defeatedOpponents.Contains(b.opponentCharacterIndex)){
							CharacterInfo opponent = MainScript.config.characters[b.opponentCharacterIndex];
							
							if (MainScript.storyMode.canFightAgainstHimself || !character.characterName.Equals(opponent.characterName)){
								possibleBattles.Add(b);
							}
						}
					}
					
					if (possibleBattles.Count > 0){
						int index = UnityEngine.Random.Range(0, possibleBattles.Count);
						MainScript.storyMode.currentBattleInformation = possibleBattles[index];
					}else{
						// If we can't find a valid battle in this group, try moving to the next group
						++MainScript.storyMode.currentGroup;
					}
				}
			}
		}

		if (MainScript.storyMode.currentBattleInformation != null){
			// If we could retrieve the battle information, load the opponent and the stage
			int characterIndex = MainScript.storyMode.currentBattleInformation.opponentCharacterIndex;
			MainScript.SetPlayer2(MainScript.config.characters[characterIndex]);

			if (MainScript.player1WonLastBattle){
				MainScript.lastStageIndex = UnityEngine.Random.Range(0, MainScript.storyMode.currentBattleInformation.possibleStagesIndexes.Count);
			}

			MainScript.SetStage(MainScript.config.stages[MainScript.storyMode.currentBattleInformation.possibleStagesIndexes[MainScript.lastStageIndex]]);
			
			// Finally, check if we should display any "Conversation Screen" before the battle
			MainScript._StartStoryModeConversationBeforeBattleScreen(MainScript.storyMode.currentBattleInformation.conversationBeforeBattle, fadeTime);
		}else{
			// Otherwise, show the "Congratulations" Screen
			if (MainScript.OnStoryModeCompleted != null){
				MainScript.OnStoryModeCompleted(character);
			}

			MainScript._StartStoryModeCongratulationsScreen(fadeTime);
		}
	}

	private static void _StartStoryModeCongratulationsScreen(float fadeTime){
		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.storyModeCongratulationsScreen == null){
			Debug.Log("Congratulations Screen not found! Make sure you have set the prefab correctly in the Global Editor");
            MainScript._StartStoryModeEndingScreen(fadeTime);
		}else{
            MainScript.ShowScreen(MainScript.config.gameGUI.storyModeCongratulationsScreen, delegate() { MainScript.StartStoryModeEndingScreen(fadeTime); });
            if (!MainScript.config.gameGUI.storyModeCongratulationsScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}

	private static void _StartStoryModeContinueScreen(float fadeTime){
		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.storyModeContinueScreen == null){
			Debug.Log("Continue Screen not found! Make sure you have set the prefab correctly in the Global Editor");
			MainScript._StartMainMenuScreen(fadeTime);
		}else{
            MainScript.ShowScreen(MainScript.config.gameGUI.storyModeContinueScreen);
            if (!MainScript.config.gameGUI.storyModeContinueScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}

    private static void _StartStoryModeConversationAfterBattleScreen(CombatScreen conversationScreen, float fadeTime) {
        MainScript.HideScreen(MainScript.currentScreen);
		if (conversationScreen != null){
            MainScript.ShowScreen(conversationScreen, delegate() { MainScript.StartStoryModeBattle(fadeTime); });
            if (!conversationScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}else{
			MainScript._StartStoryModeBattle(fadeTime);
		}
	}

    private static void _StartStoryModeConversationBeforeBattleScreen(CombatScreen conversationScreen, float fadeTime) {
        MainScript.HideScreen(MainScript.currentScreen);
		if (conversationScreen != null){
            MainScript.ShowScreen(conversationScreen, delegate() { MainScript.StartLoadingBattleScreen(fadeTime); });
            if (!conversationScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}else{
			MainScript._StartLoadingBattleScreen(fadeTime);
		}
	}

	private static void _StartStoryModeEndingScreen(float fadeTime){
		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.storyMode.characterStory.ending == null){
			Debug.Log("Ending Screen not found! Make sure you have set the prefab correctly in the Global Editor");
			MainScript._StartCreditsScreen(fadeTime);
		}else{
            MainScript.ShowScreen(MainScript.storyMode.characterStory.ending, delegate() { MainScript.StartCreditsScreen(fadeTime); });
            if (!MainScript.storyMode.characterStory.ending.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}

	private static void _StartStoryModeGameOverScreen(float fadeTime){
		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.storyModeGameOverScreen == null){
			Debug.Log("Game Over Screen not found! Make sure you have set the prefab correctly in the Global Editor");
			MainScript._StartMainMenuScreen(fadeTime);
		}else{
            MainScript.ShowScreen(MainScript.config.gameGUI.storyModeGameOverScreen, delegate() { MainScript.StartMainMenuScreen(fadeTime); });
            if (!MainScript.config.gameGUI.storyModeGameOverScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}

	private static void _StartStoryModeOpeningScreen(float fadeTime){
		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.storyMode.characterStory.opening == null){
			Debug.Log("Opening Screen not found! Make sure you have set the prefab correctly in the Global Editor");
			MainScript._StartStoryModeBattle(fadeTime);
		}else{
            MainScript.ShowScreen(MainScript.storyMode.characterStory.opening, delegate() { MainScript.StartStoryModeBattle(fadeTime); });
            if (!MainScript.storyMode.characterStory.opening.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}

	private static void _StartVersusModeScreen(float fadeTime){
		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.versusModeScreen == null){
			Debug.Log("Versus Mode Screen not found! Make sure you have set the prefab correctly in the Global Editor");
			MainScript.StartPlayerVersusPlayer(fadeTime);
		}else{
            MainScript.ShowScreen(MainScript.config.gameGUI.versusModeScreen);
            if (!MainScript.config.gameGUI.versusModeScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}

	private static void _StartVersusModeAfterBattleScreen(float fadeTime){
		MainScript.HideScreen(MainScript.currentScreen);
		if (MainScript.config.gameGUI.versusModeAfterBattleScreen == null){
			Debug.Log("Versus Mode \"After Battle\" Screen not found! Make sure you have set the prefab correctly in the Global Editor");
			
			MainScript._StartMainMenuScreen(fadeTime);
		}else{
            MainScript.ShowScreen(MainScript.config.gameGUI.versusModeAfterBattleScreen);
            if (!MainScript.config.gameGUI.versusModeAfterBattleScreen.hasFadeIn) fadeTime = 0;
            CameraFade.StartAlphaFade(MainScript.config.gameGUI.screenFadeColor, true, fadeTime);
		}
	}
#endregion
}

public interface MainScriptInterface
{
}