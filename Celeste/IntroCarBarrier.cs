﻿// Decompiled with JetBrains decompiler
// Type: Celeste.IntroCarBarrier
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Users\User\OneDrive\Desktop\Celeste!\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

#nullable disable
namespace Celeste
{
  public class IntroCarBarrier : Entity
  {
    public IntroCarBarrier(Vector2 position, int depth, Color color)
    {
      this.Position = position;
      this.Depth = depth;
      Monocle.Image image = new Monocle.Image(GFX.Game["scenery/car/barrier"]);
      image.Origin = new Vector2(0.0f, image.Height);
      image.Color = color;
      this.Add((Component) image);
    }
  }
}
