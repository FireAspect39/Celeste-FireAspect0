﻿// Decompiled with JetBrains decompiler
// Type: Celeste.MountainCamera
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Users\User\OneDrive\Desktop\Celeste!\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;

#nullable disable
namespace Celeste
{
  public struct MountainCamera
  {
    public Vector3 Position;
    public Vector3 Target;
    public Quaternion Rotation;

    public MountainCamera(Vector3 pos, Vector3 target)
    {
      this.Position = pos;
      this.Target = target;
      this.Rotation = new Quaternion().LookAt(this.Position, this.Target, Vector3.Up);
    }

    public void LookAt(Vector3 pos)
    {
      this.Target = pos;
      this.Rotation = new Quaternion().LookAt(this.Position, this.Target, Vector3.Up);
    }
  }
}
