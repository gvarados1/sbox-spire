namespace Spire;

public partial class BaseCarriable : Sandbox.BaseCarriable
{
	public virtual string ModelPath => "";

	public override void Spawn()
	{
		base.Spawn();

		if ( !string.IsNullOrEmpty( ModelPath ) )
		{
			SetModel( ModelPath );
		}
	}
}
