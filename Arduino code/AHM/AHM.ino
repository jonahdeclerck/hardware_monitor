#include<LiquidCrystal_I2C.h>
LiquidCrystal_I2C lcd(0x27,16 ,2);

#define b A3

String inData;
int brightness;

void setup() {

    brightness = 200;  //0 -> 1023    0=off 25=low 100=mid 1023=high
    pinMode(b, OUTPUT);
    analogWrite(b , brightness);
    
    Serial.begin(9600);
    lcd.begin(16,4);
    lcd.backlight();

    //lcd.begin(16,2);
    lcd.setCursor(1,0);
    lcd.print("CPU: ");
    lcd.setCursor(7,0);
    lcd.print("  % ");
    lcd.setCursor(12,0);
    lcd.print("  ""\xDF");
 
    lcd.setCursor(1,1);
    lcd.print("GPU: ");
    lcd.setCursor(7,1);
    lcd.print("  % ");
    lcd.setCursor(12,1);
    lcd.print("  ""\xDF");
}

void loop() {

    while (Serial.available() > 0)
    {
        char recieved = Serial.read();
        inData += recieved; 


         if(recieved == 'A'){
            inData.remove(inData.length() -1,1);
            lcd.setCursor(7,0);
            lcd.print("  ");
            lcd.print("% ");
            lcd.setCursor(7,0);
            lcd.print(inData);
            inData="";                
          }
        if(recieved == 'B'){
            inData.remove(inData.length() - 1, 1);
            lcd.setCursor(12,0);
            lcd.print(inData);         
            inData ="";
          }
        if(recieved == 'C'){
            inData.remove(inData.length() -1,1);
            lcd.setCursor(7,1);
            lcd.print("  ");
            lcd.print("% ");
            lcd.setCursor(7,1);
            lcd.print(inData);
            inData="";                
          }
        if (recieved == 'D'){   
            inData.remove(inData.length() - 1, 1);           
            lcd.setCursor(12,1);
            lcd.print(inData);
            inData = ""; 
        }
    }
}
