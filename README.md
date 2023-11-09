# MarseyPatch examples

## Simple examples of a harmony patch used by MarseyLoader.

Contains 4 patches:

* BasePatch - Base example patch with more documentation. Attaches to [EntryPoint::Init](https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Entry/EntryPoint.cs#L74) in Content.Client, does nothing else.
* DODPatch - Patches [Clyde::DrawOcclusionDepth](https://github.com/space-wizards/RobustToolbox/blob/master/Robust.Client/Graphics/Clyde/Clyde.LightRendering.cs#L254) from the Robust.Client assembly, [skips it](https://harmony.pardeike.net/articles/patching-prefix.html#changing-the-result-and-skipping-the-original).
* FlashPatch - Patches [FlashOverlay::Draw](https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Flash/FlashOverlay.cs#L52) from the Content.Client assembly, also skips it.
* Rethemer - Patches [StyleNano](https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Stylesheets/StyleNano.cs#L43) and [MenuButton](https://github.com/space-wizards/space-station-14/blob/master/Content.Client/UserInterface/Controls/MenuButton.cs#L12)'s [type initializers](https://learn.microsoft.com/en-us/dotnet/api/system.type.typeinitializer?view=net-7.0), and changes their colors using a [transpiler](https://harmony.pardeike.net/articles/patching-transpiler.html).

Compile and paste the .dll's into the "Marsey" folder in the MarseyLoader's directory for them to load.
