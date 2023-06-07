
using UnityEngine;
using static Sequence;

[System.Serializable]
public class SequenceAction
{
    public ActionType actionType;  // Type of action

    // Action parameters based on the action type
    [Header("Animation-Type")]
    public AnimationClip animationClip;

    [Range(0f, 1.0f)]
    public float animationTransition = 0f;

    [Header("Audio-Type")]
    public AudioClip audioClip;

    [Range(0f, 10f)]
    public float audioDelay = 0f;

    [Header("GameObject-Type")]
    public GameObject gameObject;
    public bool enable;

    [Header("Camera-Type")]
    public Transform cameraPosition;
    public float cameraMovementDuration;
    public bool cameraAnimationTime; // Scales with the Animation End time
    public bool cameraAudioTime; // Scales with the Audio End time

    [Header("[ Animation | Audio ]")]
    public bool waitForCompletion;
}
