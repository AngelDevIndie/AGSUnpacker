﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using AGSUnpacker.Graphics;
using AGSUnpacker.Graphics.Formats;
using AGSUnpacker.Lib.Utils;

namespace AGSUnpacker.Lib.Graphics
{
  public class AGSSpriteSet
  {
    private static readonly string SpriteSetSignature = "\x20Sprite File\x20";
    private static readonly string SpriteSetFilename = "acsprset.spr";
    private static readonly string SpriteSetIndexSignature = "SPRINDEX";
    private static readonly string SpriteSetIndexFileName = "sprindex.dat";

    public static readonly Palette DefaultPalette = new Palette(
      new Color[] {
        new Color(  0,   0,   0), new Color(  0,   0, 168), new Color(  0, 168,   0),
        new Color(  0, 168, 168), new Color(168,   0,   0), new Color(168,   0, 168),
        new Color(168,  84,   0), new Color(168, 168, 168), new Color( 84,  84,  84),
        new Color( 84,  84, 252), new Color( 84, 252,  84), new Color( 84, 252, 252),
        new Color(252,  84,  84), new Color(252,  84, 252), new Color(252, 252,  84),
        new Color(252, 252, 252), new Color(  0,   0,   0), new Color( 20,  20,  20),
        new Color( 32,  32,  32), new Color( 44,  44,  44), new Color( 56,  56,  56),
        new Color( 68,  68,  68), new Color( 80,  80,  80), new Color( 96,  96,  96),
        new Color(112, 112, 112), new Color(128, 128, 128), new Color(144, 144, 144),
        new Color(160, 160, 160), new Color(180, 180, 180), new Color(200, 200, 200),
        new Color(224, 224, 224), new Color(252, 252, 252), new Color(  0,   0, 252),
        new Color( 68,   0, 252), new Color(124,   0, 252), new Color(184, 148,   0),
        new Color(184, 128,   0), new Color(252,   0, 188), new Color(252,   0, 128),
        new Color(252,   0,  84), new Color(252,   0,   0), new Color(252,  68,   0),
        new Color(128, 120, 120), new Color(136, 116, 112), new Color(132,   8,   4),
        new Color( 20,   4,   4), new Color(196,  20,   4), new Color(148, 136, 132),
        new Color( 68,  12,   4), new Color(148, 124, 120), new Color(176, 168, 168),
        new Color(164, 128, 124), new Color( 88,  84,  84), new Color(128, 116, 112),
        new Color(148, 120, 112), new Color(192, 160, 148), new Color(152, 128, 116),
        new Color(172, 152, 140), new Color(104,  44,  12), new Color(160, 144, 132),
        new Color(156,  76,  24), new Color(176, 128,  92), new Color(200, 108,  32),
        new Color(204, 192, 176), new Color(176, 168, 148), new Color(188, 180, 164),
        new Color(136, 132, 120), new Color(232, 228, 196), new Color(212, 208, 200),
        new Color(116, 112,  92), new Color(232, 232, 232), new Color(188, 188,  44),
        new Color(216, 212,  16), new Color(144, 144,  92), new Color(168, 168,  68),
        new Color(124, 128, 104), new Color(196, 200, 192), new Color(  0, 228, 204),
        new Color( 40, 200, 188), new Color( 80, 172, 168), new Color(  4, 184, 176),
        new Color( 16, 144, 156), new Color(176, 188, 192), new Color(188, 212, 216),
        new Color(152, 180, 192), new Color( 56, 152, 212), new Color(112, 176, 216),
        new Color(  0, 120, 200), new Color( 96, 104, 112), new Color(112, 120, 128),
        new Color(180, 200, 216), new Color(128, 164, 192), new Color(140, 164, 180),
        new Color(108, 148, 192), new Color( 88, 140, 192), new Color(116, 144, 172),
        new Color( 68, 128, 192), new Color(120, 132, 144), new Color( 32,  76, 140),
        new Color( 64,  76,  96), new Color( 68,  76,  84), new Color( 68,  96, 140),
        new Color(112, 120, 140), new Color( 76,  84, 100), new Color( 52,  60,  76),
        new Color( 80, 108, 152), new Color( 96, 104, 120), new Color(100, 120, 160),
        new Color( 80,  92, 112), new Color( 96, 108, 140), new Color(  8,  32,  88),
        new Color( 96, 108, 128), new Color( 88, 100, 120), new Color(  8,  32, 100),
        new Color( 88, 100, 132), new Color(  8,  24,  80), new Color( 80,  88, 120),
        new Color(  8,  24,  88), new Color(  0,  16,  80), new Color(  0,  16,  88),
        new Color(112, 112, 128), new Color( 56,  64, 104), new Color( 72,  80, 128),
        new Color( 40,  48,  96), new Color( 36,  48, 116), new Color( 24,  36, 100),
        new Color( 24,  36, 120), new Color(  4,  16,  72), new Color( 48,  56, 104),
        new Color( 48,  56, 116), new Color( 44,  56, 136), new Color( 24,  32,  88),
        new Color(  8,  24, 100), new Color( 64,  72, 136), new Color( 56,  64, 124),
        new Color( 16,  24,  80), new Color( 16,  24,  88), new Color(  8,  16,  80),
        new Color(128, 132, 148), new Color( 68,  72, 120), new Color( 16,  24,  96),
        new Color(  8,  16,  88), new Color(  0,   8,  88), new Color( 96,  96, 112),
        new Color(104, 108, 140), new Color( 84,  88, 132), new Color( 36,  40,  96),
        new Color( 24,  28,  80), new Color( 56,  56,  96), new Color( 44,  48, 108),
        new Color( 36,  40,  88), new Color( 24,  32, 164), new Color( 32,  40, 216),
        new Color( 24,  32, 216), new Color( 20,  28, 200), new Color( 24,  36, 228),
        new Color( 16,  24, 216), new Color( 12,  20, 192), new Color(  8,  20, 232),
        new Color( 96,  96, 140), new Color( 72,  76, 112), new Color(  8,   8,  72),
        new Color( 44,  48, 232), new Color( 32,  40, 228), new Color( 16,  24, 228),
        new Color(104, 104, 112), new Color(120, 120, 128), new Color(104, 104, 128),
        new Color(112, 112, 140), new Color( 96,  96, 120), new Color( 88,  88, 112),
        new Color( 96,  96, 128), new Color( 88,  88, 120), new Color( 24,  24,  36),
        new Color( 68,  68, 104), new Color( 80,  80, 124), new Color( 56,  56, 108),
        new Color( 48,  48,  96), new Color( 96,  96, 228), new Color( 24,  24,  88),
        new Color( 16,  16,  80), new Color( 16,  16,  88), new Color(124, 120, 140),
        new Color( 44,  44,  60), new Color( 68,  64,  96), new Color( 84,  80, 112),
        new Color( 36,  28,  80), new Color( 32,  24,  96), new Color( 24,  16,  88),
        new Color( 16,  12,  72), new Color( 56,  48,  88), new Color( 56,  48,  96),
        new Color( 56,  48, 108), new Color( 88,  80, 124), new Color( 64,  56, 100),
        new Color(104,  96, 136), new Color( 68,  56, 120), new Color( 76,  64, 104),
        new Color( 80,  72,  96), new Color(104,  96, 128), new Color( 96,  88, 120),
        new Color(100,  88, 132), new Color( 52,  40,  88), new Color( 84,  72, 112),
        new Color(104,  96, 120), new Color(120, 112, 140), new Color( 96,  88, 112),
        new Color(144, 140, 148), new Color( 68,  52,  88), new Color( 88,  72, 104),
        new Color(120, 112, 128), new Color(112, 104, 120), new Color(116, 104, 128),
        new Color(104,  88, 120), new Color( 96,  80, 112), new Color(104,  96, 112),
        new Color(136, 128, 140), new Color(100,  68, 120), new Color( 92,  80, 100),
        new Color(112,  96, 120), new Color( 84,  64,  96), new Color(140, 108, 156),
        new Color(104,  88, 112), new Color(120,  84, 132), new Color(160, 120, 168),
        new Color(116,  88, 120), new Color(132,  88, 136), new Color(128, 112, 128),
        new Color(120, 104, 120), new Color(124,  72, 120), new Color(112, 108, 112),
        new Color(120,  96, 116), new Color(108,  84, 100), new Color(148, 104, 136),
        new Color(140,  80, 120), new Color(156, 152, 156), new Color(112,  96, 108),
        new Color(180, 120, 156), new Color(176,  88, 140), new Color(152,  56, 112),
        new Color(116, 116, 116), new Color(128, 112, 120), new Color(212,  84, 136),
        new Color(144, 120, 132), new Color(188,  28,  88), new Color(136, 124, 128),
        new Color(136, 112, 120), new Color(124,  96, 104), new Color(124,  36,  52),
        new Color(132, 104, 108), new Color(120, 108, 108), new Color(228, 224, 224),
        new Color(180, 180, 180), new Color(200, 200, 200), new Color(160, 160, 160),
        new Color(120, 120, 120)
      }
    );

