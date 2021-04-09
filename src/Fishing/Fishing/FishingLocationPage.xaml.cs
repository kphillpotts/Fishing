using Fishing.Helpers;
using Fishing.Models;
using Fishing.ViewModels;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingLocationPage : ContentPage
    {
        public FishingLocationPage(ViewModels.FishingLocationviewModel fishingLocationviewModel)
        {
            InitializeComponent();
            this.viewModel = fishingLocationviewModel;
        }

        SKBitmap backgroundBitmap;
        private FishingLocationviewModel viewModel;
        private int scrollPosition;
        private double lastX;

        Dictionary<FishModel, FishAnnotation> fishAnnotations = new Dictionary<FishModel, FishAnnotation>();

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // load our background image
            var imageUrl = viewModel.Location.PanoramaImage;
            var originalBitmap = await BitmapHelper.LoadBitmapFromUrl(imageUrl);

            // resize the image for the screen
            var imageScale = Panorama.CanvasSize.Height / originalBitmap.Height;
            SKSizeI newImageSize = new SKSizeI((int)(originalBitmap.Width * imageScale),
                (int)(originalBitmap.Height * imageScale));
            backgroundBitmap = originalBitmap.Resize(newImageSize, SKFilterQuality.High);

            foreach (var fish in viewModel.Location.Fish)
            {
                FishAnnotation annotation = await CreateAnnotations(fish);
                fishAnnotations.Add(fish, annotation);
            }


            Panorama.InvalidateSurface();
        }

        private async Task<FishAnnotation> CreateAnnotations(FishModel fish)
        {
            var annotation = new FishAnnotation();
            // create an image for the fish
            await annotation.CreateAnnotations(fish); 
            return annotation;
        }

        private void SKCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = e.Surface.Canvas;

            if (backgroundBitmap == null) return;

            canvas.Clear();
            SKRect source = new SKRect(ScrollPosition, 0, scrollPosition + info.Width, info.Height);
            SKRect dest = info.Rect;
            canvas.DrawBitmap(backgroundBitmap, source, dest);

            foreach (var fish in viewModel.Location.Fish)
            {
                DrawMarker(canvas, fish);
            }


        }

        SKPaint markerRing = new SKPaint()
        {
            Color = SKColors.White.WithAlpha(0x50),
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true
        };

        SKPaint markerPaint = new SKPaint()
        {
            Color = SKColors.White,
            IsAntialias = true,
            Style = SKPaintStyle.StrokeAndFill,
        };

        SKPaint linePaint = new SKPaint()
        {
            Color = SKColors.White,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 4
        };



        private void DrawMarker(SKCanvas canvas, Models.FishModel fish)
        {
            // position of the marker
            SKPoint markerPos = new SKPoint()
            {
                X = (float)(backgroundBitmap.Width * fish.AnchorX),
                Y = (float)(backgroundBitmap.Height * fish.AnchorY)
            };
            markerPos.Offset(-ScrollPosition, 0);

            // draw the marker
            canvas.DrawOval(markerPos, new SKSize(50, 20), markerRing);
            canvas.DrawOval(markerPos, new SKSize(25, 10), markerPaint);
            int lineLength = 300;
            canvas.DrawLine(markerPos, new SKPoint(markerPos.X, markerPos.Y - lineLength),
                linePaint);

            // draw the fish
            if (fishAnnotations.ContainsKey(fish))
            {
                var annotation = fishAnnotations[fish].OnscreenAnnotation;
                SKPoint fishPos = markerPos;
                fishPos.Offset(-(annotation.Width / 2), -(annotation.Height/2 + lineLength));
                canvas.DrawBitmap(annotation, fishPos);
            }

        }

        public int ScrollPosition 
        { 
            get => scrollPosition; 
            set
            {
                // math.clamp
                var clamped = Math.Min(backgroundBitmap.Width - Panorama.CanvasSize.Width,
                    Math.Max(0, value));

                scrollPosition = (int)clamped;
                Panorama.InvalidateSurface();
            }
        }


        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    lastX = 0;
                    break;
                case GestureStatus.Running:
                    // work out how far we are panning
                    var xOffset = e.TotalX * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
                    var deltaX = lastX - xOffset;
                    lastX = xOffset;
                    ScrollPosition = (int)(ScrollPosition + deltaX);
                    break;
                case GestureStatus.Completed:
                    break;
                case GestureStatus.Canceled:
                    break;
            }
        }
    }
}