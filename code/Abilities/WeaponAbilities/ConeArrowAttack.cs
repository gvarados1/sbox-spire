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

		CreateProjectile( -15f );
		CreateProjectile( 0f );
		CreateProjectile( 15f );
	}

	protected void DrawLine( float yawOffset )
	{
		var character = GetCharacter();
		var pos = character.Position;
		var rot = character.EyeRotation;

		var spread = new Angles().WithYaw( yawOffset );
		var offsetRotation = Rotation.From( spread ) * rot;

		DebugOverlay.Line( pos, pos + offsetRotation.Forward * 10000f + Vector3.Up * 10f, Color.Orange );
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
