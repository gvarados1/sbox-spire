namespace Spire;

public partial class Util
{
	[ClientRpc]
	public static void CreateParticle( Entity entity, string path, bool follow )
	{
		if ( follow )
			Particles.Create( path, entity, true );
		else
			Particles.Create( path, entity.Position );
	}

	[ClientRpc]
	public static void CreateParticle( Entity entity, string path, bool follow, string attachment )
	{
		Particles.Create( path, entity, attachment, follow );
	}
}
