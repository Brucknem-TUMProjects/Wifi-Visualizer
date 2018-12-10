#!/usr/bin/env python
#

import sys
import subprocess

interface = "wlan0"

# Extracts ESSID, MAC and SIGNAL from "iwconfig wlan0" and returns a string
# (seperated by ";")
def main():

    outputArray = []

    proc = subprocess.Popen(["iwconfig", interface],stdout=subprocess.PIPE, universal_newlines=True)
    out, err = proc.communicate()

# wlan0     IEEE 802.11  ESSID:"FRITZ!Box 7590 TC"
#           Mode:Managed  Frequency:5.18 GHz  Access Point: 44:4E:6D:41:87:E8
#           Bit Rate=200 Mb/s   Tx-Power=31 dBm
#           Retry short limit:7   RTS thr:off   Fragment thr:off
#           Power Management:on
#           Link Quality=62/70  Signal level=-48 dBm
#           Rx invalid nwid:0  Rx invalid crypt:0  Rx invalid frag:0
#           Tx excessive retries:0  Invalid misc:0   Missed beacon:0

    outputArray = out.split("\n")

    ESSID = outputArray[0].split(":")[1]
    ESSID = ESSID.replace("\"", "")
    ESSID = ESSID.strip()

    MAC = outputArray[1].split(": ")[1]
    MAC = MAC.strip()

    SIGNAL = outputArray[5].split("=-")[1]
    SIGNAL = SIGNAL.split(" ")[0]
    SIGNAL = SIGNAL.strip()

    netStat = MAC + ";" + ESSID + ";" + SIGNAL

    print netStat
    return netStat

main()
