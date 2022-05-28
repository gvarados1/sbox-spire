using Spire.UI;

namespace Spire;

public partial class PlayerCharacter : BaseCharacter
{
	[Net]
	public TimeSince TimeSinceDied { get; set; }

	[Net]
	public PawnController DevController { get; set; }

	[Net, Predicted]
	public PlayerHotbar Hotbar { get; set; }

	public override PawnController ActiveController => DevController ?? base.ActiveController;
	public virtual float RespawnTime => 1;

	public virtual CameraMode Camera
	{
		get => Components.Get<CameraMode>();
		set
		{
			var current = Camera;
			if ( current == value ) return;

			Components.RemoveAny<CameraMode>();
			Components.Add( value );
		}
	}

	public PlayerCharacter()
	{
		if ( Host.IsClient )
			Nameplate = new PlayerNameplate( this );
	}

	public PlayerCharacter( Client cl ) : this()
	{
		// Load clothing from client data
		Clothing.LoadFromClient( cl );
	}

	public override void Respawn()
	{
		Camera = new PlayerCamera();
		Controller = new CharacterController();
		Hotbar = new PlayerHotbar( this );

		Hotbar.SetCurrent( new SwordWeapon() );

		base.Respawn();
	}

	public override void BuildInput( InputBuilder input )
	{
		if ( input.StopProcessing )
			return;

		ActiveChild?.BuildInput( input );

		Controller?.BuildInput( input );

		if ( input.StopProcessing )
			return;

		Animator?.BuildInput( input );
	}

	public override void OnKilled()
	{
		base.OnKilled();

		TimeSinceDied = 0;
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( LifeState == LifeState.Dead )
		{
			if ( TimeSinceDied > RespawnTime && IsServer )
			{
				Respawn();
			}
		}
	}
}
