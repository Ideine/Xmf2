using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using AndroidX.Core.Content;
using Plugin.CurrentActivity;
using JavaFile = Java.IO.File;

namespace Xmf2.Core.Droid.Services
{
	public interface IPdfService
	{
		/**
		 * Store data on cache directory and open pdf externally.
		 * If the device do not have pdf application, the method throw an ActivityNotFoundException.
		 * If a pdf already exist with the same title, it is overwritten
		 *
		 * AndroidManifest must register a FileProvider on the name : $"{currentActivity.ApplicationContext!.PackageName}.fileprovider"
		 *
		 * <seealso cref="ActivityNotFoundException"/>
		 * <seealso cref="FileProvider"/>
		 */
		Task OpenPdf(string title, byte[] data);

		/**
		 * Store data on cache directory and open pdf externally.
		 * If the device do not have pdf application, the method throw an ActivityNotFoundException.
		 * If a pdf already exist with the same title, it is overwritten
		 *
		 * AndroidManifest must register a FileProvider on the name : $"{currentActivity.ApplicationContext!.PackageName}.fileprovider"
		 *
		 * <seealso cref="ActivityNotFoundException"/>
		 * <seealso cref="FileProvider"/>
		 */
		Task OpenPdf(string title, Stream data);
	}

	public class PdfService : IPdfService
	{
		public async Task OpenPdf(string title, byte[] data)
		{
			string sanitizedTitle = SanitizeTitle(title);
			string filePath = await SavePdfLocally(data, sanitizedTitle);
			StartActivity(filePath);
		}

		public async Task OpenPdf(string title, Stream data)
		{
			string sanitizedTitle = SanitizeTitle(title);
			string filePath = await SavePdfLocally(data, sanitizedTitle);
			StartActivity(filePath);
		}

		private string SanitizeTitle(string title)
		{
			string sanitizedTitle = title.Replace("/", string.Empty);
			if (!sanitizedTitle.EndsWith(".pdf"))
			{
				sanitizedTitle = $"{sanitizedTitle}.pdf";
			}

			return sanitizedTitle;
		}

		private void StartActivity(string filePath)
		{
			Activity currentActivity = CrossCurrentActivity.Current.Activity;

			JavaFile javaFile = new(filePath);
			Uri uri = FileProvider.GetUriForFile(currentActivity, $"{currentActivity.ApplicationContext!.PackageName}.fileprovider", javaFile);
			Intent intent = new(Intent.ActionView);
			intent.SetDataAndType(uri, "application/pdf");
			intent.AddFlags(ActivityFlags.NoHistory);
			intent.AddFlags(ActivityFlags.GrantReadUriPermission);

			currentActivity.StartActivity(intent);
		}

		private static async Task<string> SavePdfLocally(Stream data, string filename)
		{
			Activity currentActivity = CrossCurrentActivity.Current.Activity;
			string rootDirPath = currentActivity.CacheDir!.AbsolutePath;
			string filePath = Path.Combine(rootDirPath, filename);
			await using FileStream writer = File.Create(filePath);
			await data.CopyToAsync(writer);
			return filePath;
		}

		private static async Task<string> SavePdfLocally(byte[] data, string filename)
		{
			Activity currentActivity = CrossCurrentActivity.Current.Activity;
			string rootDirPath = currentActivity.CacheDir!.AbsolutePath;
			string filePath = Path.Combine(rootDirPath, filename);
			await using FileStream writer = File.Create(filePath);
			await writer.WriteAsync(data, 0, data.Length);
			return filePath;
		}
	}
}