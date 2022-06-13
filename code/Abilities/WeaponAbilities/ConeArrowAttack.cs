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

	public override bool TickGuide()
	{
		DrawLine( -15f );
		DrawLine( 0f );
		DrawLine( 15f );

		return true;
	}
}
