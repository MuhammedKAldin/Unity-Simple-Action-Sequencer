using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : MonoBehaviour
{
    // Enum to define different types of actions
    public enum ActionType
    {
        Animation,
        Audio,
        GameObject,
        Camera
    }

    public Animator animator;
    public AudioSource audioSource;
    public Camera mainCamera;
    public int currentActionIndex = 0;  // Index to keep track of the current action

    public bool LoopSequence;

    public List<SequenceAction> sequenceActions;  // List to store the sequence of actions

    private void Start()
    {
        // Start the sequence when the game starts
        StartSequence();
    }

    private void StartSequence()
    {
        StartCoroutine(ExecuteSequence());
    }

    private IEnumerator ExecuteSequence()
    {
        if(!LoopSequence)
        {
            // Iterate through the sequence of actions
            for (int i = 0; i < sequenceActions.Count; i++)
            {
                SequenceAction action = sequenceActions[i];
                currentActionIndex = i;

                // Handle different types of actions
                switch (action.actionType)
                {
                    case ActionType.Animation:
                        // Play animation and wait for it to finish
                        yield return StartCoroutine(PlayAnimation(action, action.animationClip, action.waitForCompletion, action.animationTransition));
                        break;
                    case ActionType.Audio:
                        // Play audio clip and wait for it to finish
                        yield return StartCoroutine(PlayAudio(action, action.audioClip, action.waitForCompletion, action.audioDelay));
                        break;
                    case ActionType.GameObject:
                        // Enable or disable game object
                        SetGameObjectActive(action.gameObject, action.enable);
                        break;
                    case ActionType.Camera:
                        // Move the camera smoothly
                        yield return StartCoroutine(MoveCamera(action.cameraPosition, action.cameraMovementDuration));
                        break;
                }
            }

            Debug.Log("Once-Sequence finished!");
        }

        while (LoopSequence)
        {
            // Iterate through the sequence of actions
            for (int i = 0; i < sequenceActions.Count; i++)
            {
                SequenceAction action = sequenceActions[i];
                currentActionIndex = i;

                // Handle different types of actions
                switch (action.actionType)
                {
                    case ActionType.Animation:
                        // Play animation and wait for it to finish
                        yield return StartCoroutine(PlayAnimation(action,action.animationClip, action.waitForCompletion, action.animationTransition));
                        break;
                    case ActionType.Audio:
                        // Play audio clip and wait for it to finish
                        yield return StartCoroutine(PlayAudio(action, action.audioClip, action.waitForCompletion, action.audioDelay));
                        break;
                    case ActionType.GameObject:
                        // Enable or disable game object
                        SetGameObjectActive(action.gameObject, action.enable);
                        break;
                    case ActionType.Camera:
                        // Move the camera smoothly
                        yield return StartCoroutine(MoveCamera(action.cameraPosition, action.cameraMovementDuration));
                        break;
                }
            }

            Debug.Log("Cycled-Sequence finished!");

            // Check if looping is enabled
            if (!LoopSequence)
            {
                break; // Exit the loop and finish the sequence
            }
        } 


    }

    private IEnumerator PlayAnimation(SequenceAction sequence, AnimationClip animationClip, bool waitForCompletion, float transitionTime)
    {
        if (sequence.cameraPosition != null)
        {
            if (sequence.cameraAnimationTime)
            {
                StartCoroutine(MoveCamera(sequence.cameraPosition, GetAnimationDuration(animationClip.name)));
            }
            else
            {
                StartCoroutine(MoveCamera(sequence.cameraPosition, sequence.cameraMovementDuration));
            }
        }

        animator.CrossFade(animationClip.name, transitionTime);

        // Wait for the animation to finish if required
        if (waitForCompletion)
        {
            float animationLength = GetAnimationDuration(animationClip.name);
            yield return new WaitForSeconds(animationLength);
        }
    }

    private IEnumerator PlayAudio(SequenceAction sequence, AudioClip audioClip, bool waitForCompletion, float delayDuration = 0f)
    {
        if (sequence.cameraPosition != null)
        {
            if(sequence.cameraAudioTime)
            {
                StartCoroutine(MoveCamera(sequence.cameraPosition, delayDuration + audioClip.length));
            }
            else
            {
                StartCoroutine(MoveCamera(sequence.cameraPosition, sequence.cameraMovementDuration));
            }
        }

        if (delayDuration > 0)
        {
            yield return new WaitForSeconds(delayDuration);
        }

        audioSource.PlayOneShot(audioClip);

        // Wait for the audio clip to finish if required
        if (waitForCompletion)
        {
            yield return new WaitForSeconds(audioClip.length);
        }

    }

    private void SetGameObjectActive(GameObject gameObject, bool enable)
    {
        gameObject.SetActive(enable);
    }

    private IEnumerator MoveCamera(Transform cameraPosition, float movementDuration)
    {
        // Move the camera smoothly to the specified position
        Transform cameraTransform = mainCamera.transform;
        Vector3 startPosition = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < movementDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / movementDuration);

            // Calculate the target position and rotation
            Vector3 targetPosition = Vector3.Lerp(startPosition, cameraPosition.position, t);
            Quaternion targetRotation = Quaternion.Slerp(startRotation, cameraPosition.rotation, t);
            cameraTransform.Translate(targetPosition - cameraTransform.position, Space.World);
            cameraTransform.rotation = targetRotation;

            yield return null;
        }
    }
    private float GetAnimationDuration(string animationName)
    {
        var animatorController = animator.runtimeAnimatorController;
        foreach (var clip in animatorController.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        Debug.LogWarning("Animation " + animationName + " not found!");
        return 0f;
    }


}
