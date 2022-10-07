﻿using Microsoft.Xna.Framework;
using Pathoschild.Stardew.Automate;
using Slothsoft.Challenger.Objects;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;

namespace Slothsoft.ChallengerAutomate; 

public class MagicalObjectAutomationFactory : IAutomationFactory
{
    /// <summary>Get a machine, container, or connector instance for a given object.</summary>
    /// <param name="obj">The in-game object.</param>
    /// <param name="location">The location to check.</param>
    /// <param name="tile">The tile position to check.</param>
    /// <returns>Returns an instance or <c>null</c>.</returns>
    public IAutomatable? GetFor(SObject obj, GameLocation location, in Vector2 tile)
    {
        if (obj.ParentSheetIndex == MagicalObject.ObjectId)
            return new MagicalObjectMachine(obj, location, tile);

        return null;
    }

    /// <summary>Get a machine, container, or connector instance for a given terrain feature.</summary>
    /// <param name="feature">The terrain feature.</param>
    /// <param name="location">The location to check.</param>
    /// <param name="tile">The tile position to check.</param>
    /// <returns>Returns an instance or <c>null</c>.</returns>
    public IAutomatable? GetFor(TerrainFeature feature, GameLocation location, in Vector2 tile)
    {
        return null;
    }

    /// <summary>Get a machine, container, or connector instance for a given building.</summary>
    /// <param name="building">The building.</param>
    /// <param name="location">The location to check.</param>
    /// <param name="tile">The tile position to check.</param>
    /// <returns>Returns an instance or <c>null</c>.</returns>
    public IAutomatable? GetFor(Building building, BuildableGameLocation location, in Vector2 tile)
    {
        return null;
    }

    /// <summary>Get a machine, container, or connector instance for a given tile position.</summary>
    /// <param name="location">The location to check.</param>
    /// <param name="tile">The tile position to check.</param>
    /// <returns>Returns an instance or <c>null</c>.</returns>
    /// <remarks>Shipping bin logic from <see cref="Farm.leftClick"/>, garbage can logic from <see cref="Town.checkAction"/>.</remarks>
    public IAutomatable? GetForTile(GameLocation location, in Vector2 tile)
    {
        return null;
    }
}