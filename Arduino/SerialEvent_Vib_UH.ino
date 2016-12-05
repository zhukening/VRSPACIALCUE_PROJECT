/*
  Serial Event example

 When new serial data arrives, this sketch adds it to a String.
 When a newline is received, the loop prints the string and
 clears it.

 A good test for this is to try it with a GPS receiver
 that sends out NMEA 0183 sentences.

 Created 9 May 2011
 by Tom Igoe

 This example code is in the public domain.

 http://www.arduino.cc/en/Tutorial/SerialEvent

 */

#define DELAYGAP 1000

String inputString = "";         // a string to hold incoming data
boolean stringComplete = false;  // whether the string is complete
byte direction = 0;

void setup() {
  // initialize serial:
  Serial.begin(115200);
  // reserve 200 bytes for the inputString:
  //inputString.reserve(200);
  pinMode(13,OUTPUT);
  pinMode(5,OUTPUT);
  pinMode(6,OUTPUT);
}

void loop() {
  
  if (Serial.available()){
      direction = Serial.read();
  }
  // trigger the feedback
  feedback();
}

void feedback(){
  switch(direction){
      case '1':           //front left
        analogWrite(A0, 255);
        analogWrite(A1, 0);
        analogWrite(A2, 0);
        analogWrite(A3, 0);
        analogWrite(A4, 0);
        analogWrite(A5, 0);
        digitalWrite(5, LOW);
        digitalWrite(6, LOW);
        break;
      case '2':           //left
        analogWrite(A0, 0);
        analogWrite(A1, 255);
        analogWrite(A2, 0);
        analogWrite(A3, 0);
        analogWrite(A4, 0);
        analogWrite(A5, 0);
        digitalWrite(5, LOW);
        digitalWrite(6, LOW);
        break;
      case '3':           //back left
        analogWrite(A0, 0);
        analogWrite(A1, 0);
        analogWrite(A2, 255);
        analogWrite(A3, 0);
        analogWrite(A4, 0);
        analogWrite(A5, 0);
        digitalWrite(5, LOW);
        digitalWrite(6, LOW);
        break;
      case '4':           //back
        analogWrite(A0, 0);
        analogWrite(A1, 0);
        analogWrite(A2, 0);
        analogWrite(A3, 255);
        analogWrite(A4, 0);
        analogWrite(A5, 0);
        digitalWrite(5, LOW);
        digitalWrite(6, LOW);
        break;
      case '5':            //back right
        analogWrite(A0, 0);
        analogWrite(A1, 0);
        analogWrite(A2, 0);
        analogWrite(A3, 0);
        analogWrite(A4, 255);
        analogWrite(A5, 0);
        digitalWrite(5, LOW);
        digitalWrite(6, LOW);
        break;
      case '6':             //right
        analogWrite(A0, 0);
        analogWrite(A1, 0);
        analogWrite(A2, 0);
        analogWrite(A3, 0);
        analogWrite(A4, 0);
        analogWrite(A5, 255);
        digitalWrite(5, LOW);
        digitalWrite(6, LOW);
        break;
      case '7':               //front right
        analogWrite(A0, 0);
        analogWrite(A1, 0);
        analogWrite(A2, 0);
        analogWrite(A3, 0);
        analogWrite(A4, 0);
        analogWrite(A5, 0);
        digitalWrite(5, HIGH);
        digitalWrite(6, LOW);
        break;
      case '8':                 //front
        analogWrite(A0, 0);
        analogWrite(A1, 0);
        analogWrite(A2, 0);
        analogWrite(A3, 0);
        analogWrite(A4, 0);
        analogWrite(A5, 0);
        digitalWrite(5, LOW);
        digitalWrite(6, HIGH);
        break;
      default:  
       analogWrite(A0, 0);
        analogWrite(A1, 0);
        analogWrite(A2, 0);
        analogWrite(A3, 0);
        analogWrite(A4, 0);
        analogWrite(A5, 0);
        digitalWrite(5, LOW);
        digitalWrite(6, LOW);
        break;
  }
}



