#include "arduino_task_timer.h"
#include <Arduino.h>

//IMPLEMENTATION OF TIMER CLASS

// Constructor
// Default Constructor
ArduinoTaskTimer::ArduinoTaskTimer()
{
    maxPeriodTimeMillis = 1000;
    isItTaskTime = false;

    startTimeMillis = millis();
    currentTimeMillis = millis();
}


// Public Delegating Constructor
ArduinoTaskTimer::ArduinoTaskTimer(unsigned long maxPerMillis)
{
    maxPeriodTimeMillis = maxPerMillis;
    isItTaskTime = false;

    startTimeMillis = millis();
    currentTimeMillis = millis();
}

void ArduinoTaskTimer::syncStartTime(unsigned long otherStartTimeMillis) {
    startTimeMillis = otherStartTimeMillis;
}

void ArduinoTaskTimer::setMaxPeriod(unsigned long maxPerMillis) {
    maxPeriodTimeMillis = maxPerMillis;
}

void ArduinoTaskTimer::setCurrentDuration(){
    currentDurationMillis = currentTimeMillis - startTimeMillis;
}


bool ArduinoTaskTimer::checkIsItTaskTime(){
    refreshTimer(isItTaskTime);

    bool condition = (currentDurationMillis >= maxPeriodTimeMillis);

    isItTaskTime = condition;

    return condition;
}

void ArduinoTaskTimer::setCurrentTime(){
    currentTimeMillis = millis();
}

void ArduinoTaskTimer::refreshTimer(bool isPassedDuration){
    if(isPassedDuration){
        startTimeMillis = currentTimeMillis;

    }
    setCurrentTime();
    setCurrentDuration();
}

//For force reset, should not be needed
void ArduinoTaskTimer::resetTaskConditions(){
    isItTaskTime = false;

    startTimeMillis = millis();
    currentTimeMillis = millis();
}

unsigned long ArduinoTaskTimer::getStartTimeMillis(){
    return startTimeMillis;
};

unsigned long ArduinoTaskTimer::getMaxPeriodTimeMillis(){
    return maxPeriodTimeMillis;
};

unsigned long ArduinoTaskTimer::getCurrentTimeMillis(){
    return currentTimeMillis;
};

unsigned long ArduinoTaskTimer::getCurrentDuration(){
    return currentDurationMillis;
};