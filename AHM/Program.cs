using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using OpenHardwareMonitor.Hardware;
using System.Threading;


namespace AHM
{
    internal class Program
    {
        //editable variables
        
        string portName = "COM5";   //name of the COM port the arduino is connected to, check arduino ide or device manager 
        int delay = 100;           //delay in ms (refresh rate)
        
        SerialPort port;    //initialize port

        static float cpuTemp;
        static float cpuUsage;

        static float gpuTemp;
        static float gpuUsage;



        //create Openhardwaremonitor computer object
        static Computer c = new Computer()
        {
            CPUEnabled = true,  
            GPUEnabled = true,
        };

        static void GatherSystemInfo()
        {
            foreach (var hardware in c.Hardware)
            {
                //Gather cpu info
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    
                    hardware.Update();

                    
                    foreach (var sensor in hardware.Sensors)    //loop through all sensors
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("CPU Package")) // if temp and cpu
                        {
                            // store
                            cpuTemp = sensor.Value.GetValueOrDefault();
                        }
                        else if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("CPU Total")) //if load and cpu
                        {
                            //store
                            cpuUsage = sensor.Value.GetValueOrDefault();
                        }
                    }
                }



                // Gather gpu info
                if (hardware.HardwareType == HardwareType.GpuNvidia)    //use HardwareType.GpuAti' for amd gpu's 
                {

                    hardware.Update();

                    foreach (var sensor in hardware.Sensors)    //loop through all sensors
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("GPU Core"))    //if temp and gpu
                        {
                            // store
                            gpuTemp = sensor.Value.GetValueOrDefault();
                        }
                        else if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU Core"))  //if load and gpu
                        {
                            //store
                            gpuUsage = sensor.Value.GetValueOrDefault();
                        }
                    }
                }
            }
        }


        public void wait()
        {
            Thread.Sleep(delay); //adds delay between sends
        }
        public void output()    //for debug purposes
        {
            //Console.WriteLine("cpu: " + String.Format("{0:00}", Math.Floor(cpuUsage)));     //gpu cores average rounded
            //Console.WriteLine("C: " + cpuUsage);  
            //Console.WriteLine(String.Format("{0:00}", Math.Floor(gpuUsage)));     //gpu load rounded
            //Console.WriteLine("G: " + gpuUsage);      //raw gpu load
            //Console.WriteLine(String.Format("{0:00}", Math.Floor(ramUsage)));     //ram % rounded
            //Console.WriteLine(c1);
        }
        public void send()
        {

            port = new SerialPort(portName, 9600);  //configure serial port using portName and a baud rate of 9600

            try
            {
                port.Open();    //open the serial port
            }
            catch 
            {
                //Console.WriteLine("unable to open serial port");     //for debugging
            }

            if (port.IsOpen)
            {
                //don't send all info in the same port.write(), the arduino needs the tiny delay to separate the string

                //send cpu usage
                port.Write(String.Format("{0:00}", Math.Floor(cpuUsage)) + "A");   
                //send cpu temp
                port.Write(cpuTemp + "B");
                wait();
                //send gpu usage
                port.Write(String.Format("{0:00}", Math.Floor(gpuUsage))+ "C");
                //send gpu temp
                port.Write(gpuTemp + "D");

                ////to be added in arduino code
                //port.Write(String.Format("{0:00}", Math.Floor(ramUsage)) + "D");


            }
            else
            {
                //Console.WriteLine("serial port has not been opened");    //for debugging
            }
            port.Close();   //close the serial port
        }

        static void Main(string[] args)
        {
            c.Open();   //open/initialize computer object
            while (true)  
            {
                GatherSystemInfo(); //gather system info

                Program sendI = new Program();  //make an instance of the send method
                sendI.send();   //send data

                Program outputI = new Program();    //make an instance of the output methtod (debug)
                outputI.output();   //output

                Program waitI = new Program();  //make an instance of the delay method
                waitI.wait();   //delay 

            }
        }
    }
}
