using SandboxEditor;

namespace Spire;

[HammerEntity]
[Solid]
[Title( "Hurt Trigger" ), Category( "Triggers" ), Icon( "personal_injury" )]
public partial class GenericHurtEntity : BaseTrigger
{
	[Property, Category( "Trigger" ), Description( "How long this Hurt Entity will exist before being destroyed. Set to 0 for indefinitely (good for maps)." )]
	public float Lifetime { get; set; } = 0f;

	[Property, Category( "Trigger" ), ResourceType( "vpcf" ), Description( "The particle emitted from the character every time they are hurt." )]
	public string HurtParticle { get; set; } = "";

	[Property, Category( "Trigger" )]
	public string HurtSound { get; set; } = "";

	[Property, Category( "Trigger" ), Description( "How often (in seconds) entities inside the trigger will be hurt." )]
	public float HurtInterval { get; set; } = 1f;

	[Property( "damage", Title = "Damage" ), Category( "Trigger" )]
	public float Damage { get; set; } = 10.0f;

	protected Output OnHurt { get; set; }

	public TimeUntil UntilTick { get; set; }
	public TimeUntil UntilDestroy { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		UntilTick = HurtInterval;
	}

	protected void PerformEffects( Entity entity )
	{
		if ( !string.IsNullOrEmpty( HurtParticle ) )
			Util.CreateParticle( entity, HurtParticle, true );

		if ( !string.IsNullOrEmpty( HurtSound ) )
			PlaySound( HurtSound );
	}

	[Event.Tick.Server]
	protected virtual void Tick()
	{
		if ( !Enabled || !UntilTick )
			return;

		UntilTick = HurtInterval;

		if ( Lifetime > 0 && UntilDestroy )
			Delete();

		foreach ( var entity in TouchingEntities )
		{
			if ( !entity.IsValid() )
				continue;

			entity.TakeDamage( DamageInfo.Generic( Damage ).WithAttacker( this ).WithPosition( entity.Position ) );

			PerformEffects( entity );

			OnHurt.Fire( entity );
		}
	}
}
