namespace Spire;

public partial class BaseCharacter
{
	[ClientRpc]
	public void CreateParticle( string path, bool follow )
	{
		if ( follow )
			Particles.Create( path, this, true );
		else
			Particles.Create( path, Position );
	}

	[ClientRpc]
	public void CreateParticle( string path, bool follow, string attachment )
	{
		Particles.Create( path, this, attachment, follow );
	}
}
