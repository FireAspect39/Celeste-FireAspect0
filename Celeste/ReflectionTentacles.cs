﻿// Decompiled with JetBrains decompiler
// Type: Celeste.ReflectionTentacles
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Users\User\OneDrive\Desktop\Celeste!\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using System;
using System.Collections.Generic;

#nullable disable
namespace Celeste
{
  [Tracked(false)]
  public class ReflectionTentacles : Entity
  {
    public int Index;
    public List<Vector2> Nodes = new List<Vector2>();
    private Vector2 outwards;
    private Vector2 lastOutwards;
    private float ease;
    private Vector2 p;
    private Player player;
    private float fearDistance;
    private float offset;
    private bool createdFromLevel;
    private int slideUntilIndex;
    private int layer;
    private const int NodesPerTentacle = 10;
    private ReflectionTentacles.Tentacle[] tentacles;
    private int tentacleCount;
    private VertexPositionColorTexture[] vertices;
    private int vertexCount;
    private Color color = Color.Purple;
    private float soundDelay = 0.25f;
    private List<MTexture[]> arms = new List<MTexture[]>();
    private List<MTexture> fillers;

    public ReflectionTentacles()
    {
    }

    public ReflectionTentacles(EntityData data, Vector2 offset)
      : base(data.Position + offset)
    {
      this.Nodes.Add(this.Position);
      foreach (Vector2 node in data.Nodes)
        this.Nodes.Add(offset + node);
      switch (data.Attr("fear_distance"))
      {
        case "close":
          this.fearDistance = 16f;
          break;
        case "medium":
          this.fearDistance = 40f;
          break;
        case "far":
          this.fearDistance = 80f;
          break;
      }
      this.Create(this.fearDistance, data.Int("slide_until"), 0, this.Nodes);
      this.createdFromLevel = true;
    }

    public override void Added(Scene scene)
    {
      base.Added(scene);
      if (!this.createdFromLevel)
        return;
      for (int layer = 1; layer < 4; ++layer)
      {
        ReflectionTentacles reflectionTentacles = new ReflectionTentacles();
        reflectionTentacles.Create(this.fearDistance, this.slideUntilIndex, layer, this.Nodes);
        scene.Add((Entity) reflectionTentacles);
      }
    }

    public override void Awake(Scene scene)
    {
      base.Awake(scene);
      Player entity = this.Scene.Tracker.GetEntity<Player>();
      bool flag = false;
      while (entity != null && this.Index < this.Nodes.Count - 1)
      {
        Vector2 vector2;
        this.p = vector2 = Calc.ClosestPointOnLine(this.Nodes[this.Index], this.Nodes[this.Index] + this.outwards * 10000f, entity.Center);
        vector2 = this.Nodes[this.Index] - vector2;
        if ((double) vector2.Length() < (double) this.fearDistance)
        {
          flag = true;
          this.Retreat();
        }
        else
          break;
      }
      if (!flag)
        return;
      this.ease = 1f;
      this.SnapTentacles();
    }

