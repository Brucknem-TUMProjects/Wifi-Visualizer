#!/usr/bin/env python
import sys
import signal
import subprocess
from sense_hat import SenseHat
sense = SenseHat()
sense.set_rotation(180)
process = None;

def start_server():
    process = subprocess.Popen([sys.executable, "tcp.py"])
    print process
    return process

def signal_handler(sig, frame):
    print('\nEnding joystick.py')
    if process is not None:
        process.send_signal(signal.SIGINT)
    sense.show_message('Ending joystick script', scroll_speed=0.05)
    sense.clear()
    sys.exit(0)
signal.signal(signal.SIGINT, signal_handler)
signal.signal(signal.SIGTERM, signal_handler)

sense.show_message('Starting joystick script', scroll_speed=0.05)
while True:
    for event in sense.stick.get_events():
        print(event.direction, event.action)

        if event.action == "pressed":
            # Check which direction
            if event.direction == "up":
                if process is None:
                    process = start_server()
            elif event.direction == "down":
                if process is not None:
                    process.send_signal(signal.SIGINT)
                    process = None
            elif event.direction == "left":
                pass
            elif event.direction == "right":
                pass
            elif event.direction == "middle":
                pass
