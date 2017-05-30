//  --------------------------------------------------------------------------------------------------------------------
//     <copyright file="CameraController.cs">
//         Copyright (c) Nathan Bowman. All rights reserved.
//         Licensed under the MIT License. See LICENSE file in the project root for full license information.
//     </copyright>
//  --------------------------------------------------------------------------------------------------------------------
using System;

using Events;

using NateTools.Utils;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

/// <summary>
///     Controls the camera for the player
/// </summary>
/// <remarks>
///     I've previously done camera window based cameras and although it would suit the project,
///     I thought I'd try something a bit different
/// </remarks>
public class CameraController : MonoBehaviour
{
    public Vector2 MaxDistanceToLead = new Vector2(4, 2);

    /// <summary>
    ///     The time it takes the camera to reach its current target X axis
    /// </summary>
    [Range(0f, 4f)]
    public float SmoothTimeX = 1f;

    /// <summary>
    ///     The time it takes the camera to reach its current target Y axis
    /// </summary>
    [Range(0f, 4f)]
    public float SmoothTimeY = 1f;

    /// <summary>
    ///     Cached reference to the camera on the GameObject
    /// </summary>
    private Camera cam;

    /// <summary>
    ///     A target for secondary focus
    /// </summary>
    [SerializeField]
    private GameObject focusTarget;

    private CameraMode lastMode = CameraMode.Lead;

    /// <summary>
    ///     The primary object the camera should follow
    /// </summary>
    [SerializeField]
    private GameObject mainTarget;

    /// <summary>
    ///     The RigidBody2D of the MainTarget
    /// </summary>
    private Rigidbody2D mainTargetBody;

    /// <summary>
    ///     The mode in which the camera operates
    /// </summary>
    [SerializeField]
    private CameraMode mode = CameraMode.Follow;

    /// <summary>
    ///     The position the camera is currently moving towards
    /// </summary>
    private Vector3 requestedPosition;

    /// <summary>
    ///     Reference value for Mathf.SmoothDamp on the X axis
    /// </summary>
    private float smoothVelocityX;

    /// <summary>
    ///     Reference value for Mathf.SmoothDamp on the Y axis
    /// </summary>
    private float smoothVelocityY;

    /// <summary>
    ///     The operating mode of the camera
    /// </summary>
    public enum CameraMode
    {
        /// <summary>
        ///     The camera will follow the target 1:1
        /// </summary>
        Follow,

        /// <summary>
        ///     The camera will move in front of the target based on its velocity
        /// </summary>
        Lead,

        /// <summary>
        ///     The camera will try to center the focusTarget if it is close to the camera
        /// </summary>
        MultiFocus,

        /// <summary>
        ///     The camera will make no adjustments
        /// </summary>
        Static,
    }

    /// <summary>
    ///     Gets or sets the secondary focus target
    /// </summary>
    public GameObject FocusTarget { get { return focusTarget; } set { focusTarget = value; } }

    /// <summary>
    ///     Gets or sets The primary target of the camera
    /// </summary>
    public GameObject MainTarget { get { return mainTarget; } set { mainTarget = value; } }

    /// <summary>
    ///     Gets or sets the operating mode of the camera
    /// </summary>
    public CameraMode Mode { get { return mode; } set { mode = value; } }

    /// <summary>
    ///     Basic follow target
    /// </summary>
    private void Follow()
    {
        if (!mainTarget)
        {
            // Debug.LogError(GetType().FullName + ".Follow() : mainTarget is null");

            // default to static as we have no target
            Static();
            return;
        }

        // Set the requested position to targets position maintaining z
        requestedPosition = ((Vector2)mainTarget.transform.position).ToVector3(transform.position.z);
    }

    /// <summary>
    ///     Adjust the camera to track in front of main target based on velocity
    /// </summary>
    private void Lead()
    {
        if (!mainTarget)
        {
            //Debug.LogError(GetType().FullName + ".Lead() : mainTarget is null");

            // default to static as we have no target
            Static();
            return;
        }

        // Check if we have the correct RigidBody2D cached
        if (!mainTargetBody || (mainTargetBody.gameObject != mainTarget))
        {
            mainTargetBody = mainTarget.GetComponent<Rigidbody2D>();
        }

        // Early out if the RigidBody2D is invalid
        if (!mainTargetBody)
        {
            // Debug.LogError(GetType().FullName + ".Lead() : mainTargetBody is null");

            // Default to follow as we have a target but no RigidBody2D
            Follow();
            return;
        }

        // limit the camera adjustment to half a screen -1 unit in either direction
        var adjustmentLimit = ((cam.ViewportToWorldPoint(Vector3.one) - cam.ViewportToWorldPoint(Vector3.zero)) / 2f).Add(-1, -1);

        // Set the requested position to targets position + velocity maintaining z and clamped to half a screen in each direction
        requestedPosition =
            ((Vector2)mainTarget.transform.position).ToVector3(transform.position.z)
            .Add(Mathf.Clamp(mainTargetBody.velocity.x, -adjustmentLimit.x, adjustmentLimit.x), Mathf.Clamp(mainTargetBody.velocity.y, -adjustmentLimit.y, adjustmentLimit.y));
    }

