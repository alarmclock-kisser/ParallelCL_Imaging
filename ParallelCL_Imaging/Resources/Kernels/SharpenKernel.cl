#define KERNEL_VALUE_0 0
#define KERNEL_VALUE_1 -1
#define KERNEL_VALUE_2 0
#define KERNEL_VALUE_3 -1
#define KERNEL_VALUE_4 5
#define KERNEL_VALUE_5 -1
#define KERNEL_VALUE_6 0
#define KERNEL_VALUE_7 -1
#define KERNEL_VALUE_8 0

__kernel void SharpenKernel(__global uchar* pixels, long width, long height) {
    int gidX = get_global_id(0);
    int gidY = get_global_id(1);

    if (gidX < width && gidY < height) {
        int center = (gidY * width + gidX) * 4;
        if (center + 3 >= width * height * 4) return;

        float r = 0.0f, g = 0.0f, b = 0.0f;
        int kernelSize = 3;

        for (int ky = -1; ky <= 1; ++ky) {
            for (int kx = -1; kx <= 1; ++kx) {
                int x = gidX + kx;
                int y = gidY + ky;

                if (x >= 0 && x < width && y >= 0 && y < height) {
                    int offset = (y * width + x) * 4;
                    int kernelIndex = (ky + 1) * kernelSize + (kx + 1);
                    int kernelValue;
                    if (kernelIndex == 0) kernelValue = KERNEL_VALUE_0;
                    else if (kernelIndex == 1) kernelValue = KERNEL_VALUE_1;
                    else if (kernelIndex == 2) kernelValue = KERNEL_VALUE_2;
                    else if (kernelIndex == 3) kernelValue = KERNEL_VALUE_3;
                    else if (kernelIndex == 4) kernelValue = KERNEL_VALUE_4;
                    else if (kernelIndex == 5) kernelValue = KERNEL_VALUE_5;
                    else if (kernelIndex == 6) kernelValue = KERNEL_VALUE_6;
                    else if (kernelIndex == 7) kernelValue = KERNEL_VALUE_7;
                    else if (kernelIndex == 8) kernelValue = KERNEL_VALUE_8;
                    else kernelValue = 0; // Sicherheitshalber

                    r += pixels[offset] * kernelValue;
                    g += pixels[offset + 1] * kernelValue;
                    b += pixels[offset + 2] * kernelValue;
                }
            }
        }

        pixels[center] = clamp((int)r, 0, 255);
        pixels[center + 1] = clamp((int)g, 0, 255);
        pixels[center + 2] = clamp((int)b, 0, 255);
    }
}