#include "task_timer.h"
#include <chrono>


//IMPLEMENTATION OF TIMER CLASS

// Constructor
// Default Constructor
TaskTimer::TaskTimer() :
    currentTimeMillis(0), 
    maxPeriodTimeMillis(1000), //default at 1 sec
    startTimeChrono(std::chrono::steady_clock::now()),
    currentTimeChrono(std::chrono::steady_clock::now())
{
    startTimeMillis = startTimeChrono.time_since_epoch().count();
    currentTimeMillis = currentTimeChrono.time_since_epoch().count();
}

// Public Delegating Constructor
TaskTimer::TaskTimer(unsigned long maxPerMillis):
    startTimeChrono(std::chrono::steady_clock::now()),
    currentTimeChrono(std::chrono::steady_clock::now())
{
    maxPeriodTimeMillis = maxPerMillis;
    startTimeMillis = startTimeChrono.time_since_epoch().count();
    currentTimeMillis = currentTimeChrono.time_since_epoch().count();
}

void TaskTimer::setMaxPeriod(unsigned long maxPerMillis) {
    maxPeriodTimeMillis = maxPerMillis;
}

bool TaskTimer::checkisItTaskTime(){
    bool condition = (currentTimeMillis - startTimeMillis >= maxPeriodTimeMillis);

    refreshTimer(condition);

    return condition;
}

void TaskTimer::setCurrentTime(){
    currentTimeChrono = std::chrono::steady_clock::now();
    currentTimeMillis = currentTimeChrono.time_since_epoch().count();
}

void TaskTimer::refreshTimer(bool isPassedDuration){
    if(isPassedDuration){
        startTimeMillis = currentTimeMillis;
    }

    setCurrentTime();
}