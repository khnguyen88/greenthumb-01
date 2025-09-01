#include "chrono_task_timer.h"
#include <chrono>

//IMPLEMENTATION OF TIMER CLASS

// Constructor
// Default Constructor
ChronoTaskTimer::ChronoTaskTimer() :
    startChronoTime(getChronoTimeAtNow()),
    currentChronoTime(getChronoTimeAtNow())
{
    maxPeriodTimeMillis = 1000;
    isItTaskTime = false;

    startChronoDurationSinceEpoch = castChronoTimeToDurationAtEpoch(startChronoTime);
    startChronoDurationSinceEpochInMillis = castChronoDurationToMillisLongLong(startChronoDurationSinceEpoch);
    startTimeMillis = startChronoDurationSinceEpochInMillis;

    currentChronoDurationSinceEpoch = castChronoTimeToDurationAtEpoch(currentChronoTime);
    currentChronoDurationSinceEpochInMillis = castChronoDurationToMillisLongLong(currentChronoDurationSinceEpoch);
    currentTimeMillis = currentChronoDurationSinceEpochInMillis;
}


// Public Delegating Constructor
ChronoTaskTimer::ChronoTaskTimer(unsigned long maxPerMillis):
    startChronoTime(getChronoTimeAtNow()),
    currentChronoTime(getChronoTimeAtNow())
{
    maxPeriodTimeMillis = maxPerMillis;
    isItTaskTime = false;

    startChronoDurationSinceEpoch = castChronoTimeToDurationAtEpoch(startChronoTime);
    startChronoDurationSinceEpochInMillis = castChronoDurationToMillisLongLong(startChronoDurationSinceEpoch);
    startTimeMillis = startTimeMillis = castChronoDurationMillisLongLongToUnsignedLong(startChronoDurationSinceEpochInMillis);

    currentChronoDurationSinceEpoch = castChronoTimeToDurationAtEpoch(currentChronoTime);
    currentChronoDurationSinceEpochInMillis = castChronoDurationToMillisLongLong(currentChronoDurationSinceEpoch);
    currentTimeMillis = castChronoDurationMillisLongLongToUnsignedLong(currentChronoDurationSinceEpochInMillis);
}

void ChronoTaskTimer::syncStartTime(unsigned long otherStartTimeMillis) {
    startTimeMillis = otherStartTimeMillis;
}

void ChronoTaskTimer::setMaxPeriod(unsigned long maxPerMillis) {
    maxPeriodTimeMillis = maxPerMillis;
}

void ChronoTaskTimer::setCurrentDuration(){
    currentDurationMillis = currentTimeMillis - startTimeMillis;
}


bool ChronoTaskTimer::checkIsItTaskTime(){
    refreshTimer(isItTaskTime);

    bool condition = (currentDurationMillis >= maxPeriodTimeMillis);

    isItTaskTime = condition;

    return condition;
}

void ChronoTaskTimer::setCurrentTime(){
    currentChronoTime = getChronoTimeAtNow();;
    currentChronoDurationSinceEpoch = castChronoTimeToDurationAtEpoch(currentChronoTime);
    currentChronoDurationSinceEpochInMillis = castChronoDurationToMillisLongLong(currentChronoDurationSinceEpoch);
    currentTimeMillis = castChronoDurationMillisLongLongToUnsignedLong(currentChronoDurationSinceEpochInMillis);
}

void ChronoTaskTimer::refreshTimer(bool isPassedDuration){
    if(isPassedDuration){
        startChronoTime = currentChronoTime;
        startChronoDurationSinceEpoch = currentChronoDurationSinceEpoch;
        startChronoDurationSinceEpochInMillis = currentChronoDurationSinceEpochInMillis;
        startTimeMillis = castChronoDurationMillisLongLongToUnsignedLong(startChronoDurationSinceEpochInMillis);

    }
    setCurrentTime();
    setCurrentDuration();
}

unsigned long ChronoTaskTimer::getStartTimeMillis(){
    return startTimeMillis;
};

unsigned long ChronoTaskTimer::getMaxPeriodTimeMillis(){
    return maxPeriodTimeMillis;
};

unsigned long ChronoTaskTimer::getCurrentTimeMillis(){
    return currentTimeMillis;
};

unsigned long ChronoTaskTimer::getCurrentDuration(){
    return currentDurationMillis;
};