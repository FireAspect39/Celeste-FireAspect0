﻿// Decompiled with JetBrains decompiler
// Type: Celeste.LevelEndingHook
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Users\User\OneDrive\Desktop\Celeste!\Celeste\Celeste.exe

using Monocle;
using System;

#nullable disable
namespace Celeste
{
  [Tracked(false)]
  public class LevelEndingHook : Component
  {
    public Action OnEnd;

    public LevelEndingHook(Action onEnd)
      : base(false, false)
    {
      this.OnEnd = onEnd;
    }
  }
}
