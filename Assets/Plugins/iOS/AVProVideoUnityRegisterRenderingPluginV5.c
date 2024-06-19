// AVProVideo
// (C) 2017 RenderHeads Ltd
//
// AVProVideo iOS plugin bootstrap for Unity versions before Unity5.4.
// Unity removed UnityRegisterRenderingPlugin with Unity5.5 and replaced
// it with UnityRegisterRenderingPluginV5 (first available in Unity5.4).
// We patch through to UnityRegisterRenderingPlugin for older versions
// of Unity.

#if !defined(UNITY_5_4_0) || UNITY_VERSION < 540

extern void AVPPluginUnityRegisterRenderingPlugin();

void UnityRegisterRenderingPluginV5(void (*unused)(void *), void (*unused2)())
{
	AVPPluginUnityRegisterRenderingPlugin();
}

#endif
