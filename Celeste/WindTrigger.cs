﻿// Decompiled with JetBrains decompiler
// Type: Celeste.WindTrigger
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Users\User\OneDrive\Desktop\Celeste!\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

#nullable disable
namespace Celeste
{
  [Tracked(false)]
  public class WindTrigger : Trigger
  {
    public WindController.Patterns Pattern;

    public WindTrigger(EntityData data, Vector2 offset)
      : base(data, offset)
    {
      this.Pattern = data.Enum<WindController.Patterns>("pattern");
    }

    public override void OnEnter(Player player)
    {
      base.OnEnter(player);
      WindController first = this.Scene.Entities.FindFirst<WindController>();
      if (first == null)
        this.Scene.Add((Entity) new WindController(this.Pattern));
      else
        first.SetPattern(this.Pattern);
    }
  }
}
