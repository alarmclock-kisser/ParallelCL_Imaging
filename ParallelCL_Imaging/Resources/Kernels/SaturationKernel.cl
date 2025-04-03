__kernel void SaturationKernel(__global uchar* pixels, long length, float saturation) {
    int i = get_global_id(0) * 4; // RGBA (4 Bytes pro Pixel)

    if (i + 3 >= length) return;

    float r = pixels[i] / 255.0f;
    float g = pixels[i + 1] / 255.0f;
    float b = pixels[i + 2] / 255.0f;

    float maxVal = max(r, max(g, b));
    float minVal = min(r, min(g, b));
    float delta = maxVal - minVal;

    float h, s, v;

    if (delta == 0.0f) {
        h = 0.0f;
        s = 0.0f;
    } else {
        s = delta / maxVal;
        if (r == maxVal) {
            h = (g - b) / delta;
        } else if (g == maxVal) {
            h = 2.0f + (b - r) / delta;
        } else {
            h = 4.0f + (r - g) / delta;
        }
        h *= 60.0f;
        if (h < 0.0f) h += 360.0f;
    }
    v = maxVal;

    s = clamp(s * saturation, 0.0f, 1.0f);

    float hh = h / 60.0f;
    int ii = (int)hh;
    float ff = hh - ii;
    float p = v * (1.0f - s);
    float q = v * (1.0f - (s * ff));
    float t = v * (1.0f - (s * (1.0f - ff)));

    float newR, newG, newB;
    switch (ii) {
        case 0: newR = v; newG = t; newB = p; break;
        case 1: newR = q; newG = v; newB = p; break;
        case 2: newR = p; newG = v; newB = t; break;
        case 3: newR = p; newG = q; newB = v; break;
        case 4: newR = t; newG = p; newB = v; break;
        default: newR = v; newG = p; newB = q; break;
    }

    pixels[i] = (uchar)clamp((int)(newR * 255.0f), 0, 255);
    pixels[i + 1] = (uchar)clamp((int)(newG * 255.0f), 0, 255);
    pixels[i + 2] = (uchar)clamp((int)(newB * 255.0f), 0, 255);
}