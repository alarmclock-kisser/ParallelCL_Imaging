__kernel void GrayscaleKernel(__global uchar* pixels, long length) {
    int i = get_global_id(0) * 4; // RGBA (4 Bytes pro Pixel)

    if (i + 3 >= length) return;

    // RGB-Werte extrahieren
    uchar r = pixels[i];
    uchar g = pixels[i + 1];
    uchar b = pixels[i + 2];

    // Grauwert berechnen (Luminanz-Formel)
    uchar gray = (uchar)clamp(0.299f * r + 0.587f * g + 0.114f * b, 0.0f, 255.0f);

    // Setze RGB auf den Grauwert (Alpha bleibt unverändert)
    pixels[i] = gray;
    pixels[i + 1] = gray;
    pixels[i + 2] = gray;
}