    public static void PackSprites(string folderPath, string outputFolderPath)
    {
      if (!Directory.Exists(outputFolderPath))
      {
        Trace.Assert(false, string.Format("PackSprites: Output folder \"{0}\" does not exist!", outputFolderPath));
        return;
      }

      string spritesetHeaderFilepath = Path.Combine(folderPath, SpriteSetHeader.FileName);
      if (!File.Exists(spritesetHeaderFilepath))
      {
        Trace.Assert(false, string.Format("PackSprites: SpriteSetHeader file \"{0}\" does not exist!", spritesetHeaderFilepath));
        return;
      }

      string[] filepaths = Directory.GetFiles(folderPath, "spr*");
      if (filepaths.Length < 1)
      {
        Trace.Assert(false, string.Format("PackSprites: Folder \"{0}\" does not have any sprite files!", folderPath));
        return;
      }

      string spritesetFilepath = Path.Combine(outputFolderPath, SpriteSetFilename);

      PackSpritesInternal(spritesetFilepath, spritesetHeaderFilepath, filepaths);
    }

    public static void PackSprites(string spritesetFilepath, string spritesetHeaderFilepath, params string[] filepaths)
    {
      if (!File.Exists(spritesetHeaderFilepath))
      {
        Trace.Assert(false, string.Format("PackSprites: SpriteSetHeader file \"{0}\" does not exist!", spritesetHeaderFilepath));
        return;
      }

      for (int i = 0; i < filepaths.Length; ++i)
      {
        if (!File.Exists(filepaths[i]))
        {
          Trace.Assert(false, string.Format("PackSprites: File \"{0}\" does not exist!", filepaths[i]));
          return;
        }
      }

      PackSpritesInternal(spritesetFilepath, spritesetHeaderFilepath, filepaths);
    }

