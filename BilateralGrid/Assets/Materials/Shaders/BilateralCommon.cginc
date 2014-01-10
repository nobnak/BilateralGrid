#ifndef BILATERAL_COMMON
#define BILATERAL_COMMON

float3 uv2gridInt(float2 xyOnImage, float l, float3 _GridSize) {
	float3 xyzOnImage = float3(xyOnImage, l);
	float3 xyzOnGrid = round(xyzOnImage * _GridSize) + 1;
	return xyzOnGrid;
}

float3 uv2gridFloat(float2 xyOnImage, float l, float3 _GridSize) {
	float3 xyzOnImage = float3(xyOnImage, l);
	float3 xyzOnGrid = 1 + (xyzOnImage * _GridSize);
	return xyzOnGrid;
}

float rgb2luminance(float4 c) {
	float l = (0.2126 * c.r) + (0.7152 * c.g) + (0.0722 * c.b);
	return l;
}

float4 gamma2linear(float4 c, float gamma) {
	c.rgb = pow(c.rgb, gamma);
	return c;
}

#endif
