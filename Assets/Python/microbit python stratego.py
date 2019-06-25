from microbit import *
import radio

status = "dood"
verkennerLoop = 1
verkennerLed = 0
radio.on()
radio.config(power=3, channel=25, data_rate=radio.RATE_250KBIT)
uart.init(115200)

img_dood = Image("90009:"
                 "09090:"
                 "00900:"
                 "09090:"
                 "90009:")

img_verkenner = Image("00000:"
                      "09990:"
                      "90909:"
                      "09990:"
                      "00000:")

img_bom = Image("00090:"
                "00900:"
                "09990:"
                "09990:"
                "09990:")

img_vlag = Image("90000:"
                 "99000:"
                 "99900:"
                 "99990:"
                 "90000:")

def show_status(stat="dood"):
    if stat == "dood":
        display.show(img_dood)
    elif stat == "verkenner":
        display.show(img_verkenner)
    elif stat == "bom":
        display.show(img_bom)
    elif stat == "vlag":
        display.show(img_vlag)

def verkenner_running_light():
    global verkennerLed
    global verkennerLoop
    display.set_pixel(verkennerLed, 0, 0)
    verkennerLed += verkennerLoop
    if verkennerLed > 4:
        verkennerLed = 4
        verkennerLoop = -1
    elif verkennerLed < 0:
        verkennerLed = 0
        verkennerLoop = 1
    display.set_pixel(verkennerLed, 0, 9)
    sleep(100)

def verkenner_scan():
    global status
    if status != "verkenner":
        return

    scan = radio.receive()
    if scan is not None:
        show_status(scan)
    verkenner_running_light()

def check_buttons():
    global status
    if button_b.is_pressed():
        sleep(200)  # check voor 2 knoppen tegelijk
        if button_a.is_pressed():
            status = "dood"
        else:
            status = "vlag"
    elif button_a.is_pressed():
        sleep(200)  # check voor 2 knoppen tegelijk
        if button_b.is_pressed():
            status = "dood"
        else:
            status = "verkenner"

def lees_serial():
    global status
    if (uart.any()):
        gelezen = uart.readline().strip()
        if (gelezen != "check"):
            status = gelezen
    print(status)

while True:
    check_buttons()
    lees_serial()

    if status != "dood":
        radio.send(status)
    if status == "verkenner":
        verkenner_scan()
    else:
        show_status(status)