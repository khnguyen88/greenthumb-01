#include <NewPing.h>

const int trigPin = 9;
const int echoPin = 10;
const int maxDistance = 200; //cm

float duration, distance;

NewPing sonar(trigPin, echoPin, maxDistance);

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  //Ultrasonic Sensor Setup
  pinMode(trigPin, OUTPUT); //needed if not using newping library
  pinMode(echoPin, INPUT);  //needed if not using newping library
}

void loop() {
  // put your main code here, to run repeatedly:

  // traditional method, very inaccurate or inconsistent
  sonarDistance(trigPin, echoPin);

  // Use NewPing library
  // delay(1000);
  // Serial.print("Ping: ");
  // Serial.print(sonar.ping_cm()/2.54);
  // Serial.println("in");
}

void sonarDistance(int trigP, int echoP){
  digitalWrite(trigP, LOW); //clears the trigger pin
  delayMicroseconds(2);
  digitalWrite(trigP, HIGH); // activate the trigger pin
  delayMicroseconds(10);
  digitalWrite(trigP, LOW); // turn off the trigger pin

  duration = pulseIn(echoP, HIGH); // read the echo pins, return the soundwave travel time in microseconds
  distance = (duration*.0343)/2; //calculate for distance in cm
  if(distance > 0){
    Serial.print("Distance: ");
    Serial.print(distance/2.54); //converts cm to in, 1 in = 2.5 cm
    Serial.println(" in");
  }

  delay(1000);
}