    public void Create(
      float fearDistance,
      int slideUntilIndex,
      int layer,
      List<Vector2> startNodes)
    {
      this.Nodes = new List<Vector2>();
      foreach (Vector2 startNode in startNodes)
        this.Nodes.Add(startNode + new Vector2((float) Calc.Random.Range(-8, 8), (float) Calc.Random.Range(-8, 8)));
      this.Tag = (int) Tags.TransitionUpdate;
      this.Position = this.Nodes[0];
      this.outwards = (this.Nodes[0] - this.Nodes[1]).SafeNormalize();
      this.fearDistance = fearDistance;
      this.slideUntilIndex = slideUntilIndex;
      this.layer = layer;
      switch (layer)
      {
        case 0:
          this.Depth = -1000000;
          this.color = Calc.HexToColor("3f2a4f");
          this.offset = 110f;
          break;
        case 1:
          this.Depth = 8990;
          this.color = Calc.HexToColor("7b3555");
          this.offset = 80f;
          break;
        case 2:
          this.Depth = 10010;
          this.color = Calc.HexToColor("662847");
          this.offset = 50f;
          break;
        case 3:
          this.Depth = 10011;
          this.color = Calc.HexToColor("492632");
          this.offset = 20f;
          break;
      }
      foreach (MTexture atlasSubtexture in GFX.Game.GetAtlasSubtextures("scenery/tentacles/arms"))
      {
        MTexture[] mtextureArray = new MTexture[10];
        int width = atlasSubtexture.Width / 10;
        for (int index = 0; index < 10; ++index)
          mtextureArray[index] = atlasSubtexture.GetSubtexture(width * (10 - index - 1), 0, width, atlasSubtexture.Height);
        this.arms.Add(mtextureArray);
      }
      this.fillers = GFX.Game.GetAtlasSubtextures("scenery/tentacles/filler");
      this.tentacles = new ReflectionTentacles.Tentacle[100];
      float along = 0.0f;
      int index1 = 0;
      while (index1 < this.tentacles.Length && (double) along < 440.0)
      {
        this.tentacles[index1].Approach = (float) (0.25 + (double) Calc.Random.NextFloat() * 0.75);
        this.tentacles[index1].Length = 32f + Calc.Random.NextFloat(64f);
        this.tentacles[index1].Width = 4f + Calc.Random.NextFloat(16f);
        this.tentacles[index1].Position = this.TargetTentaclePosition(this.tentacles[index1], this.Nodes[0], along);
        this.tentacles[index1].WaveOffset = Calc.Random.NextFloat();
        this.tentacles[index1].TexIndex = Calc.Random.Next(this.arms.Count);
        this.tentacles[index1].FillerTexIndex = Calc.Random.Next(this.fillers.Count);
        this.tentacles[index1].LerpDuration = 0.5f + Calc.Random.NextFloat(0.25f);
        along += this.tentacles[index1].Width;
        ++index1;
        ++this.tentacleCount;
      }
      this.vertices = new VertexPositionColorTexture[this.tentacleCount * 12 * 6];
      for (int index2 = 0; index2 < this.vertices.Length; ++index2)
        this.vertices[index2].Color = this.color;
    }

    private Vector2 TargetTentaclePosition(
      ReflectionTentacles.Tentacle tentacle,
      Vector2 position,
      float along)
    {
      Vector2 vector2_1;
      Vector2 vector2_2 = vector2_1 = position - this.outwards * this.offset;
      if (this.player != null)
      {
        Vector2 vector2_3 = this.outwards.Perpendicular();
        vector2_2 = Calc.ClosestPointOnLine(vector2_2 - vector2_3 * 200f, vector2_2 + vector2_3 * 200f, this.player.Position);
      }
      Vector2 vector2_4 = this.outwards.Perpendicular() * (float) ((double) along - 220.0 + (double) tentacle.Width * 0.5);
      Vector2 vector2_5 = vector2_1 + vector2_4;
      float num = (vector2_2 - vector2_5).Length();
      return vector2_5 + this.outwards * num * 0.6f;
    }

    public void Retreat()
    {
      if (this.Index >= this.Nodes.Count - 1)
        return;
      this.lastOutwards = this.outwards;
      this.ease = 0.0f;
      ++this.Index;
      if (this.layer == 0 && (double) this.soundDelay <= 0.0)
        Audio.Play((double) (this.Nodes[this.Index - 1] - this.Nodes[this.Index]).Length() > 180.0 ? "event:/game/06_reflection/scaryhair_whoosh" : "event:/game/06_reflection/scaryhair_move");
      for (int index = 0; index < this.tentacleCount; ++index)
      {
        this.tentacles[index].LerpPercent = 0.0f;
        this.tentacles[index].LerpPositionFrom = this.tentacles[index].Position;
      }
    }

