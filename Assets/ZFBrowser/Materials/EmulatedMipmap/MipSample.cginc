

/**
 * Samples the given mipmap-free texture as if it had a mipmap.
 * This is kind of expensive per fragment, especially if the texture is far from the camera,
 * but I imagine your using this because it's faster than the alternative.
 *
 * The {tex} and {uv} parameters do what you would expect. We will sample that texture
 * "at" the given uv coordinate by sampling and averaging a number of points in the vicinity.
 *
 * The averaging calculation assumes a linear color space and no premultiplied alpha.
 *
 * textureSize should be the size, in pixels, of {tex}. For a "sampler2D _MyTex" add a
 * "float4 _MyTex_TexelSize" which Unity will fill for you. Pass in _MyTex_TexelSize.zw
 *
 * maxLineSamples indicates the maximum number of times we can subsample per dimension. Worst case is
 * maxLineSamples*maxLineSamples samples.
 *
 * Pass 1 for sampleMultiplier for the default behavior. Pass less or more for fewer samples or more samples respectively.
 */
fixed4 mipSample(
	sampler2D tex, float2 uv,
	float2 textureSize, int maxLineSamples, float sampleMultiplier
) {
	//Roughly, we need to sample more times if we are far than near.
	//Rather, we should sample about the same number of times, regardless
	//of if we fill the whole screen or only fill a hundredth.

	//how much uv changes when we go right a pixel
	float2 uvDx = ddx(uv);
	//how much uv changes when we go down a pixel
	float2 uvDy = ddy(uv);


	int xSamples = clamp(length(uvDx) * textureSize.x * sampleMultiplier, 1, maxLineSamples);
	int ySamples = clamp(length(uvDy) * textureSize.x * sampleMultiplier, 1, maxLineSamples);
	//Note: above we use using size.x for both, assuming a square texture. Don't use size.y as the texture xy won't always
	//line up with our xy. If we did that, we may end up looking different based on the camera/object rotation.

	//Determine base (origin) and step size
	//We'll pull our samples from a rectangle halfway left, right, above, and below our
	//"normal" target point
	float2 p = uv - uvDx / 2 - uvDy / 2;
	uvDx /= (float)xSamples;
	uvDy /= (float)ySamples;

	//Sample a bunch.
	float4 c = float4(0, 0, 0, 0);
	for (int x = 0; x < xSamples; ++x) {
		for (int y = 0; y < ySamples; ++y) {
			//nb: can't use tex2D directly here because the compiler wants to unroll this loop to get the gradients.
			//Instead, feed it the gradients outright.
			c += tex2Dgrad(tex, p + uvDx * x + uvDy * y, uvDx, uvDy);
		}
	}

	//Average the samples.
	//(assuming linear color space)
	//(assuming no premultiplied alpha)
	c /= xSamples * ySamples;

	//Uncomment to visualize sample cost:
	// if (xSamples * ySamples > 100) c.g = 1;
	// else if (xSamples * ySamples > 10) c.g = 0.6;
	// else if (xSamples * ySamples > 1) c.g = 0.3;
	// else c.g = 0;

	return c;
}
