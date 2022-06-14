using SandboxEditor;

namespace Spire.DayNight;


[GameResource( "Spire Fog State", "spfog", "Spire fog state for the DN Controller" )]
public partial class FogState : GameResource
{
	public bool FogEnabled { get; set; } = true;
	public float FogStartDistance { get; set; } = 0.0f;
	public float FogEndDistance { get; set; } = 4000.0f;
	public float FogStartHeight { get; set; } = 0.0f;
	public float FogEndHeight { get; set; } = 200.0f;
	public float FogMaximumOpacity { get; set; } = 0.5f;
	public Color FogColor { get; set; } = Color.White;
	public float FogStrength { get; set; } = 1.0f;
	public float FogDistanceFalloffExponent { get; set; } = 2.0f;
	public float FogVerticalFalloffExponent { get; set; } = 1.0f;
	public float FogFadeTime { get; set; } = 1.0f;

	public Color AmbientLightColor { get; set; } = new Color( 0.1f, 0.1f, 0.1f );
}

public class DayNightGradient
{
	private struct GradientNode
	{
		public Color Color;
		public float Time;

		public GradientNode( Color color, float time )
		{
			Color = color;
			Time = time;
		}
	}

	private GradientNode[] _nodes;

	public DayNightGradient( Color dawnColor, Color dayColor, Color duskColor, Color nightColor )
	{
		_nodes = new GradientNode[7];
		_nodes[0] = new GradientNode( nightColor, 0f );
		_nodes[1] = new GradientNode( nightColor, 0.2f );
		_nodes[2] = new GradientNode( dawnColor, 0.3f );
		_nodes[3] = new GradientNode( dayColor, 0.5f );
		_nodes[4] = new GradientNode( dayColor, 0.7f );
		_nodes[5] = new GradientNode( duskColor, 0.85f );
		_nodes[6] = new GradientNode( nightColor, 1f );
	}

	public Color Evaluate( float fraction )
	{
		for ( var i = 0; i < _nodes.Length; i++ )
		{
			var node = _nodes[i];
			var nextIndex = i + 1;

			if ( _nodes.Length < nextIndex )
				nextIndex = 0;

			var nextNode = _nodes[nextIndex];

			if ( fraction >= node.Time && fraction <= nextNode.Time )
			{
				var duration = nextNode.Time - node.Time;
				var interpolate = 1f / duration * (fraction - node.Time);

				return Color.Lerp( node.Color, nextNode.Color, interpolate );
			}
		}

		return _nodes[0].Color;
	}
}

/// <summary>
/// A way to set the colour based on the time of day, it will smoothly blend between each colour when the time has changed. Also enables the day night cycle using a "light_environment"
/// </summary>
[HammerEntity]
[Library( "spire_daynight_controller" )]
[Title( "Time of Day Controller" ), Category( "Spire" )]
public partial class DayNightController : ModelEntity
{
	[Property( "DawnColor", Title = "Dawn Color" )]
	public Color DawnColor { get; set; }

	[Property( "DawnSkyColor", Title = "Dawn Sky Color" )]
	public Color DawnSkyColor { get; set; }

	[Property( "DayColor", Title = "Day Color" )]
	public Color DayColor { get; set; }

	[Property( "DaySkyColor", Title = "Day Sky Color" )]
	public Color DaySkyColor { get; set; }

	[Property( "DuskColor", Title = "Dusk Color" )]
	public Color DuskColor { get; set; }
	[Property( "DuskSkyColor", Title = "Dusk Sky Color" )]
	public Color DuskSkyColor { get; set; }

	[Property( "NightColor", Title = "Night Color" )]
	public Color NightColor { get; set; }

	[Property( "NightSkyColor", Title = "Night Sky Color" )]
	public Color NightSkyColor { get; set; }

	[Property, Category( "Environment Fog" ), ResourceType( "spfog" )]
	public string DawnFog { get; set; }

	[Property, Category( "Environment Fog" ), ResourceType( "spfog" )]
	public string DayFog { get; set; }

	[Property, Category( "Environment Fog" ), ResourceType( "spfog" )]
	public string DuskFog { get; set; }

	[Property, Category( "Environment Fog" ), ResourceType( "spfog" )]
	public string NightFog { get; set; }

	[ConVar.Replicated( "spire_daynight_debug" )]
	public static bool DayNightDebug { get; set; } = false;

	protected Output OnBecomeNight { get; set; }
	protected Output OnBecomeDusk { get; set; }
	protected Output OnBecomeDawn { get; set; }
	protected Output OnBecomeDay { get; set; }

