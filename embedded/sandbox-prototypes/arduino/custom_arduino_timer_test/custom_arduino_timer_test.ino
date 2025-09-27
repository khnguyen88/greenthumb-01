#include "arduino_task_timer.h"

// Create three timers with different intervals
ArduinoTaskTimer timer1(60000);  // 1 minute
ArduinoTaskTimer timer2(30000);  // 30 seconds
ArduinoTaskTimer timer3(10000);  // 10 seconds

void setup() {
  Serial.begin(9600);
}

void loop() {
  // Check and act on timer1
  // One general timer can be used to publish data (send data)
  if (timer1.checkIsItTaskTime()) {
    Serial.println("Timer 1: 1 minute passed");
  }

  // Check and act on timer2
  // One general timer can be used to subscribe data (recieve data)
  if (timer2.checkIsItTaskTime()) {
    Serial.println("Timer 2: 30 seconds passed");
  }

  // Check and act on timer3
  // One general timer can be used to update sensor reading
  if (timer3.checkIsItTaskTime()) {
    Serial.println("Timer 3: 10 seconds passed");
  }
}