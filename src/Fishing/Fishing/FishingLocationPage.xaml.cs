using Fishing.Helpers;
using Fishing.Models;
using Fishing.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            this.BindingContext = viewModel;
        }

        SKBitmap backgroundBitmap;
        private FishingLocationviewModel viewModel;
        private int scrollPosition;
        private double lastX;
        private int lineLength = 300;

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


        private enum AnnotationDisplayType
        {
            Center,
            Left,
            Right
        };

        private AnnotationDisplayType GetAnnotationDisplayType(FishModel fish)
        {
            var markerPos = GetMarkerAbsolutePos(fish);

            if (markerPos.X < ScreenWindowRect.Left)
                return AnnotationDisplayType.Left;

            if (markerPos.X > ScreenWindowRect.Right)
                return AnnotationDisplayType.Right;

            return AnnotationDisplayType.Center;

        }


        private void DrawMarker(SKCanvas canvas, Models.FishModel fish)
        {

            // get the absolute position marker
            var markerLocation = GetMarkerAbsolutePos(fish);

            // work out based on the position what type of annotation (left, right, center)
            var annotationType = GetAnnotationDisplayType(fish);

            // work out the rect for where i should draw the annnotation
            SKRect annotationRect = GetAnotationScreenRect(fish);

            if (annotationType == AnnotationDisplayType.Center)
            {
                var markerPos = GetMarkerOnScreenPos(fish);
                // draw the marker
                canvas.DrawOval(markerPos, new SKSize(50, 20), markerRing);
                canvas.DrawOval(markerPos, new SKSize(25, 10), markerPaint);

                canvas.DrawLine(markerPos, new SKPoint(markerPos.X, markerPos.Y - lineLength),
                    linePaint);
            }

            // draw our bitmap at that rect
            canvas.DrawBitmap(fishAnnotations[fish].OnscreenAnnotation, annotationRect);


        }

        private SKPoint GetMarkerAbsolutePos(FishModel fish)
        {
            SKPoint markerPos = new SKPoint()
            {
                X = (float)(backgroundBitmap.Width * fish.AnchorX),
                Y = (float)(backgroundBitmap.Height * fish.AnchorY)
            };
            return markerPos;
        }

        private SKPoint GetMarkerOnScreenPos(FishModel fish)
        {
            var markerPos = GetMarkerAbsolutePos(fish);   
            markerPos.Offset(-ScrollPosition, 0);
            return markerPos;
        }

        private SKRect GetAnotationScreenRect(FishModel fish)
        {
            // get marker location
            var markerScreenPos = GetMarkerOnScreenPos(fish);

            // get annotation
            var annotation = fishAnnotations[fish].OnscreenAnnotation;

            var annotationType = GetAnnotationDisplayType(fish);

            switch (annotationType)
            {
                case AnnotationDisplayType.Center:
                    SKRect rect = new SKRect(0, 0, annotation.Width, annotation.Height);
                    rect.Location = markerScreenPos;
                    rect.Offset(-(annotation.Width / 2), -(annotation.Height / 2 + lineLength));
                    return rect;
                case AnnotationDisplayType.Left:
                    SKRect left = new SKRect(0, 0, annotation.Width, annotation.Height);
                    left.Offset(0, markerScreenPos.Y -(annotation.Height / 2 + lineLength));
                    return left;
                case AnnotationDisplayType.Right:
                    SKRect right = new SKRect(0, 0, annotation.Width, annotation.Height);
                    right.Offset(ScreenWindowRect.Width - annotation.Width, 
                        markerScreenPos.Y -(annotation.Height / 2 + lineLength));
                    return right;
            }
            return SKRect.Empty;
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

        public SKRect ScreenWindowRect 
        { 
            get
            {
                SKPoint screenPosition = new SKPoint(ScrollPosition, 0);
                return SKRect.Create(screenPosition, Panorama.CanvasSize);
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

        private void Panorama_Touch(object sender, SkiaSharp.Views.Forms.SKTouchEventArgs e)
        {
            switch (e.ActionType)
            {
                case SkiaSharp.Views.Forms.SKTouchAction.Entered:
                    break;
                case SkiaSharp.Views.Forms.SKTouchAction.Pressed:
                    // location
                    var touchPoint = e.Location;
                    bool clickedOnFish = false ;
                    foreach (var fish in fishAnnotations)
                    {
                        var onscreenRect = GetAnotationScreenRect(fish.Key);
                        if (onscreenRect.Contains(touchPoint))
                        {
                            clickedOnFish = true;
                            AnimateToFish(fish.Key);
                            break;
                        }
                    }
                    if (!clickedOnFish)
                        AnimateClosePopup();



                    break;
                case SkiaSharp.Views.Forms.SKTouchAction.Moved:
                    break;
                case SkiaSharp.Views.Forms.SKTouchAction.Released:
                    break;
                case SkiaSharp.Views.Forms.SKTouchAction.Cancelled:
                    break;
                case SkiaSharp.Views.Forms.SKTouchAction.Exited:
                    break;
                case SkiaSharp.Views.Forms.SKTouchAction.WheelChanged:
                    break;
            }
        }

        private void AnimateClosePopup()
        {
            Popup.TranslateTo(0, Popup.Height, 400, Easing.SpringIn);
        }

        private void AnimateToFish(FishModel fish)
        {
            var startPos = ScrollPosition;
            var endPos = GetMarkerAbsolutePos(fish);
            endPos.Offset(-ScreenWindowRect.Width / 2, 0);

            var movement = new Animation((v) => ScrollPosition = (int)v,
                startPos,
                endPos.X,
                Easing.SpringOut);

            var startPop = Popup.TranslationY;
            var endPop = 0;
            var popup = new Animation((v) => Popup.TranslationY = v, startPop,
                endPop, Easing.SpringOut);

            var parentAnimation = new Animation();
            parentAnimation.Add(0, 1, movement);
            parentAnimation.Add(0, 1, popup);
            parentAnimation.Commit(this, "AnimateToFish", 16, 400);
        }

        SKPaint barPaint = new SKPaint()
        {
            Color = Color.FromHex("#BFE05F").ToSKColor(),
            StrokeCap = SKStrokeCap.Round,
            StrokeWidth = 8,
            Style = SKPaintStyle.Stroke,
        };

        private void BiteTimeChart_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();


            var barSpacing = e.Info.Width / 24;
            Random rnd = new Random();

            for (int i = 1; i < 24; i++)
            {
                var barValue = rnd.NextDouble();
                SKPoint start = new SKPoint((i * barSpacing), e.Info.Height - 10);
                SKPoint end = new SKPoint((i * barSpacing), (float)(info.Height - (info.Height * barValue)));
                canvas.DrawLine(start, end, barPaint);
            }
        }
    }
}