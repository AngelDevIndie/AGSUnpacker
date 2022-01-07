﻿using System;
using System.IO;

namespace AGSUnpacker.Lib.Game
{
  public class AGSGUIButton : AGSGUIObject
  {
    public Int32 image;
    public Int32 image_mouseover;
    public Int32 image_pushed;
    public Int32 image_current;
    public Int32 is_pushed;
    public Int32 is_mouseover;
    public Int32 font;
    public Int32 text_color;
    public Int32 left_click_action;
    public Int32 right_click_action;
    public Int32 left_click_data;
    public Int32 right_click_data;
    public string text;
    public Int32 text_aligment;
    public Int32 reserved1;

    public AGSGUIButton()
    {
      image = 0;
      image_mouseover = 0;
      image_pushed = 0;
      image_current = 0;
      is_pushed = 0;
      is_mouseover = 0;
      font = 0;
      text_color = 0;
      left_click_action = 0;
      right_click_action = 0;
      left_click_data = 0;
      right_click_data = 0;
      text = string.Empty;
      text_aligment = 0;
      reserved1 = 0;
    }

    public void LoadFromStream(BinaryReader r, int gui_version)
    {
      base.LoadFromStream(r, gui_version);

      // parse button info
      image = r.ReadInt32();
      image_mouseover = r.ReadInt32();
      image_pushed = r.ReadInt32();

      if (gui_version < 119) // 3.5.0
      {
        image_current = r.ReadInt32();
        is_pushed = r.ReadInt32();
        is_mouseover = r.ReadInt32();
      }

      font = r.ReadInt32();
      text_color = r.ReadInt32();
      left_click_action = r.ReadInt32();
      right_click_action = r.ReadInt32();
      left_click_data = r.ReadInt32();
      right_click_data = r.ReadInt32();

      if (gui_version < 119) // 3.5.0
        text = r.ReadFixedCString(50);
      else
        text = r.ReadPrefixedString32();

      if (gui_version >= 111) // 2.7.0+ ???
      {
        text_aligment = r.ReadInt32();
        if (gui_version < 119) // 3.5.0
          reserved1 = r.ReadInt32();
      }
    }
  }
}
