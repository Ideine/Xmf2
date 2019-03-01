using System;
using System.Collections.Generic;
using Android.Support.V4.App;

namespace Xmf2.Components.Droid.Services
{
	public interface IFragmentResolverService
	{
		Type GetType(string code);
		string GetCode(Type type);

		void RegisterFragment<TFragment>(string code) where TFragment : Fragment;
	}

	public class FragmentResolverService : IFragmentResolverService
	{
		public const string ENTRY_POINT = "FRAGMENT_ENTRY_POINT";
		public const string LIST_ENTRY_POINT = "LIST_FRAGMENT_ENTRY_POINT";

		private readonly Dictionary<string, Type> _typeContainer = new Dictionary<string, Type>();
		private readonly Dictionary<Type, string> _codeContainer = new Dictionary<Type, string>();

		public Type GetType(string code)
		{
			return _typeContainer[code];
		}

		public string GetCode(Type type)
		{
			return _codeContainer[type];
		}

		public void RegisterFragment<TFragment>(string code)
			where TFragment : Fragment
		{
			_typeContainer[code] = typeof(TFragment);
			_codeContainer[typeof(TFragment)] = code;
		}
	}
}