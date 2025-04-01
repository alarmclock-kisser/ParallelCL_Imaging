__kernel void TestParamsKernel(__global uchar* pixels, int r, int g, int b, float hueShift) {
    int i = get_global_id(0) * 4; // Assuming each pixel is represented by 4 bytes (RGBA)

    // Extract the original color components
    uchar originalR = pixels[i];
    uchar originalG = pixels[i + 1];
    uchar originalB = pixels[i + 2];
    uchar originalA = pixels[i + 3];

    // Apply the filter
    uchar newR = clamp(originalR + r, 0, 255);
    uchar newG = clamp(originalG + g, 0, 255);
    uchar newB = clamp(originalB + b, 0, 255);

    // Apply hue shift (simplified example)
    float hue = atan2(sqrt(3.0f) * (newG - newB), 2.0f * newR - newG - newB);
    hue += hueShift;
    float chroma = sqrt(newR * newR + newG * newG + newB * newB);

    newR = chroma * cos(hue);
    newG = chroma * cos(hue - 2.0f * M_PI / 3.0f);
    newB = chroma * cos(hue + 2.0f * M_PI / 3.0f);

    // Store the new color components back to the pixel array
    pixels[i] = clamp(newR, 0, 255);
    pixels[i + 1] = clamp(newG, 0, 255);
    pixels[i + 2] = clamp(newB, 0, 255);
    pixels[i + 3] = originalA; // Preserve the alpha channel
}
