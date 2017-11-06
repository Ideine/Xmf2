namespace Xmf2.Commons.Droid.Helpers.CustomAnimations
{
	public interface IRevealViewGroup
	{
		ViewRevealManager ViewRevealManager { get; }

		void ForceDraw();
	}
}