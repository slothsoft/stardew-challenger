using System;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;

namespace Slothsoft.Challenger.ThirdParty; 

/// <summary>
/// Base class for the entire API. Can be used to add custom information providers.
/// </summary>
public interface IInformant {

    /// <summary>
    /// Adds a tooltip generator for the <see cref="TerrainFeature"/>(s) under the mouse position.
    /// </summary>
    void AddTerrainFeatureTooltipGenerator(string id, string displayName, string description, Func<TerrainFeature, string> generator); 
    
    /// <summary>
    /// Adds a tooltip generator for the <see cref="Object"/>(s) under the mouse position.
    /// </summary>
    void AddObjectTooltipGenerator(string id, string displayName, string description, Func<SObject, string?> generator); 
    
    /// <summary>
    /// Adds a decorator for the <see cref="Item"/>(s) under the mouse position.
    /// </summary>
    void AddItemDecorator(string id, string displayName, string description, Func<Item, Texture2D?> decorator); 
}