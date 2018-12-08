#!/usr/bin/env python
import signal
import sys
import socket
import wifiReader
#from time import sleep


#print('Press Ctrl+C')
#signal.pause()

TCP_IP = '192.168.2.45'
TCP_PORT = 5005
BUFFER_SIZE = 1024

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((TCP_IP, TCP_PORT))
s.listen(1)


#print datatcp
conn, addr = s.accept()
print 'Connection address:', addr
while 1:
    datatcp = wifiReader.main()
    data = conn.recv(BUFFER_SIZE)
    if not data:
        break
    #print "received data:", data
    print datatcp
    conn.send(str(datatcp))  # echo
conn.close()

def signal_handler(sig, frame):
        print('You pressed Ctrl+C!')
        conn.close()
        sys.exit(0)
signal.signal(signal.SIGINT, signal_handler)
