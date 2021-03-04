/*
 * Créé avec SharpDevelop
 * Codeur: Doigt
 * Date: 2021-03-03
 * 
 */
using System;
using System.Media;
using System.IO;
using ConsolePaint;
using System.Drawing;

namespace console_music_video_player
{
	class Program
	{
		enum ColorMode : byte
		{ 
			Mono, 
			Duo, 
			Full
		};
		
		public static void Main(string[] args)
		{
			var mode = ColorMode.Full;
			byte color = 1;
			
			if (args.Length != 0)
			{
				switch (args[0].ToLower())
				{
					case "mono":
						mode = ColorMode.Mono;
						break;
						
					case "duo":
						mode = ColorMode.Duo;
						break;
				}
				
				if (args.Length > 1)
				{
					switch (args[1].ToLower())
					{
						case "blue":
							color = (byte)Palette.Color.Blue;
							break;
							
						case "cyan":
							color = mode == ColorMode.Mono ? (byte)Palette.Color.Cyan : (byte)Palette.DuoColor.Cyan_Blue;
							break;
							
						case "gray":
							
							color = (byte)Palette.Color.Gray;
							
							if (mode == ColorMode.Duo)
								goto case "blue";
							
							break;
							
						case "green":
							color = (byte)Palette.Color.Green;
							break;
							
						case "magenta":
							color = mode == ColorMode.Mono ? (byte)Palette.Color.Magenta : (byte)Palette.DuoColor.Magenta_Red;
							break;
							
						case "red":
							color = (byte)Palette.Color.Red;
							break;
							
						case "dark":
							color = 0;
							break;
							
						default:
							color = mode == ColorMode.Mono ? (byte)Palette.Color.Yellow : (byte)Palette.DuoColor.Yellow_Green;
							break;
					}
				}
			}
			
			if (mode == ColorMode.Full)
				Console.SetWindowSize(60, 30);
			
			else
				Console.SetWindowSize(50, 25);
			
			using (var console = Console.OpenStandardOutput()) 
			{
				var buffer = System.Text.Encoding.UTF8.GetBytes("Press any key to start.");
				console.Write(buffer, 0, buffer.Length);
			}
			
			Console.ReadKey();
			
			var audio = new SoundPlayer();
			audio.SoundLocation = "music.wav";
			audio.Play();
			
			var files = Directory.GetFiles("frames/", "*.jpg", SearchOption.TopDirectoryOnly);

			Console.CursorVisible = false;
			
			Action<string> draw;
			
			switch (mode) 
			{
				case ColorMode.Mono:
					draw = file => Painting.MakeMonochrome(new Bitmap(new Bitmap(file), Console.WindowWidth, Console.WindowHeight - 1), (Palette.Color)color).Paint();
					break;
				
				case ColorMode.Duo:
					draw = file => Painting.MakeDuochrome(new Bitmap(new Bitmap(file), Console.WindowWidth, Console.WindowHeight - 1), (Palette.DuoColor)color).Paint();
					break;
					
				default:
					
					if (color == 1)
						draw = file => Painting.DrawImage(new Bitmap(new Bitmap(file), Console.WindowWidth, Console.WindowHeight - 1), 0, 0);
					
					else
						draw = file => Painting.DrawDarkImage(new Bitmap(new Bitmap(file), Console.WindowWidth, Console.WindowHeight - 1), 0, 0);
					break;
			}
			
			foreach (var file in files)
				draw(file);
			
			audio.Stop();
			audio.Dispose();
			Console.ReadKey(true);
		}
	}
}