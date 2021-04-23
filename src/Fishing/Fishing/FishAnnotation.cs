using Fishing.Helpers;
using Fishing.Models;
using SkiaSharp;
using System;
using System.Reflection;
using System.Threading.Tasks;


namespace Fishing
{
    internal class FishAnnotation
    {
        public SKBitmap OnscreenAnnotation { get; set; }
        public SKBitmap LeftAnnotation { get; set; }
        public SKBitmap RightAnnotation { get; set; }

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


        public async Task CreateLeftAnnotation (FishModel fish)
        {
            SKBitmap bitmap = await CreateBaseAnnotation(fish);

            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                // draw the arrow
                SKPath path = new SKPath();
                var halfHeight = (float)(bitmap.Height * .5);
                path.MoveTo(0, halfHeight);
                path.LineTo(Padding, halfHeight - Padding);
                path.LineTo(Padding, halfHeight + Padding);
                path.LineTo(0, halfHeight);
                path.Close();
                canvas.DrawPath(path, annotationBackground);
            }

            LeftAnnotation = bitmap;
        }

        SKPaint headerPaint = new SKPaint()
        {
            Color = SKColors.Black,
            IsAntialias = true,
            TextSize = 40f,
            Typeface = GetTypeface("OpenSans-Bold.ttf")
        };
        SKPaint bodyPaint = new SKPaint()
        {
            Color = SKColors.Black,
            IsAntialias = true,
            TextSize = 30f,
            Typeface = GetTypeface("OpenSans-Regular.ttf")
        };

        SKPaint debugPaint = new SKPaint()
        {
            Color = SKColors.Red,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1,
        };

        public static SKTypeface GetTypeface(string fullFontName)
        {
            SKTypeface result;

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("Fishing.Fonts." + fullFontName);
            if (stream == null)
                return null;

            result = SKTypeface.FromStream(stream);
            return result;
        }

        public async Task CreateOnScreenAnnotation(FishModel fish)
        {
            int bitmapHeight = Height + (2 * Padding);

            // measure header
            string headerText = fish.Name;
            SKRect headerBounds = new SKRect();
            headerPaint.MeasureText(headerText, ref headerBounds);

            string bodyText = fish.FishSize;
            SKRect bodyBounds = new SKRect();
            bodyPaint.MeasureText(bodyText, ref bodyBounds);

            // position our fish
            SKRect fishBounds = new SKRect(0, 0, (Height - Padding * 2), (Height - Padding * 2));
            fishBounds.Offset(2 * Padding, 2 * Padding);

            // position the text
            var lineSpacing = 10;
            var totalLineHeight = headerBounds.Height + lineSpacing + bodyBounds.Height;
            var topSpacing = (bitmapHeight - totalLineHeight) / 2;

            headerBounds.Location = new SKPoint(fishBounds.Right + Padding, topSpacing);
            bodyBounds.Location = new SKPoint(fishBounds.Right + Padding, headerBounds.Bottom + lineSpacing);

            var bitmapWidth = headerBounds.Width + fishBounds.Width + (5 * Padding);

            SKBitmap bitmap = new SKBitmap((int)bitmapWidth, bitmapHeight);
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                canvas.Clear();
                // draw a rounded rect
                SKRect outline = new SKRect(0, 0, bitmap.Width, bitmap.Height);
                outline.Inflate(-Padding, -Padding);
                canvas.DrawRoundRect(outline, Radius, Radius, annotationBackground);

                // draw our fish
                var fishImage = await BitmapHelper.LoadBitmapFromUrl(fish.Image);
                //canvas.DrawBitmap(fishImage, fishBounds);
                canvas.DrawBitmap(fishImage, fishBounds, BitmapStretch.AspectFit, BitmapAlignment.Center, BitmapAlignment.Center);
                
                // draw our text
                canvas.DrawText(headerText, headerBounds.Left, headerBounds.Bottom, headerPaint);
                canvas.DrawText(bodyText, bodyBounds.Left, bodyBounds.Bottom, bodyPaint);
                //canvas.DrawRect(headerBounds, debugPaint);
                //canvas.DrawRect(bodyBounds, debugPaint);
                // draw an arrow
                SKPath path = new SKPath();
                var halfWidth = (float)(bitmap.Width * .5);
                path.MoveTo(halfWidth, bitmap.Height);
                path.LineTo(halfWidth - Padding, bitmap.Height - Padding);
                path.LineTo(halfWidth + Padding, bitmap.Height - Padding);
                path.LineTo(halfWidth, bitmap.Height);
                path.Close();
                canvas.DrawPath(path, annotationBackground);
            }

            OnscreenAnnotation = bitmap;
        }

        public async Task CreateRightAnnotation(FishModel fish)
        {
            SKBitmap bitmap = await CreateBaseAnnotation(fish);

            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                // draw the arrow
                SKPath path = new SKPath();
                var halfHeight = (float)(bitmap.Height * .5);
                path.MoveTo(bitmap.Width, halfHeight);
                path.LineTo(bitmap.Width - Padding, halfHeight - Padding);
                path.LineTo(bitmap.Width - Padding, halfHeight + Padding);
                path.LineTo(bitmap.Width, halfHeight);
                path.Close();
                canvas.DrawPath(path, annotationBackground);
            }

            RightAnnotation = bitmap;
        }

        internal async Task<SKBitmap> CreateBaseAnnotation(FishModel fish)
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
                //canvas.DrawBitmap(fishImage, outline);
                canvas.DrawBitmap(fishImage, outline, BitmapStretch.AspectFit, BitmapAlignment.Center, BitmapAlignment.Center);

            }
            return bitmap;
        }
    }
}