namespace Spire.Gamemodes.Duel;

public partial class DuelSpectatorPawn : BasePawn
{
	public DuelSpectatorPawn()
	{
	}

	[Net]
	public BasePawn Target { get; set; }

	protected void SetRandomPlayer()
	{
		var randomPlayer = Client.All.Select( x => x.Pawn as PlayerCharacter )
			.Where( x => x != Target )
			.OrderBy( x => Guid.NewGuid() )
			.FirstOrDefault();

		Target = randomPlayer;

		Log.Info( $"Target: {Target}" );
	}

	public override void Spawn()
	{
		base.Spawn();
	}

	public override void Respawn()
	{
		base.Respawn();

		CameraMode = new DuelSpectatorCameraMode();
		SetRandomPlayer();
	}

	public override void BuildInput( InputBuilder input )
	{
		base.BuildInput( input );

		if ( input.Pressed( InputButton.Jump ) )
		{
			SetRandomPlayer();
		}
	}
}
