__kernel void BlurKernel(__global uchar* pixels, long width, long height, int radius) {
    int gidX = get_global_id(0);
    int gidY = get_global_id(1);

    if (gidX < width && gidY < height) {
        int center = (gidY * width + gidX) * 4;
        if (center + 3 >= width * height * 4) return; // Schutz vor Array-Überlauf

        float r = 0.0f, g = 0.0f, b = 0.0f;
        int count = 0;

        for (int ky = -radius; ky <= radius; ++ky) {
            for (int kx = -radius; kx <= radius; ++kx) {
                int x = gidX + kx;
                int y = gidY + ky;

                if (x >= 0 && x < width && y >= 0 && y < height) {
                    int offset = (y * width + x) * 4;
                    r += pixels[offset];
                    g += pixels[offset + 1];
                    b += pixels[offset + 2];
                    count++;
                }
            }
        }

        if (count > 0) {
            pixels[center] = clamp((int)(r / count), 0, 255);
            pixels[center + 1] = clamp((int)(g / count), 0, 255);
            pixels[center + 2] = clamp((int)(b / count), 0, 255);
        }
    }
}