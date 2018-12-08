#!/bin/sh
# launcher.sh
# navigate to home directory, then to this directory, then execute python script, then back home

cd /home/pi/Wifi-Visualizer/WifiReader
sudo python joystick.py
cd /
