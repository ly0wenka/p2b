using FPLibrary;
using UnityEngine;

[System.Serializable]
public class BounceOptions
{
    public Sizes bounceForce;
    public GameObject bouncePrefab;
    public float bounceKillTime = 2;
    public float minimumBounceForce = 30;
    public Fix64 _minimumBounceForce;
    public float maximumBounces = 2;
    public Fix64 _maximumBounces;
    public bool sticky = false;
    public bool bounceHitBoxes = true;
    public bool shakeCamOnBounce = true;
    public float shakeDensity = .6f;
    public Fix64 _shakeDensity;
    public AudioClip bounceSound;
}