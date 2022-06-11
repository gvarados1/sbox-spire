namespace Spire.Abilities;

/// <summary>
/// Configurable Game Data for Spire Abilities
/// </summary>
[GameResource( "Spire Ability Definition", "ability", "An ability", Icon = "star" )]
public partial class AbilityGameResource : GameResource
{
	public struct SoundEntry
	{
		public string Tag { get; set; }

//		I want to be able to do this, but I can't. This is on purpose.
//		[ResourceType( "sound" )]
		public string Sound { get; set; }

		public string Attachment { get; set; }

		public override string ToString() => $"Sound";
	}

	public struct ParticleEntry
	{
		public string Tag { get; set; }

		[ResourceType( "vpcf" )]
		public string Particle { get; set; }

		public bool FromCharacter { get; set; }

		public string Attachment { get; set; }

		public override string ToString() => $"Particle";
	}

	public static List<AbilityGameResource> All = new();
	public static AbilityGameResource TryGet( string id ) => All.Where( x => x.Name == id ).FirstOrDefault();

	/// <summary>
	/// The ability's cooldown until you can run it again. This is assigned after Ability.PostRun
	/// </summary>
	public float Cooldown { get; set; } = 5f;

	/// <summary>
	/// An identifier for this ability. Used for Ability GameResources
	/// </summary>
	public string AbilityID { get; set; } = "ability";

	/// <summary>
	/// A friendly name for the ability
	/// </summary>
	public virtual string AbilityName { get; set; } = "Ability";

	/// <summary>
	/// A short description of an ability
	/// </summary>
	public virtual string Description { get; set; } = "This ability does nothing.";

	/// <summary>
	/// The ability's icon used in the game's user interface
	/// </summary>
	[ResourceType( "jpg" )]
	public virtual string Icon { get; set; } = "";

	/// <summary>
	/// Apply a speed modifier to the player while the ability is in progress
	/// </summary>
	public virtual float PlayerSpeedScale { get; set; } = 1f;

	/// <summary>
	/// The duration of an ability.
	/// </summary>
	public virtual float Duration { get; set; } = 0f;

	[Title( "Sound List" )]
	public List<SoundEntry> Sounds { get; set; }

	public List<SoundEntry> SoundsWithTag( string tag )
	{
		var list = new List<SoundEntry>();

		if ( Sounds is not null )
		{
			list.AddRange( Sounds.Where( x => x.Tag == tag ) );
		}

		list.ForEach( x => Log.Info( x.Sound ) );

		return list;
	}

	[Title( "Particle List" )]
	public List<ParticleEntry> Particles { get; set; }

	public bool RunDefaultPlayerAnimation { get; set; } = true;

	public List<ParticleEntry> ParticlesWithTag( string tag )
	{
		var list = new List<ParticleEntry>();

		if ( Particles is not null )
			list.AddRange( Particles.Where( x => x.Tag == tag ) );

		return list;
	}

	protected override void PostLoad()
	{
		base.PostLoad();

		if ( !All.Contains( this ) )
			All.Add( this );
	}
}
