using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content.PM;

namespace Xmf2.Core.Droid.Permissions
{
	public static class PermissionContainer
	{
		private static readonly Dictionary<int, TaskCompletionSource<Permission[]>> _permissions = new Dictionary<int, TaskCompletionSource<Permission[]>>();

		public static Task<Permission[]> WaitForPermission(int code)
		{
			var result = new TaskCompletionSource<Permission[]>();

			if (_permissions.TryAdd(code, result))
			{
				return result.Task;
			}
			if (_permissions.TryGetValue(code, out result))
			{
				return result.Task;
			}
			return null;
		}

		public static void OnResult(int requestCode, Permission[] grantResults)
		{
			if (_permissions.TryGetValue(requestCode, out var result))
			{
				result.TrySetResult(grantResults);
				_permissions.Remove(requestCode);
			}
		}
	}
}
