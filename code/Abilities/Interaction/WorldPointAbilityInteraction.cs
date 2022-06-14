namespace Spire.Abilities;

public partial class WorldPointAbilityInteraction : AbilityInteraction
{
	public override void OnTick()
	{
		WorldCursorPosition = GetWorldCursor();

		base.OnTick();

		if ( Input.Pressed( InputButton.PrimaryAttack ) )
		{
			TryEnd();
		}
		else if ( Input.Pressed( InputButton.SecondaryAttack ) )
		{
			Cancel();
		}
	}

	protected bool IsInRange()
	{
		var range = Ability.Data.AbilityRange;
		var entity = Ability.Entity;
		var dist = entity.Position.Distance( WorldCursorPosition );

		return dist <= range.Max && dist >= range.Min;
	}

	protected void TryEnd()
	{
		if ( !IsInRange() )
		{
			Cancel();
			return;
		}

		End();
	}

	protected override void OnCancel()
	{
		base.OnCancel();

		if ( AreaEntity.IsValid() )
			AreaEntity.Delete();
	}

	protected Vector3 AvailableCPColor => new Vector3( Color.Green );
	protected Vector3 UnavailableCPColor => new Vector3( Color.Red );

	public static AbilityGuideEntity AreaEntity;
	protected override void TickGuide( AbilityGuideEntity entity )
	{

		if ( !AreaEntity.IsValid() )
		{
			AreaEntity = new();
			AreaEntity.SetParticle( "particles/widgets/widget_area.vpcf" );
		}

		AreaEntity.Position = Ability.GetCharacter().Position;
		AreaEntity.Particle.SetPosition( 4, new Vector3().WithX( Ability.Data.AbilityRange.Max ) );

		entity.Position = WorldCursorPosition + Vector3.Up * 10f;
		entity.Particle.SetPosition( 2, IsInRange() ? AvailableCPColor : UnavailableCPColor );
		entity.Particle.SetPosition( 4, new Vector3().WithX( Ability.Data.AbilityEffectRadius ) );
	}

	protected override void OnEnd()
	{
		base.OnEnd();

		AreaEntity.Delete();

		Ability.Run();
	}
}
