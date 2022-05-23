using Sandbox;

namespace Rising;

public partial class BaseCharacter : AnimatedEntity
{

	[Net, Predicted]
	public BaseCarriable LastActiveChild { get; set; }

	[Net, Predicted]
	public BaseCarriable ActiveChild { get; set; }

	[Net, Predicted]
	public PawnController Controller { get; set; }

	[Net, Predicted]
	protected PawnAnimator Animator { get; set; }

	public virtual PawnController ActiveController => Controller;
	public virtual PawnAnimator ActiveAnimator => Animator;
	public virtual float MaxHealth => 100f;

	public DamageInfo LastDamageInfo { get; protected set; }

	public override void Simulate( Client cl )
	{
		SimulateActiveChild( cl, ActiveChild );
		ActiveController?.Simulate( cl, this, ActiveAnimator );
	}

	public virtual void Respawn()
	{
		Host.AssertServer();

		SetModel( "models/citizen/citizen.vmdl" );

		Animator = new StandardPlayerAnimator();

		LifeState = LifeState.Alive;
		Health = MaxHealth;
		Velocity = Vector3.Zero;

		CreateHull();
		ResetInterpolation();

		// @TODO: Decide where to spawn
		Position = Vector3.Zero;
	}

	public override void TakeDamage( DamageInfo info )
	{
		base.TakeDamage( info );

		LastDamageInfo = info;
	}

	public virtual void SimulateActiveChild( Client client, BaseCarriable child )
	{
		if ( LastActiveChild != child )
		{
			OnActiveChildChanged( LastActiveChild, child );
			LastActiveChild = child;
		}

		if ( !LastActiveChild.IsValid() )
			return;

		if ( LastActiveChild.IsAuthority )
		{
			LastActiveChild.Simulate( client );
		}
	}

	public virtual void OnActiveChildChanged( BaseCarriable previous, BaseCarriable next )
	{
		previous?.ActiveEnd( this, previous.Owner != this );
		next?.ActiveStart( this );
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		ActiveController?.FrameSimulate( cl, this, ActiveAnimator );
	}

	public virtual void CreateHull()
	{
		CollisionGroup = CollisionGroup.Player;
		AddCollisionLayer( CollisionLayer.Player );
		SetupPhysicsFromAABB( PhysicsMotionType.Keyframed, new Vector3( -16, -16, 0 ), new Vector3( 16, 16, 72 ) );

		MoveType = MoveType.MOVETYPE_WALK;
		EnableHitboxes = true;
	}
}
