Arduino project that shows cpu/gpu usage and temperature on an lcd screen using the openhardwaremonitor library 

>this project consists of 2 different sets of code
>
>- the pc side code, in c# which gathers the system info and outputs it over a com port. 
>
>- the arduino side code, which splits up the stream of data received and displays it on the lcd.

> the com port and refresh rate of the outputted data can be altered in the c# code, the brightness of the lcd can be changed in the c++ code

>I've also set the c# code up to run on startup, as a system process 
>(which makes it hidden and shows up under background processes in task manager) 
>This can be done using the windows task scheduler.
