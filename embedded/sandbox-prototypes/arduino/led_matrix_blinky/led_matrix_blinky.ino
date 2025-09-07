#include "Arduino_LED_Matrix.h"

ArduinoLEDMatrix matrix;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  matrix.begin();
}

uint8_t frame[8][12] = {

  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },

  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },

  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },

  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },

  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },

  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },

  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },

  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }

};


void leftEye(){

  //Left eye
  frame[1][3] = 1;
  frame[1][4] = 1;
  frame[2][3] = 1;
  frame[2][4] = 1;

}


void wink(int row1, int row2, int col1, int col2){

  //Wink

  frame[row1][col1] = 0;
  frame[row1][col2] = 0;
  frame[row2][col1] = 1;
  frame[row2][col2] = 1;

}


void rightEye(){

  //Right eye
  frame[1][8] = 1;
  frame[1][9] = 1;
  frame[2][8] = 1;
  frame[2][9] = 1;

}


void mouth(){

  //Mouth
  frame[5][3] = 1;
  frame[5][9] = 1;
  frame[6][3] = 1;
  frame[6][4] = 1;
  frame[6][5] = 1;
  frame[6][6] = 1;
  frame[6][7] = 1;
  frame[6][8] = 1;
  frame[6][9] = 1;

}

void loop() {
  // put your main code here, to run repeatedly:
  leftEye();
  rightEye();
  mouth();

  matrix.renderBitmap(frame, 8, 12);

  delay(1000);
  wink(1,2,3,4);
  wink(1,2,8,9);


  matrix.renderBitmap(frame, 8, 12);
  delay(1000);
}
