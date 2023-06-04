using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Models
{
    #region - Camera -
    [Serializable]
    public class CameraSettings
    {
        [Header("CameraSettings")]
        public float minimumCollisionOffSet = 1f;
        public float cameraCollisionOffSet = 0.2f;
        public float cameraFollowSpeed = 0.2f;
        public float cameraLookSpeed = 0.05f;
        public float cameraPivotSpeed = 0.05f;
        public float cameraCollisionRadius = 0.2f;
        public float maximumLockOnDistance;
        public float minPivotAngle;
        public float maxPivotAngle;
    }
    #endregion

    #region - Player -
    [Serializable]
    public class PlayerSettings
    {
        [Header("Player Speeds")]
        public float walkingSpeed = 2f;
        public float runningSpeed = 5f;
        public float sprintingSpeed = 7f;
        public float rotationSpeed = 15f;

        [Header("Falling Settings")]
        public float LeapingVelocity;
        public float FallingVelocity;
        public LayerMask GroundLayer;
        public float RayCastHeightOffSet = 0.5f;

        [Header("Jump Speeds")]
        public float jumpHeight = 2;
        public float gravityIntensity = -15;

        [Header("Player Settings Step Climb")]
        public float StepHeight = 0.3f;
        public float StepSmooth = 0.1f;
    }

    [Serializable]
    public class PlayerStatistics
    {
        [Header("Player Health And Stamina Levels")]
        public int StaminaLevel = 10;
        public int HealthLevel = 10;
    }
    #endregion
}

