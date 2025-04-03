__kernel void GammaKernel(__global uchar* pixels, long length, float gamma) {
    int i = get_global_id(0) * 4; // RGBA (4 Bytes pro Pixel)

    if (i + 3 >= length) return;

    float r = pixels[i] / 255.0f;
    float g = pixels[i + 1] / 255.0f;
    float b = pixels[i + 2] / 255.0f;

    r = pow(r, gamma);
    g = pow(g, gamma);
    b = pow(b, gamma);

    pixels[i] = (uchar)clamp((int)(r * 255.0f), 0, 255);
    pixels[i + 1] = (uchar)clamp((int)(g * 255.0f), 0, 255);
    pixels[i + 2] = (uchar)clamp((int)(b * 255.0f), 0, 255);
}