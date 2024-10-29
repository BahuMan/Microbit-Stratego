# run this script on any number of microbits
# they will use radio signals to participate
from microbit import *
import radio

radio.on()
radio.config(power=3, channel=35, data_rate=radio.RATE_1MBIT)
# Code in a 'while True:' loop repeats forever
while True:
    if button_a.is_pressed():
        radio.send("bart hop")
        display.show(Image.HEART_SMALL)
        sleep(10)
    else:
        radio.send("bart los")
    if button_b.is_pressed():
        radio.send("bart looks")
        display.show(Image.FABULOUS)
        sleep(10)
    display.show(Image.HEART)
    sleep(10)