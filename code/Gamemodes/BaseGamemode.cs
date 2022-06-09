namespace Spire.Gamemodes;

public abstract partial class BaseGamemode : Entity
{
	public static BaseGamemode Current { get; set; }

	/// <summary>
	/// Client only
	/// </summary>
	public static Panel CurrentHudPanel { get; set; }

	/// <summary>
	/// Can specify a panel to be created when the gamemode is made.
	/// </summary>
	/// <returns></returns>
	public virtual Panel CreateHud() => null;

	/// <summary>
	/// Gamemodes can define what pawn to create
	/// </summary>
	/// <param name="cl"></param>
	/// <returns></returns>
	public virtual BasePawn CreatePawn( Client cl ) => new PlayerCharacter( cl );

	public override void Spawn()
	{
		base.Spawn();

		Transmit = TransmitType.Always;

		// There can be only one gamemode running at a time.
		if ( Current.IsValid() && Current != this )
		{
			Delete();
			Log.Warning( "There can be only one gamemode running at one time. Please make sure there's only 1 gamemode entity on a level." );

			return;
		}

		Current = this;
	}


	public override void ClientSpawn()
	{
		base.ClientSpawn();

		CurrentHudPanel = CreateHud();
		CurrentHudPanel.Parent = Game.Current?.Hud;

		Current = this;
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );
	}

	protected int PlayerCount { get; private set; }

	public virtual void OnClientJoined( Client cl )
	{
		PlayerCount++;
	}

	public virtual void OnClientLeft( Client cl, NetworkDisconnectionReason reason )
	{
		PlayerCount--;
	}

	public virtual void OnCharacterKilled( BaseCharacter character, DamageInfo damageInfo )
	{
	}

	public virtual Transform? GetSpawn( BaseCharacter character )
	{
		return null;
	}

	public virtual bool AllowMovement()
	{
		return true;
	}

	public virtual bool AllowRespawning()
	{
		return true;
	}
}