    /// <summary>
    ///     Adjust the camera so the mainTarget is on screen and the focusTarget is centered.
    ///     The mode is ignored if the focusTarget is too far away
    /// </summary>
    private void MultiFocus()
    {
        /* we need to frame 2 targets.
         * The main target must remain on the screen at all times.
         */

        if (!mainTarget)
        {
            //Debug.LogError(GetType().FullName + ".MainFocus() : mainTarget is null");

            // default to static as we have no target
            Static();
            return;
        }

        if (!focusTarget)
        {
            //Debug.LogError(GetType().FullName + ".MainFocus() : focusTarget is null");

            // Default to follow as we have a target but no focusTarget
            Lead();
            return;
        }

        // limit the camera adjustment to half a screen -1 unit in either direction
        var boundsSize = (cam.ViewportToWorldPoint(Vector3.one) - cam.ViewportToWorldPoint(Vector3.zero)).Add(-4, -4f);
        var cameraMoveBounds = new Bounds(mainTarget.transform.position, boundsSize);

        if (cameraMoveBounds.Contains(focusTarget.transform.position))
        {
            requestedPosition = focusTarget.transform.position;
        }
        else
        {
            // set position to the closest possible point
            requestedPosition = cameraMoveBounds.ClosestPoint(focusTarget.transform.position);

            /* // Default to velocity based camera
             * Lead();
             */
        }
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<PlayerDied>(StopCamera);
        EventManager.RemoveListener<PlayerSpawned>(StartCamera);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (mainTarget)
        {
            Gizmos.DrawIcon(mainTarget.transform.position, "Camera Icon", true);
        }

        if (FocusTarget)
        {
            Gizmos.DrawIcon(focusTarget.transform.position, "Search Icon", true);
        }

        if (EditorApplication.isPlaying && mainTarget)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(mainTarget.transform.position, requestedPosition);
        }
    }
#endif

    private void OnEnable()
    {
        EventManager.AddListener<PlayerDied>(StopCamera);
        EventManager.AddListener<PlayerSpawned>(StartCamera);
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void StartCamera(GameEvent gameEvent)
    {
        if (gameEvent.Sender != null)
        {
            transform.position = gameEvent.Sender.transform.position;
        }

        Mode = lastMode;
    }

    /// <summary>
    ///     Maintain the cameras current position
    /// </summary>
    private void Static()
    {
        requestedPosition = transform.position;
    }

    private void StopCamera(GameEvent gameEvent)
    {
        lastMode = mode;
        Mode = CameraMode.Static;
    }

    
    // Note: camera movement should be in FixedUpdate as its following an object under the influence of Unity Physics 
    private void FixedUpdate()
    {
        switch (mode)
        {
            case CameraMode.Follow:
                Follow();
                break;
            case CameraMode.Lead:
                Lead();
                break;
            case CameraMode.MultiFocus:
                MultiFocus();
                break;
            case CameraMode.Static:
                Static();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // calculate pixel perfect movement factor
        var pixelsPerUnit = cam.pixelHeight / (cam.orthographicSize * 2);
        var unitsPerPixel = 1f / pixelsPerUnit;

        // Avoid extra math when not moving
        if (requestedPosition != transform.position)
        {
            // smooth camera movement over time
            var position =
                Mathf.SmoothDamp(transform.position.x, requestedPosition.x, ref smoothVelocityX, SmoothTimeX)
                    .ToVector2(Mathf.SmoothDamp(transform.position.y, requestedPosition.y, ref smoothVelocityY, SmoothTimeY))
                    .ToVector3(transform.position.z);

            // lock the new position to the pixel grid
            position.x = Mathf.Round(position.x / unitsPerPixel) * unitsPerPixel;
            position.y = Mathf.Round(position.y / unitsPerPixel) * unitsPerPixel;

            // apply camera movement
            transform.position = position;
        }
    }
}