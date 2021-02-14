using System;

public class StoryModeScreen : CombatScreen
{
    #region public instance properties

    public Action nextScreenAction { get; set; }

    #endregion


    #region public instance methods

    public virtual void GoToNextScreen() => nextScreenAction?.Invoke();

    #endregion
}