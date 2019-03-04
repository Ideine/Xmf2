using System.Threading.Tasks;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Navigations
{
	public delegate Task<IComponentViewModel> ViewModelCreator(string route);
}