    public override void Update()
    {
      this.soundDelay -= Engine.DeltaTime;
      if (this.slideUntilIndex > this.Index)
      {
        this.player = this.Scene.Tracker.GetEntity<Player>();
        if (this.player != null)
        {
          Vector2 vector2 = this.p = Calc.ClosestPointOnLine(this.Nodes[this.Index] - this.outwards * 10000f, this.Nodes[this.Index] + this.outwards * 10000f, this.player.Center);
          if ((double) (vector2 - this.Nodes[this.Index]).Length() < 32.0)
          {
            this.Retreat();
            this.outwards = (this.Nodes[this.Index - 1] - this.Nodes[this.Index]).SafeNormalize();
          }
          else
            this.MoveTentacles(vector2 - this.outwards * 190f);
        }
      }
      else
      {
        FinalBoss entity = this.Scene.Tracker.GetEntity<FinalBoss>();
        this.player = this.Scene.Tracker.GetEntity<Player>();
        if (entity == null && this.player != null && this.Index < this.Nodes.Count - 1 && (double) (this.Nodes[this.Index] - (this.p = Calc.ClosestPointOnLine(this.Nodes[this.Index], this.Nodes[this.Index] + this.outwards * 10000f, this.player.Center))).Length() < (double) this.fearDistance)
          this.Retreat();
        if (this.Index > 0)
        {
          this.ease = Calc.Approach(this.ease, 1f, (this.Index == this.Nodes.Count - 1 ? 2f : 1f) * Engine.DeltaTime);
          this.outwards = Calc.AngleToVector(Calc.AngleLerp(this.lastOutwards.Angle(), (this.Nodes[this.Index - 1] - this.Nodes[this.Index]).Angle(), Ease.QuadOut(this.ease)), 1f);
          float along = 0.0f;
          for (int index = 0; index < this.tentacleCount; ++index)
          {
            Vector2 vector2 = this.TargetTentaclePosition(this.tentacles[index], this.Nodes[this.Index], along);
            if ((double) this.tentacles[index].LerpPercent < 1.0)
            {
              this.tentacles[index].LerpPercent += Engine.DeltaTime / this.tentacles[index].LerpDuration;
              this.tentacles[index].Position = Vector2.Lerp(this.tentacles[index].LerpPositionFrom, vector2, Ease.CubeInOut(this.tentacles[index].LerpPercent));
            }
            else
              this.tentacles[index].Position += (vector2 - this.tentacles[index].Position) * (1f - (float) Math.Pow(0.10000000149011612 * (double) this.tentacles[index].Approach, (double) Engine.DeltaTime));
            along += this.tentacles[index].Width;
          }
        }
        else
          this.MoveTentacles(this.Nodes[this.Index]);
      }
      if (this.Index == this.Nodes.Count - 1)
      {
        Color color = this.color * (1f - this.ease);
        for (int index = 0; index < this.vertices.Length; ++index)
          this.vertices[index].Color = color;
      }
      this.UpdateVertices();
    }

    private void MoveTentacles(Vector2 pos)
    {
      float along = 0.0f;
      for (int index = 0; index < this.tentacleCount; ++index)
      {
        Vector2 vector2 = this.TargetTentaclePosition(this.tentacles[index], pos, along);
        this.tentacles[index].Position += (vector2 - this.tentacles[index].Position) * (1f - (float) Math.Pow(0.10000000149011612 * (double) this.tentacles[index].Approach, (double) Engine.DeltaTime));
        along += this.tentacles[index].Width;
      }
    }

    public void SnapTentacles()
    {
      float along = 0.0f;
      for (int index = 0; index < this.tentacleCount; ++index)
      {
        this.tentacles[index].LerpPercent = 1f;
        this.tentacles[index].Position = this.TargetTentaclePosition(this.tentacles[index], this.Nodes[this.Index], along);
        along += this.tentacles[index].Width;
      }
    }

    private void UpdateVertices()
    {
      Vector2 vector2_1 = -this.outwards.Perpendicular();
      int n = 0;
      for (int index = 0; index < this.tentacleCount; ++index)
      {
        Vector2 position = this.tentacles[index].Position;
        Vector2 vector2_2 = vector2_1 * (float) ((double) this.tentacles[index].Width * 0.5 + 2.0);
        MTexture[] arm = this.arms[this.tentacles[index].TexIndex];
        this.Quad(ref n, position + vector2_2, position + vector2_2 * 1.5f - this.outwards * 240f, position - vector2_2 * 1.5f - this.outwards * 240f, position - vector2_2, this.fillers[this.tentacles[index].FillerTexIndex]);
        Vector2 vector2_3 = position;
        Vector2 vector2_4 = vector2_2;
        float num1 = this.tentacles[index].Length / 10f + Calc.YoYo(this.tentacles[index].LerpPercent) * 6f;
        for (int y = 1; y <= 10; ++y)
        {
          float num2 = (float) y / 10f;
          float num3 = (float) ((double) this.Scene.TimeActive * (double) this.tentacles[index].WaveOffset * Math.Pow(1.1000000238418579, (double) y) * 2.0);
          float num4 = (float) ((double) this.tentacles[index].WaveOffset * 3.0 + (double) y * 0.05000000074505806);
          float num5 = (float) (2.0 + 4.0 * (double) num2);
          Vector2 vector2_5 = vector2_1 * (float) Math.Sin((double) num3 + (double) num4) * num5;
          Vector2 vector2_6 = vector2_3 + this.outwards * num1 + vector2_5;
          Vector2 vector2_7 = vector2_2 * (1f - num2);
          this.Quad(ref n, vector2_6 - vector2_7, vector2_3 - vector2_4, vector2_3 + vector2_4, vector2_6 + vector2_7, arm[y - 1]);
          vector2_3 = vector2_6;
          vector2_4 = vector2_7;
        }
      }
      this.vertexCount = n;
    }