	public EnvironmentLightEntity Environment
	{
		get
		{
			if ( _environment == null )
				_environment = All.OfType<EnvironmentLightEntity>().FirstOrDefault();
			return _environment;
		}
	}

	public GradientFogEntity GradientFog
	{
		get
		{
			if ( _gradientFog == null )
				_gradientFog = All.OfType<GradientFogEntity>().FirstOrDefault();
			return _gradientFog;
		}
	}

	private EnvironmentLightEntity _environment;
	private GradientFogEntity _gradientFog;
	private DayNightGradient _skyColorGradient;
	private DayNightGradient _colorGradient;

	public override void Spawn()
	{
		_colorGradient = new DayNightGradient( DawnColor, DayColor, DuskColor, NightColor );
		_skyColorGradient = new DayNightGradient( DawnSkyColor, DaySkyColor, DuskSkyColor, NightSkyColor );

		DayNightSystem.Instance.OnTimeStageChanged += OnTimeStageChanged;

		base.Spawn();
	}

	private void OnTimeStageChanged( TimeStage stage )
	{
		if ( stage == TimeStage.Dawn )
			OnBecomeDawn.Fire( this );
		else if ( stage == TimeStage.Day )
			OnBecomeDay.Fire( this );
		else if ( stage == TimeStage.Dusk )
			OnBecomeDusk.Fire( this );
		else if ( stage == TimeStage.Night )
			OnBecomeNight.Fire( this );

		if ( GradientFog.IsValid() )
			UpdateFogState( stage );
	}

	private FogState CurrentFogState;

	private FogState UpdateFogState( TimeStage stage )
	{
		CurrentFogState = stage switch
		{
			TimeStage.Dawn => ResourceLibrary.Get<FogState>( DawnFog ),
			TimeStage.Day => ResourceLibrary.Get<FogState>( DayFog ),
			TimeStage.Dusk => ResourceLibrary.Get<FogState>( DuskFog ),
			TimeStage.Night => ResourceLibrary.Get<FogState>( NightFog ),
			_ => ResourceLibrary.Get<FogState>( DawnFog )
		};

		return CurrentFogState;
	}

	[Event.Tick.Server]
	private void Tick()
	{
		var environment = Environment;
		if ( !environment.IsValid() ) return;

		var sunAngle = DayNightSystem.Instance.TimeOfDay / 24f * 360f;
		var radius = 10000f;

		environment.Color = _colorGradient.Evaluate( 1f / 24f * DayNightSystem.Instance.TimeOfDay );
		environment.SkyColor = _skyColorGradient.Evaluate( 1f / 24f * DayNightSystem.Instance.TimeOfDay );

		environment.Position = Vector3.Zero + Rotation.From( 0, 0, sunAngle + 60f ) * (radius * Vector3.Right);
		environment.Position += Rotation.From( 0, sunAngle, 0 ) * (radius * Vector3.Forward);

		var direction = (Vector3.Zero - environment.Position).Normal;
		environment.Rotation = Rotation.LookAt( direction, Vector3.Up );

		var fog = GradientFog;
		if ( !fog.IsValid() ) return;

		var lerpSpeed = Time.Delta * 0.5f;

		fog.FogStartDistance = fog.FogStartDistance.LerpTo( CurrentFogState.FogStartDistance, lerpSpeed );
		fog.FogEndDistance = fog.FogEndDistance.LerpTo( CurrentFogState.FogEndDistance, lerpSpeed );
		fog.FogColor = Color.Lerp( fog.FogColor, CurrentFogState.FogColor, lerpSpeed );
		fog.FogStartHeight = fog.FogStartHeight.LerpTo( CurrentFogState.FogStartHeight, lerpSpeed );
		fog.FogEndHeight = fog.FogEndHeight.LerpTo( CurrentFogState.FogEndHeight, lerpSpeed );

		Map.Scene.AmbientLightColor = Color.Lerp( Map.Scene.AmbientLightColor, CurrentFogState.AmbientLightColor, lerpSpeed * 5f );

		fog.UpdateFogState( true );

		if ( DayNightDebug )
		{
			DebugOverlay.ScreenText(
				 $"TimeOfDay: {DayNightSystem.Instance.TimeOfDay}\n" +
				 $"FogStartDistance: {fog.FogStartDistance}\n" +
				 $"FogEndDistance: {fog.FogEndDistance}\n" +
				 $"FogColor: {fog.FogColor}\n" +
				 $"AmbientLight: {Map.Scene.AmbientLightColor}\n" +
				 $"FogStartHeight: {fog.FogStartHeight}\n" +
				 $"FogEndHeight: {fog.FogEndHeight}\n"
			);
		}
	}
}
