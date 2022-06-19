namespace Spire.Abilities;

public partial class WorldPointTestAbility : PlayerAbility
{
	// Configuration
	public override string Identifier => "world_point";
	public override PlayerAbilityType Type => PlayerAbilityType.Standard;

	public virtual float ProjectileSpeed => 800f;
	public virtual float ProjectileRadius => 10f;
	public virtual float ProjectileThrowStrength => 100f;
	public virtual bool ManualProjectile => false;

	protected virtual void Teleport( Vector3 pos )
	{
		if ( Host.IsClient ) return;

		GetCharacter().Position = pos;
	}

	protected override void PostRun()
	{
		base.PostRun();

		if ( Host.IsClient )
			return;

		var interaction = Interaction as WorldPointAbilityInteraction;

		Teleport( interaction.WorldCursorPosition );
	}
}
