__kernel void ContrastKernel(__global uchar* pixels, long length, float contrast) {
    int i = get_global_id(0) * 4; // RGBA (4 Bytes pro Pixel)

    if (i + 3 >= length) return;

    float r = pixels[i] / 255.0f;
    float g = pixels[i + 1] / 255.0f;
    float b = pixels[i + 2] / 255.0f;

    float average = 0.5f; // Mittelwert (kann auch dynamisch berechnet werden)

    r = (r - average) * contrast + average;
    g = (g - average) * contrast + average;
    b = (b - average) * contrast + average;

    pixels[i] = (uchar)clamp((int)(r * 255.0f), 0, 255);
    pixels[i + 1] = (uchar)clamp((int)(g * 255.0f), 0, 255);
    pixels[i + 2] = (uchar)clamp((int)(b * 255.0f), 0, 255);
}