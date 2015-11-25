/*
Based On https://github.com/paulpatarinski/Xamarin.Forms.Plugins/blob/master/SVG/SVG/SVG.Forms.Plugin.Android/SvgImageRenderer.cs

The MIT License (MIT)

Copyright (c) 2014 Paul Patarinski 

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/ 
using System;
using Xamarin.Forms;
using ScoresApp.Controls;
using ScoresApp.Droid;
using Xamarin.Forms.Platform.Android.AppCompat;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using System.Threading.Tasks;
using NGraphics.Custom.Parsers;
using System.IO;
using System.Net.Http;
using NGraphics.Android.Custom;
using Android.Runtime;
using ScoresApp.Droid.Renderers;
using Android.Graphics.Drawables;
using Xamarin;

[assembly: ExportRenderer (typeof(SvgImage), typeof(SvgImageRenderer))]
namespace ScoresApp.Droid.Renderers
{
	[Preserve(AllMembers = true)]
	public class SvgImageRenderer : Xamarin.Forms.Platform.Android.ViewRenderer<SvgImage,ImageView>
	{
		public static void Init ()
		{
			var temp = DateTime.Now;
		}

		private SvgImage _formsControl {
			get {
				return Element as SvgImage;
			}
		}


		protected override void OnElementChanged(ElementChangedEventArgs<SvgImage> e)
		{
			base.OnElementChanged(e);

			if (_formsControl != null)
			{
				if (string.IsNullOrWhiteSpace (_formsControl.SvgPath))
					return;
				UpdateSVGSource ();
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (_formsControl != null)
			{
				if (e.PropertyName != SvgImage.SvgPathProperty.PropertyName)
					return;

				if (string.IsNullOrWhiteSpace (_formsControl.SvgPath))
					return;

				UpdateSVGSource ();
			}
		}

		async void UpdateSVGSource()
		{
			await Task.Run(async () =>
				{
					Uri u;
					try{
						u = new Uri (_formsControl.SvgPath);
					}catch(Exception e){
						Insights.Report(e);
						return null;
					}
					var path = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), Path.GetFileName (u.AbsolutePath));
					if (!File.Exists (path)) {
						using (var client = new HttpClient ()) {
							try{
								var bytes = await client.GetByteArrayAsync (_formsControl.SvgPath);
								File.WriteAllBytes (path, bytes);
							}catch(Exception e){
								Insights.Report(e);
								return null;
							}
						}
					}

					var svgStream = File.OpenRead(path);

					if (svgStream == null)
					{
						throw new Exception(string.Format("Error retrieving {0} make sure Build Action is Embedded Resource",
							_formsControl.SvgPath));
					}
					SvgReader r;
					try{
						r = new SvgReader(new StreamReader(svgStream), new StylesParser(new ValuesParser()), new ValuesParser());
					}catch(Exception e){
						Insights.Report(e);
						return null;
					}

					var graphics = r.Graphic;

					var width = PixelToDP((int)_formsControl.WidthRequest <= 0 ? 100 : (int)_formsControl.WidthRequest);
					var height = PixelToDP((int)_formsControl.HeightRequest <= 0 ? 100 : (int)_formsControl.HeightRequest);

					var scale = 1.0;

					if (height >= width)
					{
						scale = height / graphics.Size.Height;
					}
					else
					{
						scale = width / graphics.Size.Width;
					}

					try{
					var canvas = new AndroidPlatform().CreateImageCanvas(graphics.Size, scale);
					graphics.Draw(canvas);
						var image = (BitmapImage)canvas.GetImage();
						return image;
					}
					catch(Exception e){
						Insights.Report(e);
						return null;
					}
				}).ContinueWith(taskResult =>
					{
						Device.BeginInvokeOnMainThread(() =>
							{
								var imageView = new ImageView(Context);
								SetNativeControl(imageView);
								if(taskResult.Result == null){
									var id = getResourceId(_formsControl.SvgDefaultImage, "drawable", Context.PackageName);
									Control.SetImageResource(id);
									return;
								}

								imageView.SetScaleType(ImageView.ScaleType.FitXy);
								imageView.SetImageBitmap(taskResult.Result.Bitmap);
							});

					});
		}

		public int getResourceId(String pVariableName, String pResourcename, String pPackageName) 
		{ 
			try { 
				return Context.Resources.GetIdentifier(pVariableName, pResourcename, pPackageName);
			} catch (Exception e) {
				Insights.Report(e);
				return -1; 
			}  
		} 

		public override SizeRequest GetDesiredSize (int widthConstraint, int heightConstraint)
		{
			return new SizeRequest (new Size (_formsControl.WidthRequest, _formsControl.WidthRequest));
		}

		/// <summary>
		/// http://stackoverflow.com/questions/24465513/how-to-get-detect-screen-size-in-xamarin-forms
		/// </summary>
		/// <param name="pixel"></param>
		/// <returns></returns>
		private int PixelToDP(int pixel) {
			var scale =Resources.DisplayMetrics.Density;
			return (int) ((pixel * scale) + 0.5f);
		}
	}
}

