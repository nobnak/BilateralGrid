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

#endif
