/*===============================================================================
Copyright (c) 2025 PTC Inc. and/or Its Subsidiary Companies. All Rights Reserved.

Confidential and Proprietary - Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/

using UnityEngine;
using UnityEngine.Events;
using Vuforia;

/// <summary>
/// A custom handler that inherits from the DefaultObserverEventHandler class.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class DefaultValidationEventHandler : DefaultObserverEventHandler
{
    public UnityEvent<ValidationAreaBehaviour, ValidationInfo> OnValidationInfoUpdate = new ();

    ValidationAreaBehaviour mValidationAreaBehaviour;
    ValidationStatus mPreviousValidationStatus = ValidationStatus.UNDECIDABLE;

    protected override void Start()
    {
        base.Start();

        mValidationAreaBehaviour = mObserverBehaviour as ValidationAreaBehaviour;
        if (mValidationAreaBehaviour)
        {
            mValidationAreaBehaviour.OnValidationInfoUpdateReceived += OnValidationInfoUpdateReceived;
            Debug.Log($"{mValidationAreaBehaviour.TargetName} Validation Status: { ValidationStatus.UNDECIDABLE }");
        }
    }

    protected override void OnDestroy()
    {
        if (mValidationAreaBehaviour)
            mValidationAreaBehaviour.OnValidationInfoUpdateReceived -= OnValidationInfoUpdateReceived;

        base.OnDestroy();
    }

    void OnValidationInfoUpdateReceived(ValidationAreaBehaviour behaviour, ValidationInfo info)
    {
        if (info.ValidationStatus != mPreviousValidationStatus)
        {
            mPreviousValidationStatus = info.ValidationStatus;
            Debug.Log($"{mValidationAreaBehaviour.TargetName} Validation Status: { info.ValidationStatus }");
        }

        OnValidationInfoUpdate.Invoke(behaviour, info);
    }
}