﻿// Decompiled with JetBrains decompiler
// Type: Celeste.MountainState
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Users\User\OneDrive\Desktop\Celeste!\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

#nullable disable
namespace Celeste
{
  public class MountainState
  {
    public Skybox Skybox;
    public VirtualTexture TerrainTexture;
    public VirtualTexture BuildingsTexture;
    public Color FogColor;

    public MountainState(
      VirtualTexture terrainTexture,
      VirtualTexture buildingsTexture,
      VirtualTexture skyboxTexture,
      Color fogColor)
    {
      this.TerrainTexture = terrainTexture;
      this.BuildingsTexture = buildingsTexture;
      this.Skybox = new Skybox(skyboxTexture);
      this.FogColor = fogColor;
    }
  }
}
