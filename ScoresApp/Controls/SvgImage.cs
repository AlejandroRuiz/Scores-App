/*
Based On https://github.com/paulpatarinski/Xamarin.Forms.Plugins/blob/master/SVG/SVG/SVG.Forms.Plugin.Abstractions/SvgImage.cs

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
using System.Reflection;
using ScoresApp.Defaults;

namespace ScoresApp.Controls
{
	public class SvgImage : Image
	{
		/// <summary>
		/// The path to the svg file
		/// </summary>
		public static readonly BindableProperty SvgPathProperty =
			BindableProperty.Create("SvgPath", typeof(string), typeof(SvgImage), default(string));

		/// <summary>
		/// The path to the svg file
		/// </summary>
		public string SvgPath
		{
			get { return (string)GetValue(SvgPathProperty); }
			set { SetValue(SvgPathProperty, value); }
		}

		/// <summary>
		/// The assembly containing the svg file
		/// </summary>
		public static readonly BindableProperty SvgAssemblyProperty =
			BindableProperty.Create("SvgAssembly", typeof(Assembly), typeof(SvgImage), default(Assembly));

		/// <summary>
		/// The assembly containing the svg file
		/// </summary>
		public Assembly SvgAssembly
		{
			get { return (Assembly)GetValue(SvgAssemblyProperty); }
			set { SetValue(SvgAssemblyProperty, value); }
		}

		public static readonly BindableProperty SvgDefaultImageProperty =
			BindableProperty.Create("SvgDefaultImage", typeof(string), typeof(SvgImage), Strings.AppLogoIcon);
		
		public string SvgDefaultImage
		{
			get { return (string)GetValue(SvgDefaultImageProperty); }
			set { SetValue(SvgDefaultImageProperty, value); }
		}
	}
}

