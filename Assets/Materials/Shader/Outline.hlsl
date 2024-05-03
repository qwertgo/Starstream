#ifndef SOBELOUTLINES_INDCLUDED
#define SOBELOUTLINES_INDCLUDED

//Thses are points to sample relative to the starting point
static float2 sobelSamplePoints[9] = {
    float2(-1, 1), float2(0, 1), float2(1, 1),
    float2(-1,0), float2(0, 0), float2(1, 1),
    float2(-1, -1), float2(0,-1), float2(1, -1),
};

//wheights for the x component
static float sobelXMatrix[9] = {
    1, 0, -1,
    2, 0, -2,
    1, 0, -1
};

static float sobelYMatrix[9] = {
    1, 2, 1,
    0, 0, 0,
    -1, -2, -1
};


//This function runs the sobel algorithm over the depth texture
void DepthSobel_float(float2 UV, float Thickness, 
    out float Out){
    float2 sobel = 0;

    //We can unroll this loop to make it more efficient
    //the compiler is also smart enough to remove the i = 4 iteration, which is always 0
    [unroll] for(int i = 0; i < 9; i++){
        float depth = SHADERGRAPH_SAMPLE_SCENE_NORMAL(UV + sobelSamplePoints[i] * Thickness);

        sobel += depth * float2(sobelXMatrix[i], sobelYMatrix[i]);
    }

    //Get the final sobel value
    Out = length(sobel);
}

void ColorSobel_float(float2 UV, float Thickness, 
out float Outline, out float Depth){
    //We have to run the sobel algorithm over the RGB channels seperatly
    float2 sobelR = 0;
    float2 sobelG = 0;
    float2 sobelB = 0;
    //We can unroll this loop to make it more efficient
    //The compiler is also smart enough to remove the i = 4 iteration, which is always zero
    [unroll] for(int i = 0; i < 9; i++){
        //Sample the scene color texture
        float3 rgb = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV + sobelSamplePoints[i] * Thickness);
        //Create the kernel for this iteration
        float2 kernel  = float2(sobelXMatrix[i], sobelYMatrix[i]);
        //Accumulate samples for each color
        sobelR += rgb.r * kernel;
        sobelG += rgb.g * kernel;
        sobelB += rgb.b * kernel;
    }

    //Get the fial sobel value
    //Combine the RGB values by taking the one with the largest sobel malue
    Outline = max(length(sobelR), max(length(sobelG), length(sobelB)));
    Depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV);
}

#endif