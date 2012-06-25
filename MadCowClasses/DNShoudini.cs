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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Management;
using System.Net;

namespace MadCow
{
    public class DNShoudini
    {
        /// <summary>
        /// Checks if the bridged DNS configuration exist, if not it gets changed.
        /// Primary DNS only has authority over us.actual.battle.net.
        /// Secondary DNS will be ur own ISP/Router DNS so browsing stays normal.
        /// </summary>
        public static void checkDNS()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in nics)
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    var ip = Dns.GetHostAddresses("www.d3sharp.com");
                    IPAddressCollection ips = ni.GetIPProperties().DnsAddresses;
                    if (ips[0].ToString().Contains(ip[0].ToString()))
                    {
                        Console.WriteLine("Found correct DNS settings.");
                        break;
                    }
                    SetCustomNameservers(ip[0] + "," + ips[0]);                   
                    break;
                }
            }
        }

        /// <summary>
        /// Set DNS for active NIC.
        /// </summary>
        /// <param name="dnsServers">String of dns ip's divided by ','</param>
        static void SetCustomNameservers(String dnsServers)
        {
            using (var networkConfigMng = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                using (var networkConfigs = networkConfigMng.GetInstances())
                {
                    foreach (var managementObject in networkConfigs.Cast<ManagementObject>().Where(objMO => (bool)objMO["IPEnabled"]))
                    {
                        using (var newDNS = managementObject.GetMethodParameters("SetDNSServerSearchOrder"))
                        {
                            newDNS["DNSServerSearchOrder"] = dnsServers.Split(',');
                            managementObject.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                        }
                    }
                }
            }
            Console.WriteLine("Succesfully modified DNS records.");
        }

        /// <summary>
        /// Restore the DNS to automatic detection.
        /// This function its called on MadCow closing event.
        /// </summary>
        public static void RestoreNameservers()
        {
            using (var networkConfigMng = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                using (var networkConfigs = networkConfigMng.GetInstances())
                {
                    foreach (var managementObject in networkConfigs.Cast<ManagementObject>().Where(objMO => (bool)objMO["IPEnabled"]))
                    {
                        using (var newDNS = managementObject.GetMethodParameters("SetDNSServerSearchOrder"))
                        {
                            newDNS["DNSServerSearchOrder"] = null;
                            managementObject.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                        }
                    }
                }
            }
        }
    }
}
