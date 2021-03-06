using Sandbox;
using Spire.UI;

namespace Spire;

public partial class BaseCharacter : AnimatedEntity
{
	[Net, Predicted]
	public BaseCarriable LastActiveChild { get; set; }

	[Net, Predicted]
	public BaseCarriable ActiveChild { get; set; }

	[Net, Predicted]
	public CharacterController Controller { get; set; }

	[Net, Predicted]
	protected PawnAnimator Animator { get; set; }

	public virtual PawnController ActiveController => Controller;
	public virtual PawnAnimator ActiveAnimator => Animator;
	public virtual float MaxHealth => 100f;

	public DamageInfo LastDamageInfo { get; protected set; }

	public ClothingContainer Clothing = new();

	public BaseNameplate Nameplate { get; set; }

	public BaseCharacter()
	{
		Tags.Add( "player" );
	}

	public override void Simulate( Client cl )
	{
		SimulateActiveChild( cl, ActiveChild );
		ActiveController?.Simulate( cl, this, ActiveAnimator );

		SimulateBuffs( cl );
	}

	public virtual void Respawn()
	{
		Host.AssertServer();

		SetModel( "models/citizen/citizen.vmdl" );

		Animator = new CharacterAnimator();

		LifeState = LifeState.Alive;
		Health = MaxHealth;
		Velocity = Vector3.Zero;

		Clothing.DressEntity( this );

		CreateHull();
		ResetInterpolation();

		// @TODO: Decide where to spawn
		Position = Vector3.Zero;
	}

	[ClientRpc]
	protected void RpcTakeDamage( Vector3 pos, float damage )
	{
		ClientTakeDamage( pos, damage );
	}

	protected virtual void ClientTakeDamage( Vector3 pos, float damage )
	{
		DamageIndicator.Create( pos, damage );
	}

	public override void TakeDamage( DamageInfo info )
	{
		base.TakeDamage( info );

		LastDamageInfo = info;

		if ( Host.IsServer )
			RpcTakeDamage( info.Position, info.Damage );
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

		LastActiveChild.Simulate( client );
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

	protected override void OnDestroy()
	{
		base.OnDestroy();

		if ( Host.IsClient )
			Nameplate?.Delete();
	}

	public override void OnKilled()
	{
		base.OnKilled();

		BecomeRagdollOnClient(
			Velocity,
			LastDamageInfo.Flags,
			LastDamageInfo.Position,
			LastDamageInfo.Force,
			GetHitboxBone( LastDamageInfo.HitboxIndex ) );
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
