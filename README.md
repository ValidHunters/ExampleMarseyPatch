# MarseyPatch example

Simple example of a harmony patch used by MarseyLoader.

This patches [DrawOcclusionDepth](https://github.com/space-wizards/RobustToolbox/blob/be33bc2219f228bfac8c6f33f5bdf96f6b763f29/Robust.Client/Graphics/Clyde/Clyde.LightRendering.cs#L254) from the Robust.Client assembly and [FlashOverlay.Draw](https://github.com/space-wizards/space-station-14/blob/c318cbac9f14577d68470bf940837efb8843f733/Content.Client/Flash/FlashOverlay.cs#L52) from the Content.Client assembly.

Compile and paste the .dll into the "Marsey" folder in the MarseyLoader's directory.