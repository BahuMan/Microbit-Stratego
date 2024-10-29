# Collection of Unity games using Microbit as controller

## Stratego

Each stratego player carries a microbit with their "rank" (general, sapper, flag, bomb) encoded.
When a player gets killed, they return to base where the PC running Unity app is located.
Connect the microbit to be revived
PC (unity) game will keep track of how many lives or each rank have been handed out.
When PC runs out of ranks to hand out, player can't be revived and is dead.

## HorseRace

Carnival horse race game.
One microbit is connected via USB to PC, running "horse race-PC.py"
All other microbits need "horse race participant.py", and with a custom name
participants can press the "B" button to change their looks
whent the race starts, participants press the "A" button as quickly as they can to move their horse and win the race