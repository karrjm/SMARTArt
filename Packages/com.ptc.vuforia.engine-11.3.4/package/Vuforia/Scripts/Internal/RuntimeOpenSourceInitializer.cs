/*===============================================================================
Copyright (c) 2025 PTC Inc. and/or Its Subsidiary Companies. All Rights Reserved.

Confidential and Proprietary - Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/

#if UNITY_WSA && !UNITY_EDITOR
#define RUNTIME_WSA
#endif

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Vuforia.ARFoundation;
using Vuforia.Internal.Core;
using Object = UnityEngine.Object;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

#if UNITY_XR_ARFOUNDATION || UNITY_XR_OPENXR
using UnityEngine.XR;
using UnityEngine.XR.Management;
using Unity.XR.CoreUtils;
#endif

#if UNITY_XR_OPENXR
using UnityEngine.XR.OpenXR;
#endif

#if MICROSOFT_MIXEDREALITY_OPENXR
using Microsoft.MixedReality.OpenXR;
#endif

#if PLATFORM_LUMIN && MAGICLEAP_UNITYSDK
using UnityEngine.XR.MagicLeap;
using System.Linq;
#endif

#if UNITY_RENDER_PIPELINES_UNIVERSAL
using UnityEngine.Rendering.Universal;
#endif

namespace Vuforia.UnityRuntimeCompiled
{
    public class RuntimeOpenSourceInitializer
    {
        static IUnityRuntimeCompiledFacade sFacade;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnRuntimeMethodLoad()
        {
            InitializeFacade();
        }

        static void InitializeFacade()
        {
            if (sFacade != null) return;
            
            sFacade = new OpenSourceUnityRuntimeCompiledFacade();
            UnityRuntimeCompiledFacade.Instance = sFacade;
        }
        
        class OpenSourceUnityRuntimeCompiledFacade : IUnityRuntimeCompiledFacade
        {
            public IUnityInputBridge UnityInputBridge { get; } = new UnityInputBridge();

            public IUnityRenderPipeline UnityRenderPipeline { get; } = new UnityRenderPipeline();

            public IUnityXRBridge UnityXRBridge { get; } = new UnityXRBridge();

            public bool IsUnityUICurrentlySelected()
            {
                return !(EventSystem.current == null || EventSystem.current.currentSelectedGameObject == null);
            }

            public Font GetDefaultFont()
            {
                return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            }
        }
        
        class UnityInputBridge : IUnityInputBridge
        {
            public bool IsNewInputSystemEnabled()
            {
#if ENABLE_INPUT_SYSTEM
                return true;
#else
                return false;
#endif
            }
        }

        class UnityRenderPipeline : IUnityRenderPipeline
        {
            public event Action<List<Camera>> BeginFrameRendering;
            public event Action<Camera> BeginCameraRendering;

            public UnityRenderPipeline()
            {
                RenderPipelineManager.beginContextRendering += OnBeginFrameRendering;
                RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            }
            void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
            {
                if (BeginCameraRendering != null)
                    BeginCameraRendering(camera);
            }

            void OnBeginFrameRendering(ScriptableRenderContext context, List<Camera> cameras)
            {
                if (BeginFrameRendering != null)
                    BeginFrameRendering(cameras);
            }

            public Camera CreateNonOccludingSimulatorCamera(Camera mainCamera, string name, float depth, int cullingMask)
            {
                var cameraObject = new GameObject(name);
                var notOccludingCamera = cameraObject.AddComponent<Camera>();
                cameraObject.transform.SetParent(mainCamera.transform, false);
                notOccludingCamera.clearFlags = CameraClearFlags.SolidColor;
                notOccludingCamera.backgroundColor = Color.black;
                notOccludingCamera.depth = depth;
                notOccludingCamera.cullingMask = cullingMask;
                notOccludingCamera.allowHDR = mainCamera.allowHDR;

#if UNITY_RENDER_PIPELINES_UNIVERSAL
                if (VuforiaRuntimeUtilities.IsUsingBuiltInRenderPipeline())
                    return notOccludingCamera;

                var mainCameraData = mainCamera.GetUniversalAdditionalCameraData();
                mainCameraData.renderType = CameraRenderType.Overlay;
                var notOccludingCameraData = notOccludingCamera.GetUniversalAdditionalCameraData();
                notOccludingCameraData.renderType = CameraRenderType.Base;
                notOccludingCameraData.allowHDROutput = mainCameraData.allowHDROutput;

                notOccludingCameraData.cameraStack.Add(mainCamera);
#endif

                return notOccludingCamera;
            }
        }

        class UnityXRBridge : IUnityXRBridge
        {
#if UNITY_XR_OPENXR
            XROrigin mXROrigin;
#endif

            public UnityXRBridge()
            {
                RegisterCallbacks();
            }

            void RegisterCallbacks()
            {
#if RUNTIME_WSA && UNITY_XR_OPENXR
                var xrSettings = XRGeneralSettings.Instance;
                var xrManager = xrSettings.Manager;
                var xrLoader = xrManager.activeLoader;
                var xrInput = xrLoader.GetLoadedSubsystem<XRInputSubsystem>();

                xrInput.trackingOriginUpdated += TrackingOriginUpdated;
#endif
            }

#pragma warning disable 0067
            public event Action OnTrackingOriginUpdated;
#pragma warning restore 0067

            public bool IsOpenXREnabled()
            {
#if UNITY_XR_OPENXR
                return XRGeneralSettings.Instance?.AssignedSettings?.activeLoader is OpenXRLoader;
#else
                return false;
#endif
            }

            public Vector3 GetXROriginOffset()
            {
#if UNITY_XR_OPENXR
                if (mXROrigin == null)
                {
                    mXROrigin = Object.FindFirstObjectByType<XROrigin>();
                }

                if (mXROrigin != null)
                {
                    // The offset is based on the world-space position of the XROrigin GameObject
                    var xrOriginTransform = mXROrigin.transform;
                    var offset = xrOriginTransform.position;
                    // The Y component of the XROrigin's local position is replaced at runtime by the CameraYOffset,
                    // so the Y value of the actual XROrigin world position at runtime is given by the position of
                    // the XROrigin's parent object (diff between XROrigin world and local positions) and the CameraYOffset
                    offset.y = xrOriginTransform.position.y - xrOriginTransform.localPosition.y + mXROrigin.CameraYOffset;
                    return offset;
                }
#endif
                return Vector3.zero;
            }

            public IntPtr GetHoloLensSpatialCoordinateSystemPtr()
            {
#if RUNTIME_WSA && MICROSOFT_MIXEDREALITY_OPENXR
                // This method returns null for a short amount of time during initialization.
                // On HoloLens we attempt the configuration of the SceneCoordinateSystem until 
                // a non-null value is returned.
                // After initialization, we rely on the XRInputSubsystem.trackingOriginUpdated event
                // to retrieve a valid pointer to the SceneCoordinateSystem.
                var sceneCoordinateSystem = PerceptionInterop.GetSceneCoordinateSystem(Pose.identity);
                if (sceneCoordinateSystem == null)
                {
                    return IntPtr.Zero;
                }
                
                return Marshal.GetIUnknownForObject(sceneCoordinateSystem);
#else
                Debug.LogError("Failed to get HoloLens Spatial Coordinate System. " +
                               "Please include the appropriate XR Plugin package into your project.");
                return IntPtr.Zero;
#endif
            }

            public bool IsHolographicDevice()
            {
#if RUNTIME_WSA && (MICROSOFT_MIXEDREALITY_OPENXR || UNITY_XR_OPENXR)
                var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
                SubsystemManager.GetSubsystems(xrDisplaySubsystems);

                foreach (var xrDisplay in xrDisplaySubsystems)
                {
                    if (xrDisplay.running && !xrDisplay.displayOpaque)
                        return true;
                }
#endif
                return false;
            }

            public void SetFocusPointForFrame(Vector3 position, Vector3 normal)
            {
#if RUNTIME_WSA && (MICROSOFT_MIXEDREALITY_OPENXR || UNITY_XR_OPENXR)
                var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
                SubsystemManager.GetSubsystems(xrDisplaySubsystems);

                foreach (var xrDisplay in xrDisplaySubsystems)
                {
                    if (xrDisplay.running && !xrDisplay.displayOpaque)
                    {
                        xrDisplay.SetFocusPlane(position, normal, Vector3.zero);
                        return;
                    }
                }
#endif
            }

#if RUNTIME_WSA && UNITY_XR_OPENXR
            void TrackingOriginUpdated(XRInputSubsystem inputSubsystem)
            {
                OnTrackingOriginUpdated?.Invoke();
            }
#endif
        }
    }
}
