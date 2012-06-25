// Copyright (C) 2011 MadCow Project
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

// Used code from http://www.dreamincode.net/code/snippet1568.htm.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Nini.Config;

namespace MadCow
{
    class Helper
    {
        //For MadCow configuration loading settings.
        public static void Helpers()
        {
            if (File.Exists(Program.madcowINI))
            {
                IConfigSource source = new IniConfigSource(Program.madcowINI);
                String BalloonTips = source.Configs["Balloons"].Get("ShowBalloons");

                Form1.GlobalAccess.TrayNotificationsStatusLabel.ResetText();

                if (BalloonTips.Contains("1"))
                {
                    Form1.GlobalAccess.TrayNotificationsStatusLabel.Text = "Enabled";
                    Form1.GlobalAccess.TrayNotificationsStatusLabel.ForeColor = Color.SeaGreen;
                }
                else
                {
                    Form1.GlobalAccess.TrayNotificationsStatusLabel.Text = "Disabled";
                    Form1.GlobalAccess.TrayNotificationsStatusLabel.ForeColor = Color.DimGray;
                }

                String Shorcut = source.Configs["ShortCut"].Get("Shortcut");
                Form1.GlobalAccess.SrtCutStatusLabel.ResetText();

                if (Shorcut.Contains("1"))
                {
                    Form1.GlobalAccess.SrtCutStatusLabel.Text = "Enabled";
                    Form1.GlobalAccess.SrtCutStatusLabel.ForeColor = Color.SeaGreen;
                }
                else
                {
                    Form1.GlobalAccess.SrtCutStatusLabel.Text = "Disabled";
                    Form1.GlobalAccess.SrtCutStatusLabel.ForeColor = Color.DimGray;
                }

                String LastRepo = source.Configs["LastPlay"].Get("Enabled");
                Form1.GlobalAccess.RememberLastRepoStatusLabel.ResetText();

                if (LastRepo.Contains("1"))
                {
                    Form1.GlobalAccess.RememberLastRepoStatusLabel.Text = "Enabled";
                    Form1.GlobalAccess.RememberLastRepoStatusLabel.ForeColor = Color.SeaGreen;
                    Form1.GlobalAccess.LastPlayedRepoReminderLabel.Visible = true;
                }
                else
                {
                    Form1.GlobalAccess.RememberLastRepoStatusLabel.Text = "Disabled";
                    Form1.GlobalAccess.RememberLastRepoStatusLabel.ForeColor = Color.DimGray;
                    Form1.GlobalAccess.LastPlayedRepoReminderLabel.Visible = false;
                }

                String TrayIcon = source.Configs["Tray"].Get("Enabled");
                Form1.GlobalAccess.MinimizeTrayStatusLabel.ResetText();

                if (TrayIcon.Contains("1"))
                {
                    Form1.GlobalAccess.MinimizeTrayStatusLabel.Text = "Enabled";
                    Form1.GlobalAccess.MinimizeTrayStatusLabel.ForeColor = Color.SeaGreen;
                    Form1.GlobalAccess.loadTrayMenu();//Loading the contextMenu for trayIcon    
                }
                else
                {
                    Form1.GlobalAccess.MinimizeTrayStatusLabel.Text = "Disabled";
                    Form1.GlobalAccess.MinimizeTrayStatusLabel.ForeColor = Color.DimGray;
                }
            }
        }

        //MadCowUpdater process killer.
        public static void KillUpdater()
        {
            if (ProcessFinder.FindProcess("MadCowUpdater") == true)
            {
                ProcessFinder.KillProcess("MadCowUpdater");
            }
        }

        //Check for internet Connection.
        public static void CheckForInternet()
        {
            Ping ping = new Ping();
            ping.StartPingTest();
        }

        //Create default need folders.
        public static void DefaultFolderCreator()
        {
            if (Directory.Exists(Program.programPath + @"\MPQ\base") == false)
            {
                Directory.CreateDirectory(Program.programPath + @"\MPQ\base");
            }
            if (Directory.Exists(Program.programPath + @"\Repositories") == false)
            {
                Directory.CreateDirectory(Program.programPath + @"\Repositories");
            }
            if (Directory.Exists(Program.programPath + @"\RuntimeDownloads") == false)
            {
                Directory.CreateDirectory(Program.programPath + @"\RuntimeDownloads");
            }
            //Apart from creating ServerProfiles folder, we write the Defualt profile.
            if (Directory.Exists(Program.programPath + @"\ServerProfiles") == false)
            {
                Directory.CreateDirectory(Program.programPath + @"\ServerProfiles");
                TextWriter tw = new StreamWriter(Program.programPath + @"\ServerProfiles\Default.mdc");
                //tw.WriteLine("Bnet Server Ip");
                tw.WriteLine("0.0.0.0");
                //tw.WriteLine("Game Server Ip");
                tw.WriteLine("0.0.0.0");
                //tw.WriteLine("Public Server Ip");
                tw.WriteLine("0.0.0.0");
                //tw.WriteLine("Bnet Server Port");
                tw.WriteLine("1119");
                //tw.WriteLine("Game Server Port");
                tw.WriteLine("1999");
                //tw.WriteLine("MOTD");
                tw.WriteLine("Welcome to mooege development server!");
                //tw.WriteLine("NAT");
                tw.WriteLine("False");
                tw.Close();
            }
        }
    }
}
