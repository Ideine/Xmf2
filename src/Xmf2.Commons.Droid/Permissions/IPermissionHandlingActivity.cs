using System;
using System.Threading.Tasks;
using Android.Content.PM;

namespace Xmf2.Commons.Droid.Permissions
{
	public interface IPermissionHandlingActivity
	{
		Task<Permission[]> WaitForPermission(int code);
	}
}
