__kernel void BrightnessKernel(__global uchar* pixels, long length, float brightness) {
    int i = get_global_id(0) * 4; // RGBA (4 Bytes pro Pixel)

    if (i + 3 >= length) return;

    int r = clamp((int)(pixels[i] + brightness), 0, 255);
    int g = clamp((int)(pixels[i + 1] + brightness), 0, 255);
    int b = clamp((int)(pixels[i + 2] + brightness), 0, 255);

    pixels[i] = (uchar)r;
    pixels[i + 1] = (uchar)g;
    pixels[i + 2] = (uchar)b;
}