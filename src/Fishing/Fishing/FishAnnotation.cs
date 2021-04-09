using Fishing.Helpers;
using Fishing.Models;
using SkiaSharp;
using System;
using System.Threading.Tasks;

namespace Fishing
{
    internal class FishAnnotation
    {
        public SKBitmap OnscreenAnnotation { get; set; }

        int Width = 250;
        int Height = 125;
        int Padding = 25;
        int Radius = 25;

        SKPaint annotationBackground = new SKPaint()
        {
            Color = SKColors.White,
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true
        };


        internal async Task CreateAnnotations(FishModel fish)
        {
            // create a bitmap
            SKBitmap bitmap = new SKBitmap(Width + 2 * Padding, Height + 2 * Padding);
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                canvas.Clear();
                // draw a rounded rect
                SKRect outline = new SKRect(0, 0, bitmap.Width, bitmap.Height);
                outline.Inflate(-Padding, -Padding);
                canvas.DrawRoundRect(outline, Radius, Radius, annotationBackground);

                // draw a fish in the middle
                outline.Inflate(-Padding, -Padding);
                var fishImage = await BitmapHelper.LoadBitmapFromUrl(fish.Image);
                canvas.DrawBitmap(fishImage, outline);
            }
            OnscreenAnnotation = bitmap;
        }
    }
}