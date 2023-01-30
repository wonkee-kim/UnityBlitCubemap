Unity Blit Cubemap Example
--------------------------
This Unity project contains two examples of blending cubemaps and blit cubemaps. (Built-in and URP compatible)

### 1. Blend cubemaps using [ReflectionProbe.BlendCubemap](https://docs.unity3d.com/ScriptReference/ReflectionProbe.BlendCubemap.html)
You can blend two cubemaps into one render texture simply by using ReflectionProbe.BlendCubemap(src, dot, blend, target) provided by Unity.

### 2. Blit cubemaps using Graphics.Blit and [Graphics.SetRenderTarget](https://docs.unity3d.com/ScriptReference/Graphics.SetRenderTarget.html)
If you need to blit cubemaps with a custom shader, you can use Graphics.Blit. You can assign a cubemap face to a render target using Graphics.SetRenderTarget.
