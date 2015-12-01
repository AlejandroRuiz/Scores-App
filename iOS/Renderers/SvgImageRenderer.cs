/*
Based On https://github.com/paulpatarinski/Xamarin.Forms.Plugins/blob/master/SVG/SVG/SVG.Forms.Plugin.iOS/SvgImageRenderer.cs

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
using ScoresApp.iOS.Renderers;
using Foundation;
using Xamarin.Forms.Platform.iOS;
using NGraphics.Custom.Parsers;
using System.IO;
using UIKit;
using NGraphics.iOS.Custom;
using System.Net.Http;

[assembly: ExportRenderer(typeof(SvgImage), typeof(SvgImageRenderer))]
namespace ScoresApp.iOS.Renderers
{
	/// <summary>
	/// SVG Renderer
	/// </summary>
	[Preserve(AllMembers = true)]
	public class SvgImageRenderer : ImageRenderer
	{
		/// <summary>
		///   Used for registration with dependency service
		/// </summary>
		public static void Init()
		{
			var temp = DateTime.Now;
		}

		private SvgImage _formsControl
		{
			get { return Element as SvgImage; }
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
			Control.ContentMode = UIViewContentMode.ScaleAspectFit;
			Uri u;
			try{
				u = new Uri (_formsControl.SvgPath);
			}catch{
				Control.Image = UIImage.FromBundle (_formsControl.SvgDefaultImage);
				return;
			}
			var path = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), Path.GetFileName (u.AbsolutePath));
			if (!File.Exists (path)) {
				using (var client = new HttpClient ()) {
					try{
					var bytes = await client.GetByteArrayAsync (_formsControl.SvgPath);
					File.WriteAllBytes (path, bytes);
					}catch{
						Control.Image = UIImage.FromBundle (_formsControl.SvgDefaultImage);
						return;
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
			}catch{
				Control.Image = UIImage.FromBundle (_formsControl.SvgDefaultImage);
				return;
			}

			var graphics = r.Graphic;

			var width = _formsControl.WidthRequest <= 0 ? 100 : _formsControl.WidthRequest;
			var height = _formsControl.HeightRequest <= 0 ? 100 : _formsControl.HeightRequest;

			var scale = 1.0;

			if (height >= width)
			{
				scale = height/graphics.Size.Height;
			}
			else
			{
				scale = width/graphics.Size.Width;
			}

			var scaleFactor = UIScreen.MainScreen.Scale;

			try{
				var canvas = new ApplePlatform().CreateImageCanvas(graphics.Size, scale*scaleFactor);
				graphics.Draw(canvas);
				var image = canvas.GetImage();

				var uiImage = image.GetUIImage();
				Control.Image = uiImage;
			}
			catch{
				Control.Image = UIImage.FromBundle (_formsControl.SvgDefaultImage);
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			if (_formsControl != null)
			{
				if (string.IsNullOrWhiteSpace (_formsControl.SvgPath))
					return;
				UpdateSVGSource ();
			}
		}
	}
}

