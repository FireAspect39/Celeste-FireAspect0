﻿// Decompiled with JetBrains decompiler
// Type: Celeste.CreditsTrigger
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Users\User\OneDrive\Desktop\Celeste!\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

#nullable disable
namespace Celeste
{
  [Tracked(false)]
  public class CreditsTrigger : Trigger
  {
    public string Event;

    public CreditsTrigger(EntityData data, Vector2 offset)
      : base(data, offset)
    {
      this.Event = data.Attr("event");
    }

    public override void OnEnter(Player player)
    {
      this.Triggered = true;
      if (CS07_Credits.Instance == null)
        return;
      CS07_Credits.Instance.Event = this.Event;
    }
  }
}
