using System;
using System.Collections.Generic;
using FFImageLoading;
using FFImageLoading.Work;
using UIKit;

namespace Xmf2.Core.ImgLoading.iOS
{
	public class UIRemoteImageView : UIImageView
	{
		private List<Action<TaskParameter>> _taskParamSetupChain;

		public TimeSpan CacheDuration { get; set; }

		public UIRemoteImageView()
		{
			_taskParamSetupChain = new List<Action<TaskParameter>>();
		}

		public UIRemoteImageView WithImageLoadingTaskParam(Action<TaskParameter> taskParamSetupAction)
		{
			_taskParamSetupChain.Add(taskParamSetupAction);
			return this;
		}

		/// <summary>
		/// Defines the placeholder used when an error occurs.
		/// </summary>
		/// <returns>The TaskParameter instance for chaining the call.</returns>
		/// <param name="filepath">Path to the file.</param>
		/// <param name="source">Source for the path: local, web, assets</param>
		public UIRemoteImageView WithErrorPlaceholder(string filepath, ImageSource source = ImageSource.CompiledResource)
			=> WithImageLoadingTaskParam((taskParam) => taskParam.ErrorPlaceholder(filepath, source));

		/// <summary>
		/// Defines the placeholder used while loading.
		/// </summary>
		/// <returns>The TaskParameter instance for chaining the call.</returns>
		/// <param name="path">Path to the file.</param>
		/// <param name="source">Source for the path: local, web, assets</param>
		public UIRemoteImageView WithLoadingPlaceholder(string path, ImageSource source = ImageSource.CompiledResource)
			=> WithImageLoadingTaskParam((taskParam) => taskParam.LoadingPlaceholder(path, source));

		/// <summary>
		/// Reduce memory usage by downsampling the image. Aspect ratio will be kept even if width/height values are incorrect.
		/// Uses pixels units for width/height
		/// </summary>
		/// <returns>The TaskParameter instance for chaining the call.</returns>
		/// <param name="width">Optional width parameter, if value is higher than zero it will try to downsample to this width while keeping aspect ratio.</param>
		/// <param name="height">Optional height parameter, if value is higher than zero it will try to downsample to this height while keeping aspect ratio.</param>
		/// <param name="allowUpscale">Whether to upscale the image if it is smaller than passed dimensions or not; if <c>null</c> the value is taken from Configuration (<c>false</c> by default)</param>
		public UIRemoteImageView WithDownSample(int width = 0, int height = 0, bool? allowUpscale = default(bool?))
			=> WithImageLoadingTaskParam(taskParam => taskParam.DownSample(width, height, allowUpscale));

		public virtual void LoadUrl(string url)
		{
			var taskParam = ImageService.Instance.LoadUrl(url, this.CacheDuration);
			AddTaskParamTransformation(taskParam);
			//taskParam.Transform(new GrayOverlayTransformation());

			//TODO: see in PR, ça serait parfait avec une transformation custom ajoutant un overlay gris...
			//... qui sait faire ça ?
			//taskParam.Transform(new GrayOverlayTransformation());
			taskParam.Into(this);
		}

		private void AddTaskParamTransformation(TaskParameter taskParameter)
		{
			_taskParamSetupChain.ForEach(transformation => transformation(taskParameter));
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_taskParamSetupChain?.Clear();
				_taskParamSetupChain = null;
			}
			base.Dispose(disposing);
		}
	}
}
