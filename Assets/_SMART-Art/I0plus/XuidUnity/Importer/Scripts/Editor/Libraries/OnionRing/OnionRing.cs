﻿using UnityEngine;

namespace OnionRing
{
    public class TextureSlicer
    {
        private const int SafetyMargin = 2;
        private const int Margin = 2;
        private readonly int height;
        private readonly int[] pixels;

        private readonly int width;

        private TextureSlicer(Texture refTexture, Color[] getPixels)
        {
            width = refTexture.width;
            height = refTexture.height;

            pixels = new int[getPixels.Length];
            for (var i = 0; i < getPixels.Length; ++i) pixels[i] = ToHashCode(getPixels[i]);
        }

        public static SlicedTexture Slice(Texture2D texture)
        {
            var pixels = texture.GetPixels();
            var slicer = new TextureSlicer(texture, pixels);
            return slicer.Slice(pixels);
        }

        private int ToHashCode(Color color)
        {
            if ((int) (color.a * 255) == 0) return 0;

            const int a = 256 / 4;
            if (color.a > 0.95f) color.a = 1.0f;
            if (color.a < 0.05f) color.a = 0.0f;
            return (int) (color.a * a) * 255 * 255 * 255 + (int) (color.r * a) * 255 * 255 + (int) (color.g * a) * 255 +
                   (int) (color.b * a);
        }

        private static void CalcLine(ulong[] list, out int start, out int end)
        {
            start = 0;
            end = 0;
            var tmpStart = 0;
            var tmpEnd = 0;
            var tmpHash = list[0];
            for (var i = 0; i < list.Length; ++i)
                if (tmpHash == list[i])
                {
                    tmpEnd = i;
                }
                else
                {
                    if (end - start < tmpEnd - tmpStart)
                    {
                        start = tmpStart;
                        end = tmpEnd;
                    }

                    tmpStart = i;
                    tmpEnd = i;
                    tmpHash = list[i];
                }

            if (end - start < tmpEnd - tmpStart)
            {
                start = tmpStart;
                end = tmpEnd;
            }

            end -= SafetyMargin * 2 + Margin;
            if (end >= start) return;
            start = 0;
            end = 0;
        }

        private ulong[] CreateHashListX(int aMax, int bMax)
        {
            var hashList = new ulong[aMax];
            for (var a = 0; a < aMax; ++a)
            {
                ulong line = 0;
                for (var b = 0; b < bMax; ++b)
                    line = (ulong) (line + (ulong) (pixels[b * width + a] * b)).GetHashCode();
                hashList[a] = line;
            }

            return hashList;
        }

        private ulong[] CreateHashListY(int aMax, int bMax)
        {
            var hashList = new ulong[aMax];
            for (var a = 0; a < aMax; ++a)
            {
                ulong line = 0;
                for (var b = 0; b < bMax; ++b)
                    line = (ulong) (line + (ulong) (pixels[a * width + b] * b)).GetHashCode();
                hashList[a] = line;
            }

            return hashList;
        }

        private SlicedTexture Slice(Color[] originalPixels)
        {
            int xStart, xEnd;
            {
                var hashList = CreateHashListX(width, height);
                CalcLine(hashList, out xStart, out xEnd);
            }

            int yStart, yEnd;
            {
                var hashList = CreateHashListY(height, width);
                CalcLine(hashList, out yStart, out yEnd);
            }

            var skipX = false;
            if (xEnd - xStart < 2)
            {
                skipX = true;
                xStart = 0;
                xEnd = 0;
            }

            var skipY = false;
            if (yEnd - yStart < 2)
            {
                skipY = true;
                yStart = 0;
                yEnd = 0;
            }

            var output = GenerateSlicedTexture(xStart, xEnd, yStart, yEnd, originalPixels);
            var left = xStart + SafetyMargin;
            var bottom = yStart + SafetyMargin;
            var right = width - xEnd - SafetyMargin - Margin;
            var top = height - yEnd - SafetyMargin - Margin;
            if (skipX)
            {
                left = 0;
                right = 0;
            }

            if (skipY)
            {
                top = 0;
                bottom = 0;
            }

            return new SlicedTexture(output, new Boarder(left, bottom, right, top));
        }

        private Texture2D GenerateSlicedTexture(int xStart, int xEnd, int yStart, int yEnd, Color[] originalPixels)
        {
            var outputWidth = width - (xEnd - xStart);
            var outputHeight = height - (yEnd - yStart);
            var outputPixels = new Color[outputWidth * outputHeight];
            for (int x = 0, originalX = 0; x < outputWidth; ++x, ++originalX)
            {
                if (originalX == xStart) originalX += xEnd - xStart;
                for (int y = 0, originalY = 0; y < outputHeight; ++y, ++originalY)
                {
                    if (originalY == yStart) originalY += yEnd - yStart;
                    outputPixels[y * outputWidth + x] = originalPixels[originalY * width + originalX];
                }
            }

            var output = new Texture2D(outputWidth, outputHeight);
            output.SetPixels(outputPixels);
            return output;
        }

        private int Get(int x, int y)
        {
            return pixels[y * width + x];
        }
    }

    public class SlicedTexture
    {
        public SlicedTexture(Texture2D texture, Boarder boarder)
        {
            Texture = texture;
            Boarder = boarder;
        }

        public Texture2D Texture { get; }
        public Boarder Boarder { get; }
    }

    public class Boarder
    {
        public Boarder(int left, int bottom, int right, int top)
        {
            Left = left;
            Bottom = bottom;
            Right = right;
            Top = top;
        }

        public int Left { get; }
        public int Bottom { get; }
        public int Right { get; }
        public int Top { get; }

        public Vector4 ToVector4()
        {
            return new Vector4(Left, Bottom, Right, Top);
        }
    }
}