using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Widget;
using Loscate.App.Droid;
using Loscate.App.Map;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Maps;
using Loscate.App.Services;
using Loscate.App.Droid.Map;
using Pin = Xamarin.Forms.Maps.Pin;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Loscate.App.Droid
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        public List<CustomPin> customPins;
        private MapService mapService;

        public CustomMapRenderer(Context context) : base(context)
        {
            mapService = (MapService)DependencyService.Get<IMapService>();
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);



            var bitmap = BitmapFactory.DecodeStream(new MemoryStream(Convert.FromBase64String((((CustomPin)pin).Photo))));
            var scaleBitmap = Bitmap.CreateScaledBitmap(bitmap, 150, 150, false);
            marker.SetIcon(BitmapDescriptorFactory.FromBitmap(GetBitmapClippedCircle(scaleBitmap)));
            return marker;
        }

        public static Bitmap GetBitmapClippedCircle(Bitmap bitmap)
        {

            int width = bitmap.Width;
            int height = bitmap.Height;
            Bitmap outputBitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);

            Android.Graphics.Path path = new Android.Graphics.Path();
            path.AddCircle(
                      (float)(width / 2)
                    , (float)(height / 2)
                    , (float)Math.Min(width, (height / 2))
                    , Android.Graphics.Path.Direction.Ccw);

            Canvas canvas = new Canvas(outputBitmap);
            canvas.ClipPath(path);
            canvas.DrawBitmap(bitmap, 0, 0, null);
            return outputBitmap;
        }

        private Bitmap DecodeImage(byte[] img)
        {
            return BitmapFactory.DecodeByteArray(img, 0, img.Length);
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = DecodeImage(imageBytes);
                }
            }

            return imageBitmap;
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            mapService.Invoke(customPin);
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }

                view = inflater.Inflate(Resource.Layout.XamarinMapInfoWindow, null);


                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);

                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }

                return view;
            }
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }

    }
}