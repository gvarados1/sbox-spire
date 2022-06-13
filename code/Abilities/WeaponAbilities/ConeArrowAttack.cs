namespace Spire.Abilities;

public partial class ConeArrowAttack : BasicArrowAttack
{
	// Configuration
	public override string Identifier => "cone_arrow_attack";
	public override WeaponAbilityType Type => WeaponAbilityType.Special;

	protected override void PostRun()
	{
		base.PostRun();

		if ( Host.IsClient )
			return;

		CreateProjectile( -25 );
		CreateProjectile( 0f );
		CreateProjectile( 25 );
	}

	public override bool TickGuide( AbilityGuideEntity entity )
	{
		var character = GetCharacter();

		entity.SetParticle( "particles/widgets/cone/widget_cone_base_45.vpcf" );
		entity.Position = character.Position;
		entity.Rotation = character.EyeRotation;

		return true;
	}
}