    public static bool UnpackSprites(string spriteFilePath, string targetFolderPath)
    {
      if (!File.Exists(spriteFilePath))
      {
        Trace.Assert(false, string.Format("UnpackSprites: Sprite file \"{0}\" does not exist!", spriteFilePath));
        return false;
      }

      if (!Directory.Exists(targetFolderPath))
      {
        Trace.Assert(false, string.Format("UnpackSprites: Target folder \"{0}\" does not exist!", targetFolderPath));
        return false;
      }

      return UnpackSpritesInternal(spriteFilePath, targetFolderPath);
    }

    private static byte[] CompressRLE(byte[] buffer, int width, int height, int bytesPerPixel)
    {
      switch (bytesPerPixel)
      {
        case 1:
          return AGSCompression.WriteRLE8Rows(buffer, width, height);
        case 2:
          return AGSCompression.WriteRLE16Rows(buffer, width, height);
        case 4:
          return AGSCompression.WriteRLE32Rows(buffer, width, height);

        default:
          throw new InvalidDataException("Unknown bytesPerPixel value is encountered!");
      }
    }

    private static byte[] DecompressRLE(BinaryReader reader, long sizeCompressed, long sizeUncompressed, int bytesPerPixel)
    {
      switch (bytesPerPixel)
      {
        case 1:
          return AGSCompression.ReadRLE8(reader, sizeCompressed, sizeUncompressed);
        case 2:
          return AGSCompression.ReadRLE16(reader, sizeCompressed, sizeUncompressed);
        case 4:
          return AGSCompression.ReadRLE32(reader, sizeCompressed, sizeUncompressed);

        default:
          throw new InvalidDataException("Unknown bytesPerPixel value is encountered!");
      }
    }

