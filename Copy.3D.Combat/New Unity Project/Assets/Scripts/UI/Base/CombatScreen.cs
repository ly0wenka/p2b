using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScreen : MonoBehaviour
{
    public bool canvasPreview = true;
    public GameObject firstSelectableGameObject = null;
    public bool hasFadeIn = true;
    public bool hasFadeOut = true;
    public bool wrapinput = true;

    public virtual void DoFixedUpdate(
        IDictionary<InputReferences, InputEvents> player1PreviousInputs,
        IDictionary<InputReferences, InputEvents> player1CurrentInputs,
        IDictionary<InputReferences, InputEvents> player2PreviousInputs,
        IDictionary<InputReferences, InputEvents> player2CurrentInputs
        )
    { }

    public virtual bool IsVisible() => this.gameObject.activeInHierarchy;

    public virtual void OnHide() { }
    public virtual void OnShow() { }
    public virtual void SelectOption(int option, int player) { }
}
