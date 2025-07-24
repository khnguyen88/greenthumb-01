#ifndef TASK_TIMER_H
#define TASK_TIMER_H

#include <chrono>

//DECLARATION OF TIMER CLASS

class TaskTimer {
    private:
        // Properties
        unsigned long currentDurationMillis;
        unsigned long currentTimeMillis;
        unsigned long startTimeMillis;
        unsigned long maxPeriodTimeMillis;

        std::chrono::time_point<std::chrono::steady_clock> startChronoTime;
        std::chrono::steady_clock::duration startChronoDurationSinceEpoch;
        long long startChronoDurationSinceEpochInMillis;

        std::chrono::time_point<std::chrono::steady_clock> currentChronoTime;
        std::chrono::steady_clock::duration currentChronoDurationSinceEpoch;
        long long currentChronoDurationSinceEpochInMillis;

        // Default Constructor
        TaskTimer();

        // Methods
        void setCurrentTime();
        void setCurrentDuration();
        void refreshTimer(bool isPassedDuration);

    public:
        // Properties
        bool isItTaskTime;

        // Public Delegating Constructor
        TaskTimer(unsigned long maxPerMillis);
        
        // Methods
        void syncStartTime(unsigned long otherStartTimeMillis);
        void setMaxPeriod(unsigned long maxPerMillis);
        bool checkIsItTaskTime();

        unsigned long getStartTimeMillis();
        unsigned long getMaxPeriodTimeMillis();
        unsigned long getCurrentTimeMillis();
        unsigned long getCurrentDuration();
};

#endif