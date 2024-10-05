// Decompiled with JetBrains decompiler
// Type: Celeste.ScreenshakeAmount
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Users\User\OneDrive\Desktop\Celeste!\Celeste\Celeste.exe

using System.Xml.Serialization;

#nullable disable
namespace Celeste
{
  public enum ScreenshakeAmount
  {
    [XmlEnum("false")] Off,
    [XmlEnum("true")] Half,
    On,
  }
}
