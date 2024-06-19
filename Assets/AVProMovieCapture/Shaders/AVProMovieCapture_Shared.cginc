
float4
rgb2yuv(float4 rgb)
{
	float r = rgb.r;
	float g = rgb.g;
	float b = rgb.b;
	float y = 0.299 * r + 0.587 * g + 0.114 * b;
	float u = -0.147 * r -0.289 * g + 0.436 * b;
	float v = 0.615 * r - 0.515 * g - 0.100 * b;

	return float4(y, u, v, 0.0);
}