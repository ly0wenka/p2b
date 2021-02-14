using System;
using System.Reflection;

public class OptionsScreen : CombatScreen
{
    public virtual float GetMusicVolume() => MainScript.GetMusicVolume();

    public virtual float GetSoundFXVolume() => MainScript.GetSoundFXVolume();

    public virtual void GoToControlsScreen()
    {
        if (MainScript.config.inputOptions.inputManagerType == InputManagerType.cInput && MainScript.isCInputInstalled)
        {
            MainScript.SearchClass("cGUI").GetMethod(
                "ToggleGUI",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy,
                null,
                new Type[] { },
                null
            ).Invoke(null, null);
        }
        else if (MainScript.config.inputOptions.inputManagerType == InputManagerType.Rewired && MainScript.isRewiredInstalled)
        {
            if (RewiredInputController.inputConfiguration != null)
            {
                RewiredInputController.inputConfiguration.ShowInputConfigurationUI(() =>
                    MainScript.StartOptionsScreen(
                        0.1f)); // show the config screen and return to Options screen when screen is closed
                Destroy(this
                    .gameObject); // close options screen when control config UI is opened to prevent UFEScreen navigation system from interfering
            }
        }
    }

    public virtual void GoToMainMenuScreen() => MainScript.StartMainMenuScreen();

    public virtual bool IsMusicMuted() => !MainScript.config.music;

    public virtual bool IsSoundMuted() => !MainScript.config.soundfx;

    public void MuteMusic(bool mute) => this.SetMusic(!mute);

    public void MuteSoundFX(bool mute) => this.SetSoundFX(!mute);

    public virtual void SetAIDifficulty(AIDifficultySettings difficulty)
    {
        if (difficulty != null)
        {
            MainScript.SetAIDifficulty(difficulty.difficultyLevel);
        }
    }

    public virtual void SetAIEngine(AIEngine aiEngine)
    {
        if (MainScript.isAiAddonInstalled)
        {
            MainScript.SetAIEngine(aiEngine);
        }
        else
        {
            MainScript.SetAIEngine(AIEngine.RandomAI);
        }
    }

    public virtual void SetDebugMode(bool enabled) => MainScript.SetDebugMode(enabled);

    public virtual void SetMusic(bool enabled) => MainScript.SetMusic(enabled);

    public virtual void SetSoundFX(bool enabled) => MainScript.SetSoundFX(enabled);

    public virtual void SetMusicVolume(float volume) => MainScript.SetMusicVolume(volume);

    public virtual void SetSoundFXVolume(float volume) => MainScript.SetSoundFXVolume(volume);

    public virtual void ToggleAIEngine()
    {
        if (MainScript.GetAIEngine() == AIEngine.RandomAI)
        {
            this.SetAIEngine(AIEngine.FuzzyAI);
        }
        else
        {
            this.SetAIEngine(AIEngine.RandomAI);
        }
    }

    public virtual void ToggleDebugMode() => this.SetDebugMode(!MainScript.config.debugOptions.debugMode);

    public virtual void ToggleMusic() => MainScript.SetMusic(!MainScript.GetMusic());

    public virtual void ToggleSoundFX() => MainScript.SetSoundFX(!MainScript.GetSoundFX());
}