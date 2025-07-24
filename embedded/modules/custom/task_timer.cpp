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
    startChronoDurationSinceEpoch = castChronoTimeToDurationAtEpoch(startChronoTime);
    startChronoDurationSinceEpochInMillis = castChronoDurationToMillisLongLong(startChronoDurationSinceEpoch);
    startTimeMillis = startChronoDurationSinceEpochInMillis;

    currentChronoDurationSinceEpoch = castChronoTimeToDurationAtEpoch(currentChronoTime);
    currentChronoDurationSinceEpochInMillis = castChronoDurationToMillisLongLong(currentChronoDurationSinceEpoch);
    currentTimeMillis = currentChronoDurationSinceEpochInMillis;
}


// Public Delegating Constructor
TaskTimer::TaskTimer(unsigned long maxPerMillis):
    startChronoTime(getChronoTimeAtNow()),
    currentChronoTime(getChronoTimeAtNow()),
    isItTaskTime(false)
{
    maxPeriodTimeMillis = maxPerMillis;

    startChronoDurationSinceEpoch = castChronoTimeToDurationAtEpoch(startChronoTime);
    startChronoDurationSinceEpochInMillis = castChronoDurationToMillisLongLong(startChronoDurationSinceEpoch);
    startTimeMillis = startTimeMillis = castChronoDurationMillisLongLongToUnsignedLong(startChronoDurationSinceEpochInMillis);

    currentChronoDurationSinceEpoch = castChronoTimeToDurationAtEpoch(currentChronoTime);
    currentChronoDurationSinceEpochInMillis = castChronoDurationToMillisLongLong(currentChronoDurationSinceEpoch);
    currentTimeMillis = castChronoDurationMillisLongLongToUnsignedLong(currentChronoDurationSinceEpochInMillis);
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
    currentChronoDurationSinceEpoch = castChronoTimeToDurationAtEpoch(currentChronoTime);
    currentChronoDurationSinceEpochInMillis = castChronoDurationToMillisLongLong(currentChronoDurationSinceEpoch);
    currentTimeMillis = castChronoDurationMillisLongLongToUnsignedLong(currentChronoDurationSinceEpochInMillis);
}

void TaskTimer::refreshTimer(bool isPassedDuration){
    if(isPassedDuration){
        startChronoTime = currentChronoTime;
        startChronoDurationSinceEpoch = currentChronoDurationSinceEpoch;
        startChronoDurationSinceEpochInMillis = currentChronoDurationSinceEpochInMillis;
        startTimeMillis = castChronoDurationMillisLongLongToUnsignedLong(startChronoDurationSinceEpochInMillis);

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