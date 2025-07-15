#ifndef TASK_TIMER_H
#define TASK_TIMER_H

#include <chrono>

//DECLARATION OF TIMER CLASS

class TaskTimer {
    private:
        // Properties
        unsigned long currentTimeMillis;
        unsigned long currentDuration;
        std::chrono::time_point<std::chrono::steady_clock> startTimeChrono;
        std::chrono::time_point<std::chrono::steady_clock> currentTimeChrono;

        // Default Constructor
        TaskTimer();

        // Methods
        void setCurrentTime();
        void refreshTimer(bool isPassedDuration);
        
    public:
        // Properties
        unsigned long startTimeMillis;
        unsigned long maxPeriodTimeMillis;
        bool isItTaskTime = false;

        // Public Delegating Constructor
        TaskTimer(unsigned long maxPerMillis);
        
        // Methods
        void setMaxPeriod(unsigned long maxPerMillis);
        bool checkisItTaskTime();
};

#endif