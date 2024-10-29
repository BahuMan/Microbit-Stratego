# run this script on exactly one microbit, connected to the PC
# via USB cable. It will receive radio messages from other
# microbits and transfer them to the PC over USB
from microbit import *
import radio

radio.on()
radio.config(power=3, channel=35, data_rate=radio.RATE_1MBIT)
uart.init(115200)
img_connect = Image( "00000:"
                     "00000:"
                     "99099:"
                     "00000:"
                     "00000:")

def connectPC():
    display.show(image = img_connect, delay=400, wait=True, clear=False)
    
    while True:
        if uart.any():
            gelezen = str(uart.readline())
            if gelezen.find("horserace") >= 0:
                uart.write("letsgo\r\n")
                return True
            else:
                display.scroll(gelezen)
        #invert pixel in the middle
        display.set_pixel(2,2, 9-display.get_pixel(2,2))
        sleep(200);


while True:
    if connectPC():
        display.scroll("connected")
        sleep(1000)