    private void Quad(
      ref int n,
      Vector2 a,
      Vector2 b,
      Vector2 c,
      Vector2 d,
      MTexture subtexture = null)
    {
      if (subtexture == null)
        subtexture = GFX.Game["util/pixel"];
      float num1 = 1f / (float) subtexture.Texture.Texture.Width;
      float num2 = 1f / (float) subtexture.Texture.Texture.Height;
      Vector2 vector2_1;
      ref Vector2 local1 = ref vector2_1;
      double x1 = (double) subtexture.ClipRect.Left * (double) num1;
      Rectangle clipRect1 = subtexture.ClipRect;
      double y1 = (double) clipRect1.Top * (double) num2;
      local1 = new Vector2((float) x1, (float) y1);
      Vector2 vector2_2;
      ref Vector2 local2 = ref vector2_2;
      clipRect1 = subtexture.ClipRect;
      double x2 = (double) clipRect1.Right * (double) num1;
      Rectangle clipRect2 = subtexture.ClipRect;
      double y2 = (double) clipRect2.Top * (double) num2;
      local2 = new Vector2((float) x2, (float) y2);
      Vector2 vector2_3;
      ref Vector2 local3 = ref vector2_3;
      clipRect2 = subtexture.ClipRect;
      double x3 = (double) clipRect2.Left * (double) num1;
      Rectangle clipRect3 = subtexture.ClipRect;
      double y3 = (double) clipRect3.Bottom * (double) num2;
      local3 = new Vector2((float) x3, (float) y3);
      Vector2 vector2_4;
      ref Vector2 local4 = ref vector2_4;
      clipRect3 = subtexture.ClipRect;
      double x4 = (double) clipRect3.Right * (double) num1;
      double y4 = (double) subtexture.ClipRect.Bottom * (double) num2;
      local4 = new Vector2((float) x4, (float) y4);
      this.vertices[n].Position = new Vector3(a, 0.0f);
      this.vertices[n++].TextureCoordinate = vector2_1;
      this.vertices[n].Position = new Vector3(b, 0.0f);
      this.vertices[n++].TextureCoordinate = vector2_2;
      this.vertices[n].Position = new Vector3(d, 0.0f);
      this.vertices[n++].TextureCoordinate = vector2_3;
      this.vertices[n].Position = new Vector3(d, 0.0f);
      this.vertices[n++].TextureCoordinate = vector2_3;
      this.vertices[n].Position = new Vector3(b, 0.0f);
      this.vertices[n++].TextureCoordinate = vector2_2;
      this.vertices[n].Position = new Vector3(c, 0.0f);
      this.vertices[n++].TextureCoordinate = vector2_4;
    }

    public override void Render()
    {
      if (this.vertexCount <= 0)
        return;
      GameplayRenderer.End();
      Engine.Graphics.GraphicsDevice.Textures[0] = (Texture) this.arms[0][0].Texture.Texture;
      GFX.DrawVertices<VertexPositionColorTexture>((this.Scene as Level).Camera.Matrix, this.vertices, this.vertexCount, GFX.FxTexture);
      GameplayRenderer.Begin();
    }

    private struct Tentacle
    {
      public Vector2 Position;
      public float Width;
      public float Length;
      public float Approach;
      public float WaveOffset;
      public int TexIndex;
      public int FillerTexIndex;
      public Vector2 LerpPositionFrom;
      public float LerpPercent;
      public float LerpDuration;
    }
  }
}