    private static void PackSpritesInternal(string outputFilepath, string headerFilepath, params string[] filepaths)
    {
      SpriteSetHeader header = SpriteSetHeader.ReadFromFile(headerFilepath);
      List<SpriteEntry> sprites = GetSortedSpritesList(filepaths);

      using (FileStream stream = new FileStream(outputFilepath, FileMode.Create))
      {
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding(1252)))
        {
          int spritesCount = GetLargestIndex(sprites);
          WriteSpriteSetHeader(writer, header, spritesCount);
          SpriteIndexInfo[] spritesWritten = WriteSprites(writer, header, sprites);

          // HACK(adm244): temp solution
          string indexFilepath = Path.GetDirectoryName(outputFilepath);
          WriteSpriteIndexFile(indexFilepath, header, spritesWritten);
        }
      }
    }

    private static bool UnpackSpritesInternal(string spriteFilePath, string targetFolderPath)
    {
      Console.Write("Opening {0}...", spriteFilePath);

      SpriteSetHeader header = null;

      using (FileStream stream = new FileStream(spriteFilePath, FileMode.Open))
      {
        using (BinaryReader reader = new BinaryReader(stream, Encoding.Latin1))
        {
          Console.WriteLine(" Done!");
          Console.Write("Parsing {0}...", spriteFilePath);

          header = ReadSpriteSetHeader(reader);

          Console.WriteLine(" Done!");
          Console.WriteLine("Extracting...");

          //TODO(adm244): read sprindex.dat

          for (int index = 0; index <= header.SpritesCount; ++index)
          {
            Console.Write(string.Format("\tExtracting spr{0:D5}...", index));

            Bitmap sprite = ReadSprite(reader, header);
            if (sprite == null)
            {
              Console.WriteLine(" Skipping (empty).");
              continue;
            }

            SaveSprite(sprite, targetFolderPath, index);

            Console.WriteLine(" Done!");
          }
        }
      }

      //TODO(adm244): should probably check if file were _actually_ read
      if (header == null)
      {
        Console.WriteLine("Error! Could not read a file.");
        return false;
      }

      Console.WriteLine("Done!");
      Console.Write("Writting meta file...");

      header.WriteMetaFile(targetFolderPath);

      Console.WriteLine(" Done!");

      return (header.SpritesCount > 0);
    }

    private static bool GetSpriteIndex(string filename, out int index)
    {
      index = -1;

      const string prefix = "spr";

      if (!filename.StartsWith(prefix))
        return false;

      return int.TryParse(filename.Substring(prefix.Length), out index);
    }

    private static List<SpriteEntry> GetSortedSpritesList(string[] filePaths)
    {
      List<SpriteEntry> sprites = new List<SpriteEntry>();

      for (int i = 0; i < filePaths.Length; ++i)
      {
        string filename = Path.GetFileNameWithoutExtension(filePaths[i]);

        if (!GetSpriteIndex(filename, out int index))
          continue;

        Bitmap sprite = new Bitmap(filePaths[i]);
        sprites.Add(new SpriteEntry(index, sprite));
      }

      sprites.Sort();

      return sprites;
    }

    private static int GetLargestIndex(List<SpriteEntry> sprites)
    {
      //NOTE(adm244): assumes that sprites list is sorted
      return sprites[sprites.Count - 1].Index;
    }

    private static SpriteIndexInfo[] WriteSprites(BinaryWriter writer, SpriteSetHeader header, List<SpriteEntry> sprites)
    {
      List<SpriteIndexInfo> spriteIndexData = new List<SpriteIndexInfo>();

      int spriteIndex = 0;
      int listIndex = 0;

      while (listIndex < sprites.Count)
      {
        if (sprites[listIndex].Index == spriteIndex)
        {
          SpriteIndexInfo spriteWritten = WriteSprite(writer, header, sprites[listIndex].Sprite);
          spriteIndexData.Add(spriteWritten);
          ++listIndex;
        }
        else
        {
          SpriteIndexInfo spriteEmpty = new SpriteIndexInfo(writer.BaseStream.Position);
          spriteIndexData.Add(spriteEmpty);
          writer.Write((UInt16)0);
        }

        ++spriteIndex;
      }

      return spriteIndexData.ToArray();
    }

    private static SpriteIndexInfo WriteSprite(BinaryWriter writer, SpriteSetHeader header, Bitmap sprite)
    {
      SpriteIndexInfo spriteIndexData = new SpriteIndexInfo();

      spriteIndexData.Width = sprite.Width;
      spriteIndexData.Height = sprite.Height;
      spriteIndexData.Offset = writer.BaseStream.Position;

      if (header.Compression == CompressionType.RLE)
      {
        //NOTE(adm244): AGS doesn't support 24bpp RLE compressed images, so we convert them to 32bpp
        if (sprite.Format == PixelFormat.Rgb24)
          sprite = sprite.Convert(PixelFormat.Argb32);
      }

      writer.Write((UInt16)sprite.BytesPerPixel);
      writer.Write((UInt16)sprite.Width);
      writer.Write((UInt16)sprite.Height);

      //byte[] buffer = sprite.InternalImage.GetPixels();
      byte[] buffer = sprite.GetPixels();

      if (header.Compression == CompressionType.RLE)
        buffer = CompressRLE(buffer, sprite.Width, sprite.Height, sprite.BytesPerPixel);

      if (header.Version >= 6)
      {
        if (header.Compression == CompressionType.RLE)
          writer.Write((UInt32)buffer.Length);
      }
      else if (header.Version == 5)
        writer.Write((UInt32)buffer.Length);

      writer.Write((byte[])buffer);

      return spriteIndexData;
    }

    private static SpriteSetHeader ReadSpriteSetHeader(BinaryReader reader)
    {
      Int16 version = reader.ReadInt16();
      string signature = reader.ReadFixedCString(SpriteSetSignature.Length);
      Debug.Assert(signature == SpriteSetSignature);

      CompressionType compression = SpriteSetHeader.DefaultCompression;
      UInt32 fileID = SpriteSetHeader.DefaultFileID;

      if (version == 4)
        compression = CompressionType.Uncompressed;
      else if (version == 5)
        compression = CompressionType.RLE;
      else if (version >= 6)
      {
        compression = CompressionType.Unknown;
        byte compressionType = reader.ReadByte();
        if (Enum.IsDefined(typeof(CompressionType), (int)compressionType))
          compression = (CompressionType)compressionType;

        fileID = reader.ReadUInt32();
      }

      //TODO(adm244): we should probably get palette from a DTA file instead
      //TODO(adm244): it seems like we should take paluses into consideration here
      //TODO(adm244): double check that palette color format is RGB6bits
      Palette palette = SpriteSetHeader.DefaultPalette;
      if (version < 5)
        palette = AGSGraphics.ReadPalette(reader, PixelFormat.Rgb666);

      UInt16 spritesCount = reader.ReadUInt16();

      return new SpriteSetHeader(version, compression, fileID, spritesCount, palette);
    }

    private static void WriteSpriteSetHeader(BinaryWriter writer, SpriteSetHeader header, int spritesCount)
    {
      writer.Write((UInt16)header.Version);
      writer.Write((char[])SpriteSetSignature.ToCharArray());

      if (header.Version >= 6)
      {
        writer.Write((byte)header.Compression);
        writer.Write((UInt32)header.FileID);
      }

      if (header.Version < 5)
        AGSGraphics.WritePalette(writer, header.Palette, PixelFormat.Rgb666);

      writer.Write((UInt16)spritesCount);
    }

    //TODO(adm244): ReadSpriteIndexFile

    private static void WriteSpriteIndexFile(string outputFolder, SpriteSetHeader header, SpriteIndexInfo[] spriteIndexInfo)
    {
      // FIXME(adm244): check all filepaths so that they ALL are either RELATIVE or ABSOLUTE
      // because for now some files are saved in a working directory (relative paths)
      // and some in other places (absolute paths)
      string targetFilepath = Path.Combine(outputFolder, SpriteSetIndexFileName);

      using (FileStream stream = new FileStream(targetFilepath, FileMode.Create))
      {
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding(1252)))
        {
          writer.Write((char[])SpriteSetIndexSignature.ToCharArray());

          //NOTE(adm244): is this a file version?
          writer.Write((UInt32)2);

          writer.Write((UInt32)(header.FileID));
          writer.Write((UInt32)(spriteIndexInfo.Length - 1));
          writer.Write((UInt32)(spriteIndexInfo.Length));

          for (int i = 0; i < spriteIndexInfo.Length; ++i)
            writer.Write((UInt16)spriteIndexInfo[i].Width);

          for (int i = 0; i < spriteIndexInfo.Length; ++i)
            writer.Write((UInt16)spriteIndexInfo[i].Height);

          for (int i = 0; i < spriteIndexInfo.Length; ++i)
            writer.Write((UInt32)spriteIndexInfo[i].Offset);
        }
      }
    }

    private static Bitmap ReadSprite(BinaryReader reader, SpriteSetHeader header)
    {
      // TODO(adm244): maybe return PixelFormat instead of bytesPerPixel?
      byte[] buffer = ReadSprite(reader, header, out int width, out int height, out int bytesPerPixel);
      if (buffer == null)
        return null;

      PixelFormat format = PixelFormatExtension.FromBytesPerPixel(bytesPerPixel);
      if (format == PixelFormat.Indexed)
        return new Bitmap(width, height, buffer, format, header.Palette);

      return new Bitmap(width, height, buffer, format);
    }

    private static byte[] ReadSprite(BinaryReader reader, SpriteSetHeader header, out int width, out int height, out int bytesPerPixel)
    {
      width = 0;
      height = 0;

      bytesPerPixel = reader.ReadUInt16();
      if (bytesPerPixel == 0)
        return null;

      width = reader.ReadUInt16();
      height = reader.ReadUInt16();

      long size = (long)width * height * bytesPerPixel;
      long sizeUncompressed = size;

      if (header.Version >= 6)
      {
        if (header.Compression == CompressionType.RLE)
          size = reader.ReadUInt32();
      }
      else if (header.Version == 5)
        size = reader.ReadUInt32();

      if (header.Compression == CompressionType.RLE)
        return DecompressRLE(reader, size, sizeUncompressed, bytesPerPixel);

      return reader.ReadBytes((int)size);
    }

    private static void SaveSprite(Bitmap image, string folderPath, int index)
    {
      string fileName = string.Format("spr{0:D5}", index);
      string filePath = Path.Combine(folderPath, fileName);

      image.Save(filePath, ImageFormat.Png);
    }

    private class SpriteEntry : IComparable<SpriteEntry>
    {
      public int Index { get; }
      public Bitmap Sprite { get; }

      public SpriteEntry(int index, Bitmap sprite)
      {
        Index = index;
        Sprite = sprite;
      }

      public int CompareTo(SpriteEntry other)
      {
        if (Index == other.Index)
          return 0;

        if (Index > other.Index)
          return 1;
        else
          return -1;
      }
    }

    private struct SpriteIndexInfo
    {
      public int Width;
      public int Height;
      public long Offset;

      public SpriteIndexInfo(long offset)
        : this(0, 0, offset)
      {
      }

      public SpriteIndexInfo(int width, int height, long offset)
      {
        Width = width;
        Height = height;
        Offset = offset;
      }
    }
  }
}