/*===============================================================================
Copyright (c) 2025 PTC Inc. and/or Its Subsidiary Companies. All Rights Reserved.

Confidential and Proprietary - Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Vuforia;

/// <summary>
/// This class manages the appearance of the icon of a Validation Area in the scene.
/// The icon is used to display the validation result at runtime, showing the
/// appropriate material based on the <see cref="ValidationInfo.ValidationStatus"/>
/// reported by the ValidationAreaBehaviour.
/// </summary>
/// <remarks>
/// This class can be copied and customized to extend its behaviour.
/// </remarks>
public class ValidationAreaIconBehaviour : MonoBehaviour
{
    const float DEFAULT_ICON_SIZE = 0.025f;
    const float DEFAULT_MARGIN = 0.25f;

    static readonly HashSet<string> sPassLabelNameCandidates = new() { "pass", "correct", "ok", "success", "done", "passed", "succeed", "succeeded", "yes", "true" };
    static readonly HashSet<string> sFailLabelNameCandidates = new() { "fail", "incorrect", "notok", "notcorrect", "wrong", "failed", "no", "false" };

    public MeshRenderer IconRenderer;

    [Header("Materials")]
    public Material FailIconMaterial;
    public Material PassIconMaterial;
    public Material UndecidableIconMaterial;

    Transform mCameraTransform;
    ValidationAreaBehaviour mValidationAreaBehaviour;
    ValidationStatus mPreviousValidationStatus;
    TargetStatus mPreviousTargetStatus;

    /// <summary>
    /// Start is called when the script is enabled.
    /// </summary>
    void Start()
    {
        VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
    }

    /// <summary>
    /// OnDestroy is called when the script is destroyed.
    /// </summary>
    void OnDestroy()
    {
        if (mValidationAreaBehaviour)
            mValidationAreaBehaviour.OnValidationInfoUpdateReceived -= OnValidationInfoUpdateReceived;
    }

    /// <summary>
    /// Update is called once per frame if the script is enabled
    /// </summary>
    void Update()
    {
        if (mValidationAreaBehaviour == null || mCameraTransform == null || IconRenderer == null)
            return;

        // Rotate the icon such that it always faces the camera
        IconRenderer.transform.LookAt(IconRenderer.transform.position + mCameraTransform.forward, mCameraTransform.up);
        IconRenderer.transform.localPosition = Vector3.forward * (mValidationAreaBehaviour.AreaSize.x * 0.5f +
                                                                  DEFAULT_ICON_SIZE * (0.5f + DEFAULT_MARGIN));
    }

    void OnVuforiaStarted()
    {
        Initialize();

        VuforiaApplication.Instance.OnVuforiaStarted -= OnVuforiaStarted;
    }

    /// <summary>
    /// Callback executed when a new Validation Area result is received
    /// </summary>
    void OnValidationInfoUpdateReceived(ValidationAreaBehaviour behaviour, ValidationInfo validationInfo)
    {
        if (mPreviousTargetStatus.Equals(behaviour.TargetStatus) &&
            validationInfo.ValidationStatus == mPreviousValidationStatus)
        {
            return;
        }

        mPreviousTargetStatus = behaviour.TargetStatus;
        mPreviousValidationStatus = validationInfo.ValidationStatus;

        var showIcon = ShouldShowIcon(behaviour.TargetStatus);
        switch (validationInfo.ValidationStatus)
        {
            case ValidationStatus.NORMAL:
                var material = GetMaterialForLabel(validationInfo.Label);
                UpdateIconRenderer(material, showIcon);
                break;
            case ValidationStatus.NOT_VISIBLE:
            case ValidationStatus.UNDECIDABLE:
                UpdateIconRenderer(UndecidableIconMaterial, showIcon);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Initialize the components of the Icon
    /// </summary>
    void Initialize()
    {
        if (VuforiaBehaviour.Instance)
            mCameraTransform = VuforiaBehaviour.Instance.transform;

        if (IconRenderer == null)
        {
            var icon = GameObject.CreatePrimitive(PrimitiveType.Quad);
            icon.name = "Icon";
            icon.transform.SetParent(transform, false);
            IconRenderer = icon.GetComponent<MeshRenderer>();
        }

        mValidationAreaBehaviour = GetComponent<ValidationAreaBehaviour>();
        if (mValidationAreaBehaviour == null)
            mValidationAreaBehaviour = GetComponentsInParent<ValidationAreaBehaviour>(true).FirstOrDefault();

        if (mValidationAreaBehaviour != null)
        {
            mValidationAreaBehaviour.OnValidationInfoUpdateReceived += OnValidationInfoUpdateReceived;
            IconRenderer.transform.localPosition = Vector3.forward * (DEFAULT_ICON_SIZE * (0.5f + DEFAULT_MARGIN));
        }

        IconRenderer.transform.localScale = Vector3.one * DEFAULT_ICON_SIZE;
        IconRenderer.transform.localRotation = Quaternion.AngleAxis(90, Vector3.right);

        EnsureIconMaterials();
        UpdateIconRenderer(null, false);
    }

    /// <summary>
    /// Helper function to ensure that each icon material contains a valid value. If not, it assigns a default material
    /// </summary>
    void EnsureIconMaterials()
    {
        if (FailIconMaterial == null)
            FailIconMaterial = VuforiaConfiguration.Instance.RuntimeResources.Register.DefaultValidationIconFailMaterials.First();

        if (PassIconMaterial == null)
            PassIconMaterial = VuforiaConfiguration.Instance.RuntimeResources.Register.DefaultValidationIconPassMaterials.First();

        if (UndecidableIconMaterial == null)
            UndecidableIconMaterial = VuforiaConfiguration.Instance.RuntimeResources.Register.DefaultValidationIconUndecidableMaterials.First();
    }

    /// <summary>
    /// Helper function to apply the icon material and enable the renderer.
    /// If the material is null, the renderer's material is not changed
    /// </summary>
    void UpdateIconRenderer(Material material, bool visible)
    {
        if (IconRenderer == null)
            return;

        if (material != null)
            IconRenderer.material = material;

        IconRenderer.enabled = visible;
    }

    /// <summary>
    /// Helper function that returns the material the icon should display for the provided label
    /// </summary>
    Material GetMaterialForLabel(string label)
    {
        var sanitizedLabel = SanitizeLabel(label);

        // Try to semantically match the default pass/fail materials to the label
        if (IsPassLabel(sanitizedLabel))
            return PassIconMaterial;

        if (IsFailLabel(sanitizedLabel))
            return FailIconMaterial;

        // Fall-back to the pass material if the label could not be matched
        return PassIconMaterial;
    }

    /// <summary>
    /// Sanitizes the input label by converting it to lower case and removing white spaces and non-letter characters
    /// </summary>
    static string SanitizeLabel(string label)
    {
        return Regex.Replace(label.ToLower(), "[^a-zA-Z]", "");
    }

    /// <summary>
    /// True if the label qualifies as a 'pass' label
    /// </summary>
    static bool IsPassLabel(string label)
    {
        // Determine if the labelName matches a pass candidate
        return sPassLabelNameCandidates.Contains(label);
    }

    /// <summary>
    /// True if the label qualifies as a 'fail' label
    /// </summary>
    static bool IsFailLabel(string label)
    {
        // Determine if the labelName matches a fail candidate
        return sFailLabelNameCandidates.Contains(label);
    }

    /// <summary>
    /// True if the ValidationArea is extended tracked and guidance is available
    /// </summary>
    static bool ShouldShowIcon(TargetStatus targetStatus)
    {
        return targetStatus.Status == Status.EXTENDED_TRACKED;
    }
}