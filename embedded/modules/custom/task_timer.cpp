#include "task_timer.h"
#include "chrono_ext.cpp"
#include <chrono>


//IMPLEMENTATION OF TIMER CLASS

// Constructor
// Default Constructor
TaskTimer::TaskTimer() :
    currentTimeMillis(0), 
    maxPeriodTimeMillis(1000), //default at 1 sec
    startChronoTime(getChronoTimeAtNow()),
    currentChronoTime(getChronoTimeAtNow()),
    isItTaskTime(false)
{
    startChronoDurationSinceEpoch = getChronoDurationSinceEpoch(startChronoTime);
    startChronoDurationSinceEpochInMillis = getChronoDurationSinceEpochInMillisLongLong(startChronoDurationSinceEpoch);
    startTimeMillis = startChronoDurationSinceEpochInMillis;

    currentChronoDurationSinceEpoch = getChronoDurationSinceEpoch(currentChronoTime);
    currentChronoDurationSinceEpochInMillis = getChronoDurationSinceEpochInMillisLongLong(currentChronoDurationSinceEpoch);
    currentTimeMillis = currentChronoDurationSinceEpochInMillis;
}

// Public Delegating Constructor
TaskTimer::TaskTimer(unsigned long maxPerMillis):
    startChronoTime(getChronoTimeAtNow()),
    currentChronoTime(getChronoTimeAtNow()),
    isItTaskTime(false)
{
    maxPeriodTimeMillis = maxPerMillis;

    startChronoDurationSinceEpoch = getChronoDurationSinceEpoch(startChronoTime);
    startChronoDurationSinceEpochInMillis = getChronoDurationSinceEpochInMillisLongLong(startChronoDurationSinceEpoch);
    startTimeMillis = startTimeMillis = getChronoDurationSinceEpochInMillisUnsignedLong(startChronoDurationSinceEpochInMillis);

    currentChronoDurationSinceEpoch = getChronoDurationSinceEpoch(currentChronoTime);
    currentChronoDurationSinceEpochInMillis = getChronoDurationSinceEpochInMillisLongLong(currentChronoDurationSinceEpoch);
    currentTimeMillis = getChronoDurationSinceEpochInMillisUnsignedLong(currentChronoDurationSinceEpochInMillis);
}

void TaskTimer::setMaxPeriod(unsigned long maxPerMillis) {
    maxPeriodTimeMillis = maxPerMillis;
}

void TaskTimer::setCurrentDuration(){
    currentDurationMillis = currentTimeMillis - startTimeMillis;
}


bool TaskTimer::checkIsItTaskTime(){
    refreshTimer(isItTaskTime);

    bool condition = (currentDurationMillis >= maxPeriodTimeMillis);

    isItTaskTime = condition;

    return condition;
}

void TaskTimer::setCurrentTime(){
    currentChronoTime = getChronoTimeAtNow();;
    currentChronoDurationSinceEpoch = getChronoDurationSinceEpoch(currentChronoTime);
    currentChronoDurationSinceEpochInMillis = getChronoDurationSinceEpochInMillisLongLong(currentChronoDurationSinceEpoch);
    currentTimeMillis = getChronoDurationSinceEpochInMillisUnsignedLong(currentChronoDurationSinceEpochInMillis);
}

void TaskTimer::refreshTimer(bool isPassedDuration){
    if(isPassedDuration){
        startChronoTime = currentChronoTime;
        startChronoDurationSinceEpoch = currentChronoDurationSinceEpoch;
        startChronoDurationSinceEpochInMillis = currentChronoDurationSinceEpochInMillis;
        startTimeMillis = getChronoDurationSinceEpochInMillisUnsignedLong(startChronoDurationSinceEpochInMillis);

    }
    setCurrentTime();
    setCurrentDuration();
}

unsigned long TaskTimer::getStartTimeMillis(){
    return startTimeMillis;
};

unsigned long TaskTimer::getMaxPeriodTimeMillis(){
    return maxPeriodTimeMillis;
};

unsigned long TaskTimer::getCurrentTimeMillis(){
    return currentTimeMillis;
};

unsigned long TaskTimer::getCurrentDuration(){
    return currentDurationMillis;
};