#!/usr/bin/env python
import signal
import sys
import socket
import wifiReader
from sense_hat import SenseHat
from time import sleep

sense = SenseHat()
conn, addr = None, None

def full_colour(r,g,b):
	sense.clear((r, g, b))
	sleep(1)
	sense.clear()

def quit():
	print('Ending tcp.py')
	global conn
	global sense
	if conn is not None:
		conn.close()
	full_colour(255,0,0)
	sys.exit(0)

def signal_handler(sig, frame):
	quit()
signal.signal(signal.SIGINT, signal_handler)
signal.signal(signal.SIGTERM, signal_handler)


def get_own_ip():
	s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
	s.connect(("8.8.8.8", 80))
	name = s.getsockname()[0]
	s.close()
	return name

TCP_IP = get_own_ip()
TCP_PORT = 5005
BUFFER_SIZE = 1024

sense.set_rotation(180)
sense.show_message(TCP_IP + ":" + str(TCP_PORT), scroll_speed = 0.05)

try:
	s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	s.bind((TCP_IP, TCP_PORT))
	s.listen(1)
except socket.error:
	quit()

full_colour(0,255,0)

#print datatcp
while 1:
	conn, addr = s.accept()
	print 'Connection address:', addr
	sense.show_message(str(addr), text_colour=[0,255,0], scroll_speed = 0.05)
	while 1:
		sense.clear()
		data = conn.recv(BUFFER_SIZE)
		sense.show_letter("X")
		datatcp = wifiReader.main()
		if not data:
			break
		print datatcp
		conn.send(str(datatcp))  # echo
	sense.show_message(str(addr), text_colour=[255,0,0], scroll_speed = 0.05)
	conn.close()
