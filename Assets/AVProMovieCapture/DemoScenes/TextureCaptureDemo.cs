using UnityEngine;
using System.Collections;

public class TextureCaptureDemo : MonoBehaviour 
{
	public AVProMovieCaptureFromTexture _movieCapture;
	private const int Width = 256;
	private const int Height = 256;
	private const int PaletteSize = 8;
	private Texture2D _texture;
	private Color32[] _pixels;
	private Color32[] _palette;
	private float _time;

	void Start() 
	{		
		_texture = new Texture2D(Width, Height, TextureFormat.ARGB32, false);
		_texture.Apply(false, false);
		
		if (_movieCapture)
		{
			_movieCapture.SetSourceTexture(_texture);
		}		
		
		_pixels = new Color32[Width*Height];
		_palette = new Color32[PaletteSize];
		
		Keyframe[] keysR = { new Keyframe(0f, 0f), new Keyframe(0.25f, 1f), new Keyframe(1f, 0f) };
		Keyframe[] keysG = { new Keyframe(0f, 1f), new Keyframe(0.5f, 0f), new Keyframe(1f, 1f) };
		Keyframe[] keysB = { new Keyframe(0f, 1f), new Keyframe(0.75f, 0f), new Keyframe(1f, 0f) };
		AnimationCurve curveR = new AnimationCurve(keysR);
		AnimationCurve curveG = new AnimationCurve(keysG);
		AnimationCurve curveB = new AnimationCurve(keysB);
		for (int i = 0; i < PaletteSize; i++)
		{
			float r = curveR.Evaluate((float)i / (float)PaletteSize);
			float g = curveG.Evaluate((float)i / (float)PaletteSize);
			float b = curveB.Evaluate((float)i / (float)PaletteSize);
			_palette[i] = new Color32((byte)(r * 255.0f), (byte)(g * 255.0f), (byte)(b * 255.0f), (byte)(b * 255.0f));
		}
	}
	
	void OnDestroy()
	{
		if (_texture != null)
		{
			Texture2D.Destroy(_texture);
			_texture = null;
		}
	}		
		
	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			for (int i = 0; i < PaletteSize; i++)
			{
				float r = Random.value;
				float g = Random.value;
				float b = Random.value;
				_palette[i] = new Color32((byte)(r * 255.0f), (byte)(g * 255.0f), (byte)(b * 255.0f), (byte)(b * 255.0f));
			}
		}
		
		_time += Time.deltaTime;	
		
		int index = 0;
		float d = _time;
		for (int j = 0; j < Height; j++)
		{
			for (int i = 0; i < Width; i++)
			{
				float x = Mathf.Sin(i * 0.1f + d) - Mathf.Cos(j * 0.02f + d * 4f) + Mathf.Cos((j+i+d) * 0.015f) - Mathf.Sin((j-i-d*2.421f) * 0.0311f);
				x /= (float)j / (float)(Height/2);
				float lumaf = Mathf.Abs(x);
				int luma = (int)(lumaf * PaletteSize) % PaletteSize;
				_pixels[index++] = _palette[luma];
			}
		}
		
		_texture.SetPixels32(_pixels);
		_texture.Apply(false, false);
	}
	
	void OnGUI()
	{
		GUI.depth = 100;
		GUI.DrawTexture(new Rect(Screen.width / 2 - _texture.width / 2, Screen.height / 2 - _texture.height / 2, _texture.width, _texture.height), _texture);
	}
}
